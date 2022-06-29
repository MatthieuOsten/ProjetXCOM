using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Matt_HUDManager_V2 : MonoBehaviour
{
    [Header("DATA")]
    [SerializeField] private PlayerController _pC;
    [SerializeField] private DataCharacter _dataCH;
    [SerializeField] private Character _cH;

    [Header("SCREEN")]
    [SerializeField] private Canvas _canvasHUD;

    [Header("SOUND")]
    [SerializeField] private static string _soundResetSelection = "action_reset";

    [Header("WIDGETS")]
    [SerializeField] private List<Widget> listWidget;
    [SerializeField] private List<DisplayPosition> listDisplayPosition;

    [Space]

    [Header("DEBUG")]
    [SerializeField] private bool _resetPosition;
    [SerializeField] private bool _resetWidgets;

    #region EDITOR
#if UNITY_EDITOR

    private void OnValidate()
    {
        if (_resetPosition == true) {

            _resetPosition = false;

            float camWidth, camHeight;

            if (Camera.current != null)
            {
                 camWidth = Camera.current.pixelWidth;
                 camHeight = Camera.current.pixelHeight;
            } 
            else if (_canvasHUD != null) 
            {
                 camWidth = _canvasHUD.pixelRect.width;
                 camHeight = _canvasHUD.pixelRect.height;
            } 
            else
            {
                 camWidth = 1920;
                 camHeight = 1080;
            }


            string stringERROR = "NULL";

            if (GetDisplayPositionToName("None").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("None", false, -1, new Rect(new Vector2(0, 0), new Vector2(camWidth, camHeight))));
            }
            if (GetDisplayPositionToName("Center").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("Center", false, 1, new Rect(new Vector2(0, 0), new Vector2(camWidth, camHeight))));
            }

            if (GetDisplayPositionToName("Left").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("Left", false, 1, new Rect(new Vector2(0, camHeight / 2), new Vector2(camWidth, camHeight))));
            }
            if (GetDisplayPositionToName("Right").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("Right", false, 1, new Rect(new Vector2(camWidth, camHeight / 2), new Vector2(camWidth, camHeight))));
            }
            if (GetDisplayPositionToName("Top").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("Top", false, 1, new Rect(new Vector2(camWidth / 2, 0), new Vector2(camWidth, camHeight))));
            }
            if (GetDisplayPositionToName("Bottom").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("Bottom", false, 1, new Rect(new Vector2(camWidth / 2, camHeight), new Vector2(camWidth, camHeight))));
            }

            if (GetDisplayPositionToName("TopLeft").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("TopLeft", false, 1, new Rect(new Vector2(0, 0), new Vector2(camWidth, camHeight))));
            }
            if (GetDisplayPositionToName("TopRight").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("TopRight", false, 1, new Rect(new Vector2(0, camHeight), new Vector2(camWidth, camHeight))));
            }
            if (GetDisplayPositionToName("BottomLeft").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("BottomLeft", false, 1, new Rect(new Vector2(camWidth, 0), new Vector2(camWidth, camHeight))));
            }
            if (GetDisplayPositionToName("BottomRight").Name == stringERROR)
            {
                listDisplayPosition.Add(new DisplayPosition("BottomRight", false, 1, new Rect(new Vector2(camWidth, camHeight), new Vector2(camWidth, camHeight))));
            }

        }

        InitialiseWidgets(listWidget);

    }

    private void OnDrawGizmosSelected()
    {
        if (_resetWidgets)
        {
            _resetWidgets = false;

            foreach (var widget in listWidget)
            {
                if (widget.WidgetClass != null && widget.DebugMode)
                    widget.WidgetClass.SystemDebug();
                else if (widget.DebugMode)
                    widget.UpdateClass();
            }

        }

    }

