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

    [Header("ACTION BAR")]
    [SerializeField] private List<GameObject> _actionButton;
    [SerializeField] private List<DataCharacter.Capacity> _actionCapacity;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private GameObject _prefabButton;
    [SerializeField] private int _difference;
    [SerializeField] private bool _updateActionBar;

    [Header("POPUP")]
    [SerializeField] private string _informationPopUp;

    [Header("WIDGETS")]
    [SerializeField] private List<Widget> listWidget;
    [SerializeField] private List<DisplayPosition> listDisplayPosition;
    [SerializeField] private bool _resetPosition;

    private void OnValidate()
    {
        if (_resetPosition == true) {

            _resetPosition = false;

            int camWidth = Camera.current.pixelWidth;
            int camHeight = Camera.current.pixelHeight;

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

    }

    [System.Serializable]
    public struct DisplayPosition
    {
        [SerializeField] private string _name;

        [Header("EMPLACEMENT")]
        [SerializeField] private bool _occupied;
        [SerializeField] [Min(-1)] private int _widgetsMAX;
        [SerializeField] private List<string> _listWidgets;

        [Header("ZONE")]
        [SerializeField] private Rect _rect;

        public bool Occupied { get { return _occupied;} set { _occupied = value; } }
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

        [SerializeField] private GameObject _actualObject;
        [SerializeField] private GameObject _prefabObject;
        [SerializeField] private string _displayPosition;
        [SerializeField] public bool _actived;
        [SerializeField] public bool _visible;

        public string Name { get { return _name; } }
        public string Position { get { return _displayPosition; } }
        public GameObject ActualObject { get { return _actualObject; } }
        public GameObject PrefabObject { get { return _prefabObject; } }

        public bool Actived { get { return _actived; } }

        public Widget()
        {
            _name = "NULL";
            _displayPosition = "NULL";

            _actived = true;
            _visible = true;

            _actualObject = null;
            _prefabObject = null;
        }

        public Widget(string name,string position)
        {
            _name = name;
            _displayPosition = position;

            _actived = true;
            _visible = true;

            _actualObject = null;
            _prefabObject = null;
        }

        public void SetActive(bool active)
        {
            _actived = active;
            ActualObject.SetActive(active);
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
        UpdateActionBar();

        InitialiseWidgets(listWidget);

    }

    // Update is called once per frame
    private void Update()
    {
        GetActualScripts();
    }

    private void FixedUpdate()
    {
        if (_updateActionBar)
        {
            _updateActionBar = false;
            UpdateActionBar();
        }

    }

    /// <summary>
    /// Recupere les donnees d'un personnage "DataCharacter" 
    /// </summary>
    private DataCharacter GetData()
    {
        DataCharacter data;

        if (_pC != null)
        {
            // Recupere la base de donnee du personnage selectionner
            data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
            return data;
        }
        else
        {

            if (_dataCH != null)
            {
                data = _dataCH;
                return data;
            }

            return new DataCharacter();
        }

    }

    /// <summary>
    /// Recupere le script du PlayerControlleur et du Character actuellement selectionner
    /// </summary>
    private void GetActualScripts()
    {
        // Recupere le "PlayerController" qui est actuellement entrain de jouer
        _pC = (PlayerController)LevelManager.GetCurrentController();

        // Si le "PlayerController" n'est pas null alors recupere les donnees du personnage actuel
        if (_pC != null)
        {
            _dataCH = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
        }

    }



    /// <summary>
    /// Met a jour les informations des bouttons, image, texte, click event et Pointer trigger event
    /// </summary>
    private void UpdateButtonInformation()
    {
        // ---- Initialise chaque bouttons en rapport avec les capacites actuel ---- //
        for (int i = 0; i < _actionButton.Count; i++)
        {
            if (_actionCapacity.Count < i) { break; }


            Button button = _actionButton[i].GetComponent<Button>();
            // Nettoie la liste d'action du boutton
            button.onClick.RemoveAllListeners();

            // -- Initialise les donnees du boutton initialiser -- //

            // Insert l'action effectuer si le boutton est appuyer
            int index = i;
            button.onClick.AddListener(() => SetActionMode(_actionCapacity[index].typeA));

            // Verifie que l'objet a une icone et l'affiche
            if (_actionCapacity[i].icon != null)
                _actionButton[i].GetComponent<Image>().sprite = _actionCapacity[i].icon;

            // Verifie que l'objet a un nom et l'ecrit
            if (_actionCapacity[i].name != null)
                _actionButton[i].GetComponentInChildren<TextMeshProUGUI>().text = _actionCapacity[i].name;

            // -- Initialise "DisplayDescription" sur le boutton -- //
            if (_actionCapacity[i].description != null) // Verifie que la description est remplie
            {
                // Recupere le "EventTrigger" du boutton
                EventTrigger eventTrigger;

                if (_actionButton[i].TryGetComponent<EventTrigger>(out eventTrigger))
                {
                    eventTrigger.triggers.Clear();

                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onSelected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onSelected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onSelected.eventID = EventTriggerType.PointerEnter;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    int indexTrigger = i;
                    onSelected.callback.AddListener((eventData) => { DisplayPopUp(GetWidgetToName(_informationPopUp).ActualObject, _actionButton[indexTrigger].transform.position, _actionCapacity[indexTrigger].description, _actionCapacity[indexTrigger].name); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onSelected);

                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onDeselected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onDeselected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onDeselected.eventID = EventTriggerType.PointerExit;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    onDeselected.callback.AddListener((eventData) => { HidePopUp(GetWidgetToName(_informationPopUp).ActualObject); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onDeselected);
                }

            }

        }
    }

    /// <summary>
    /// Met a jour la barre d'action, met a jour le nombre de boutton par rapport aux nombre de capaciter de l'actor atuelment selectionner
    /// </summary>
    private void UpdateActionBar()
    {
        if (_dataCH != null)
        {
            // Recupere les donnees du personnage actuellement selectionner
            DataCharacter data = GetData();

            // Initialise "_actionCapacity" en y rentrant toute les capacite du personnage
            _actionCapacity.Clear();
            _actionCapacity = data.ListCapacity;

            // Nettoie la liste des actions
            if (_actionCapacity.Count > 0)
            {
                _actionCapacity.RemoveAll(item => item.name == null);
            }

            // Verifie si il contient assez de bouton comparer au nombre de capacite du personnage
            if (_actionButton.Count != _actionCapacity.Count)
            {

                Debug.Log("Nombre de boutton : " + _actionButton.Count);
                Debug.Log("Nombre de capacites : " + _actionCapacity.Count);

                // Si il y a moins de boutons que de capacites alors rajoute des boutons
                if (_actionButton.Count < _actionCapacity.Count)
                {
                    InstantiateButton(_actionButton.Count, _actionCapacity.Count, _prefabButton, _layoutGroup);
                }
                // Supprime les bouttons en trop
                else if (_actionButton.Count > _actionCapacity.Count)
                {

                    // Detruit chaque bouttons un par un
                    foreach (var button in _actionButton)
                    {
                        //Debug.Log("Boutton : " + button.name + " " + button.GetInstanceID() + " Supprimer");
                        Destroy(button);

                    }
                    _actionButton.Clear();

                    InstantiateButton(_actionButton.Count, _actionCapacity.Count, _prefabButton, _layoutGroup);

                }

                // Verifie que les donnee soit bien initialiser avant de procede
                if (data != null && _actionCapacity.Count > 0 && _actionButton.Count > 0)
                {
                    UpdateButtonInformation();
                }

            }

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

                    InstantiateWidget(widget.Name.ToString(), widget.ActualObject, widget.PrefabObject, transform);

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
    private void InstantiateWidget(string name, GameObject thisObject, GameObject prefab)
    {
        // Si l'objet n'est pas referencer alors initialise la sequence
        if (thisObject == null)
        {
            // Cherche si l'objet est enfant de l'HUD sinon instancie l'objet et le reference
            thisObject = transform.Find(name).gameObject;
            if (thisObject != null)
            {
                Debug.Log("L'objet " + thisObject.name + " a etais retrouver et referencer");
                return;
            }
            else if (prefab != null)
            {
                // Initialise le popup si il n'existe pas et que la prefab a etais definit
                thisObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
                thisObject.name = name;
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
    private void InstantiateWidget(string name, GameObject thisObject, GameObject prefab, Transform parent)
    {
        // Si l'objet n'est pas referencer alors initialise la sequence
        if (thisObject == null)
        {
            // Cherche si l'objet est enfant de l'HUD sinon instancie l'objet et le reference
            thisObject = transform.Find(name).gameObject;
            if (thisObject != null)
            {
                Debug.Log("L'objet " + thisObject.name + " a etais retrouver et referencer");
                return;
            }
            else if (prefab != null)
            {
                // Initialise le popup si il n'existe pas et que la prefab a etais definit
                thisObject = Instantiate(prefab, parent.position, Quaternion.identity, parent);
                thisObject.name = name;
            }
            else
            {
                Debug.LogWarning("Le systeme de PopUp a etais implementer mais aucun moyen n'as etais touver pour referencer ou initialiser le popup");
                return;
            }

        }
    }

    /// <summary>
    /// Cree un nombre d'objet definit avec un for, a la position de son parent
    /// </summary>
    /// <param name="start">Nombre d'objet deja initialiser</param>
    /// <param name="end">Nombre total d'objet voulu</param>
    /// <param name="prefab">Prefab de l'objet</param>
    /// <param name="parent">Parent de l'objet instancier</param>
    private void InstantiateButton(int start, int end, GameObject prefab, GameObject parent)
    {
        // Initialise des bouttons en plus si besoin
        for (int i = start; i < end; i++)
        {
            _actionButton.Add(Instantiate(prefab, parent.transform.position, Quaternion.identity, parent.transform));
        }
    }

    /// <summary>
    /// Affiche une pop-up sur la souris avec les information entrer
    /// </summary>
    private void DisplayPopUp(GameObject popUp, Vector3 position, string description = " ", string title = "Information")
    {

        // Si le popUp est initialiser, l'affiche et change son texte
        if (popUp != null)
        {
            // Deplace le popUp a l'endroit indiquer
            popUp.transform.position = position;

            // Recupere la transform du popUp
            Transform parent = popUp.transform;
            // Change le titre du PopUp
            ModifyTextBox(parent, "Title", title);
            // Change la description du PopUp
            ModifyTextBox(parent, "Description", description);

            if (popUp.activeSelf == false) { popUp.SetActive(true); }
        }

    }

    /// <summary>
    /// Si l'objet PopUp est assigne, le desactive si il est actif
    /// </summary>
    private void HidePopUp(GameObject popUp)
    {
        // Si le popUp est initialiser, le cache
        if (popUp != null)
        {
            if (popUp.activeSelf == true) { popUp.SetActive(false); }
        }
    }

    /// <summary>
    /// Met a jour la boite de texte enfant d'un GameObject
    /// </summary>
    /// <param name="nameChild">Enfant a chercher du GameObject</param>
    /// <param name="valueString">Chaine de charactere a inserer</param>
    private void ModifyTextBox(Transform parent, string nameChild, string valueString)
    {
        TextMeshProUGUI textMesh;
        Text text;
        Transform textBox;

        // -- Initialise le texte de la boite de texte de "Description" -- //
        textBox = parent.Find(nameChild);
        // Si le composant text est present alors change le texte
        if (textBox.TryGetComponent<TextMeshProUGUI>(out textMesh))
        {
            textMesh.text = valueString;
        }
        else if (textBox.TryGetComponent<Text>(out text))
        {
            text.text = valueString;
        }
    }

    /// <summary>
    /// Active ou desactive le mode action par rapport a son "ActionTypeMode"
    /// </summary>
    public void SetActionMode(ActionTypeMode actionType)
    {
        // Si le playerController est null alors quitte la fonction
        if (_pC == null) { return; }

        // Si le joueur est pas en mode action alors active le mode action et change son type
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = actionType;
        }
        else // Sinon Desactive le mode action pour le mode selection et retire le type action en le changeant par "none"
        {
            _pC.SelectionMode = SelectionMode.Selection;
            _pC.ActionTypeMode = ActionTypeMode.None;
        }

    }

}
