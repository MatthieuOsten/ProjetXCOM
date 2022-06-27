using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WidgetsManager : MonoBehaviour
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
        if (_resetPosition == true)
        {

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
        [SerializeField][Min(-1)] private int _widgetsMAX;
        [SerializeField] private List<string> _listWidgets;

        [Header("ZONE")]
        [SerializeField] private Rect _rect;

        public bool Occupied { get { return _occupied; } set { _occupied = value; } }
        public int WidgetsMAX
        {
            get { return _widgetsMAX; }
            set
            {
                if (value >= -1)
                {
                    _widgetsMAX = value;
                }
                else
                {
                    _widgetsMAX = 0;
                }
            }
        }

        public string Name { get { return _name; } }
        public Rect RectZone { get { return _rect; } set { _rect = value; } }
        public List<string> ListWidgets
        {
            get { return _listWidgets; }
            set
            {
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

        public DisplayPosition(string name, bool occupied, int max, Rect rect)
        {
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

        public Widget(string name, string position)
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
        InitialiseWidgets(listWidget);

        FindObjectOfType<UI>()._objectPopUp = GetWidgetToName("PopUp").ActualObject;
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



}