#endif
    #endregion

    public PlayerController ActualPlayer { get { return _pC; } }
    public DataCharacter ActualCharacter { get { return _dataCH; } }
    public Character Character { get { return _cH; } }

    [System.Serializable]
    public struct DisplayPosition
    {
        [SerializeField] private string _name;

        [Header("EMPLACEMENT")]
        [SerializeField] private bool _occupied;
        [SerializeField][Min(-1)] private int _widgetsMAX;
        [SerializeField] private List<string> _listWidgets;

        [Header("ZONE")]
        [SerializeField] private Rect _rect;

        public bool Occupied { get { return _occupied; } set { _occupied = value; } }
        public int WidgetsMAX { get { return _widgetsMAX; }
            set
            {
                if (value >= -1)
                {
                    _widgetsMAX = value;
                } else {
                    _widgetsMAX = 0;
                }
            }
        }

        public string Name { get { return _name; } }
        public Rect RectZone { get { return _rect; } set { _rect = value; } }
        public List<string> ListWidgets
        {
            get { return _listWidgets; }
            set {
                if (value.Count < WidgetsMAX || WidgetsMAX == -1) { _listWidgets = value; }
            }
        }

        public DisplayPosition(string name)
        {
            _name = name;
            _occupied = false;
            _widgetsMAX = 0;
            _listWidgets = new List<string>();
            _rect = new Rect();
        }

        public DisplayPosition(string name, bool occupied, int max, Rect rect) {
            _name = name;
            _occupied = occupied;
            _widgetsMAX = max;
            _listWidgets = new List<string>();
            _rect = rect;


        }

        public void UpdateOccupied()
        {
            if (ListWidgets.Count >= _widgetsMAX) { _occupied = true; } else { _occupied = false; }
        }
    }

    [System.Serializable]
    private class Widget
    {
        [SerializeField] private string _name;

        [SerializeField] private Matt_Widgets _widgetClass;

        [SerializeField] private GameObject _actualObject;
        [SerializeField] private GameObject _prefabObject;
        [SerializeField] private string _displayPosition;
        [SerializeField] public bool _actived;
        [SerializeField] public bool _visible;

        [SerializeField] public bool _debugMode;

        public string Name { get { return _name; } }
        public string Position { get { return _displayPosition; } }
        public GameObject ActualObject { get { return _actualObject; } set { _actualObject = value; } }
        public GameObject PrefabObject { get { return _prefabObject; } }
        public Matt_Widgets WidgetClass { get { return _widgetClass; } }

        public bool Actived { get { return _actived; } }
        public bool DebugMode { get { return _debugMode; } }

        public Widget()
        {
            _name = "NULL";
            _displayPosition = "NULL";

            _actived = true;
            _visible = true;

            _actualObject = null;
            _prefabObject = null;
            UpdateClass();
        }

        public Widget(string name, string position)
        {
            _name = name;
            _displayPosition = position;

            _actived = true;
            _visible = true;

            _actualObject = null;
            _prefabObject = null;
            UpdateClass();
        }

        public void UpdateClass()
        {
            if (_actualObject != null) { _widgetClass = _actualObject.GetComponent<Matt_Widgets>(); }
            else if (_prefabObject != null) { _widgetClass = _prefabObject.GetComponent<Matt_Widgets>(); }
            else {
                Debug.Log("Class introuvable");
                _widgetClass = null; }
        }

        public void SetActive(bool active)
        {
            _actived = active;
            if (ActualObject != null)
            {
                ActualObject.SetActive(active);
            } 

        }

    }

    private DisplayPosition GetDisplayPositionToName(string name)
    {
        foreach (var displayPosition in listDisplayPosition)
        {
            if (displayPosition.Name == name)
            {
                return displayPosition;
            }
        }

        Debug.Log("Widget pas trouver");
        return new DisplayPosition("NULL");
    }

    private Widget GetWidgetToName(string name)
    {
        foreach (var widget in listWidget)
        {
            if (widget.Name == name)
            {
                return widget;
            }
        }

        Debug.Log("Widget pas trouver");
        return new Widget();
    }

    private void Start()
    {
        //UpdateActionBar();

        InitialiseWidgets(listWidget);

        foreach (var widget in listWidget)
        {
            widget.UpdateClass();

            widget.WidgetClass.SystemStart();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //GetActualScripts();

        foreach (var widget in listWidget)
        {
            widget.WidgetClass.SystemUpdate();

            #if UNITY_EDITOR
                widget.WidgetClass.SystemDebug();
            #endif
        }

    }

    /// <summary>
    /// Active ou desactive le mode action par rapport a son "ActionTypeMode"
    /// </summary>
    public void SetActionMode(ActionTypeMode actionType, string sound = null)
    {
        // Si le playerController est null alors quitte la fonction
        if (_pC == null || _cH.IsMoving) { return; }

        if (sound == null && _soundResetSelection != null) { sound = _soundResetSelection; }



        // Si le joueur est pas en mode action alors active le mode action et change son type
        if (_pC.SelectionMode != SelectionMode.Action
        || _pC.ActionTypeMode != actionType)
        {
            if (sound != null)
                AudioManager.PlaySoundAtPosition(sound, Vector3.zero);

            _pC.IsPreviewing = false;
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = actionType;
        }
        else // Sinon Desactive le mode action pour le mode selection et retire le type action en le changeant par "none"
        {


            if (sound != null)
                AudioManager.PlaySoundAtPosition(_soundResetSelection, Vector3.zero);

            _pC.ExitActionMode();
        }

    }

    /// <summary>
    /// Active ou desactive le mode action par rapport a son "ActionTypeMode"
    /// </summary>
    public void SetActionMode(ActionTypeMode actionType)
    {
        // Si le playerController est null alors quitte la fonction
        if (ActualPlayer == null) { return; }

        PlayerController playerController = ActualPlayer;

        // Si le joueur est pas en mode action alors active le mode action et change son type
        if (playerController.SelectionMode != SelectionMode.Action)
        {
            playerController.SelectionMode = SelectionMode.Action;
            playerController.ActionTypeMode = actionType;
        }
        else // Sinon Desactive le mode action pour le mode selection et retire le type action en le changeant par "none"
        {
            playerController.SelectionMode = SelectionMode.Selection;
            playerController.ActionTypeMode = ActionTypeMode.None;
        }

    }

    /// <summary>
    /// Recupere le script du PlayerControlleur et du Character actuellement selectionner
    /// </summary>
    private void GetActualScripts()
    {
        if ((Team)_pC != Matt_LevelManager.GetCurrentController())
        {

            _pC = (PlayerController)Matt_LevelManager.GetCurrentController();

        }

        if (_pC != null)
        {
            if (_cH != _pC.GetCurrentCharactedSelected)
            {
                for (int i = 0; i < listWidget.Count; i++)
                {
                    listWidget[i].WidgetClass.SystemReset();
                }
                
            }

            _cH = _pC.GetCurrentCharactedSelected;

        }

        else
        {
            Debug.LogWarning("UI : Attention l'UI n'arrive pas à récupérer le PlayerController depuis le Level Manager");
        }
    }

    private void InitialiseWidgets(List<Widget> list)
    {
        foreach (var widget in list)
        {
             DisplayPosition position = GetDisplayPositionToName(widget.Position);

            if (position.Name != "NULL")
            {

                if (position.Occupied == false && widget.Actived)
                {

                    InstantiateWidget(widget);

                    position.ListWidgets.Add(widget.Name);
                    position.Occupied = true;

                }
                else if (!widget.Actived) 
                {
                    widget.SetActive(false);

                    Debug.Log("Le widget " + widget.Name + " n'est pas activer"); 
                }
                else
                {

                    widget.SetActive(false);

                    Debug.Log("La position " + position.Name + " est deja prise");
                }

            }
            else 
            {
                Debug.Log("La valeur de position n'est pas valide");
            }

        }
    }

    /// <summary>
    /// Verifie l'existance d'un widget, si il n'existe pas essaye de le trouver ou de l'instancier
    /// </summary>
    /// <param name="name">nom de l'objet a rechercher</param>
    /// <param name="thisObject">reference de l'objet</param>
    /// <param name="prefab">prefab de l'objet voulu</param>
    private void InstantiateWidget(Widget widget)
    {
        // Si l'objet n'est pas referencer alors initialise la sequence
        if (widget.ActualObject == null)
        {
            // Cherche si l'objet est enfant de l'HUD sinon instancie l'objet et le reference
            //widget.ActualObject = transform.Find(name).gameObject;
            if (widget.ActualObject != null)
            {
                Debug.Log("L'objet " + widget.Name + " a etais retrouver et referencer");
                return;
            }
            else if (widget.PrefabObject != null)
            {
                // Initialise le popup si il n'existe pas et que la prefab a etais definit
                if (FindObjectOfType<Canvas>() != null)
                {
                    Transform canvas = FindObjectOfType<Canvas>().transform;
                    widget.ActualObject = Instantiate(widget.PrefabObject, canvas);
                } 
                else
                {
                    widget.ActualObject = Instantiate(widget.PrefabObject);
                }

                widget.ActualObject.name = widget.Name;

                widget.UpdateClass();
                widget.WidgetClass.hudManager = this;
            }
            else
            {
                Debug.LogWarning("Le systeme de PopUp a etais implementer mais aucun moyen n'as etais touver pour referencer ou initialiser le popup");
                return;
            }

        }
    }

    /// <summary>
    /// Verifie l'existance d'un widget, si il n'existe pas essaye de le trouver ou de l'instancier
    /// </summary>
    /// <param name="name">nom de l'objet a rechercher</param>
    /// <param name="thisObject">reference de l'objet</param>
    /// <param name="prefab">prefab de l'objet voulu</param>
    private void InstantiateWidget(Widget widget, Transform parent)
    {
        // Si l'objet n'est pas referencer alors initialise la sequence
        if (widget.ActualObject == null)
        {
            // Cherche si l'objet est enfant de l'HUD sinon instancie l'objet et le reference
            widget.ActualObject = transform.Find(name).gameObject;
            if (widget.ActualObject != null)
            {
                Debug.Log("L'objet " + widget.Name + " a etais retrouver et referencer");
                return;
            }
            else if (widget.PrefabObject != null)
            {
                // Initialise le popup si il n'existe pas et que la prefab a etais definit
                widget.ActualObject = Instantiate(widget.PrefabObject, parent.position, Quaternion.identity, parent);
                widget.ActualObject.name = widget.Name;
            }
            else
            {
                Debug.LogWarning("Le systeme de PopUp a etais implementer mais aucun moyen n'as etais touver pour referencer ou initialiser le popup");
                return;
            }

        }
    }

}
