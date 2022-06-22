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
    [Matt_CustomAttribues.ReadOnly] 
    [SerializeField] private bool _updateActionBar;

    [Header("POPUP")]
    [SerializeField] private string _informationPopUp;

    [Header("WIDGETS")]
    [SerializeField] private List<Widget> listWidget;
    [SerializeField] private bool[] widgetsPlace = { false, false, false, false, false, false, false, false, false };

    [System.Serializable]
    public enum displayPosition
    {
        none,
        center,

        left,
        right,
        top,
        bottom,

        topLeft,
        topRight,
        bottomLeft,
        bottomRight,

        end
    }

    [System.Serializable]
    private struct Widget
    {
        [SerializeField] private PropertyName _name;
        [SerializeField] private GameObject _actualObject;
        [SerializeField] private GameObject _prefabObject;
        [SerializeField] private displayPosition _position;
        [SerializeField] public bool _actived;

        public PropertyName Name { get { return _name; } }
        public displayPosition Position { get { return _position; } }
        public GameObject ActualObject { get { return _actualObject; } }
        public GameObject PrefabObject { get { return _prefabObject; } }

        public bool Actived { get { return _actived; } }

        public void SetActive(bool active)
        {
            _actived = active;
        }

    }

    private Widget GetWidget(List<Widget> list, string name)
    {
        foreach (var widget in list)
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

        widgetsPlace = new bool[(int)displayPosition.end - 2];

        foreach (var widget in listWidget)
        {
            displayPosition pos = widget.Position;

            if (pos != displayPosition.none && pos != displayPosition.end)
            {
                int indexPos = (int)widget.Position - 1;

                if (widgetsPlace[(int)widget.Position] == false && widget.Actived)
                {

                    InstantiateWidget(widget.Name.ToString(), widget.ActualObject, widget.PrefabObject);
                    widgetsPlace[(int)widget.Position] = true;

                }
                else if (widgetsPlace[(int)widget.Position] != false)
                {

                    widget.SetActive(false);

                    Debug.Log("La position numero " + (int)widget.Position + " est deja prise");
                }
                else
                {
                    Debug.Log("Le widget " + widget.Name + " n'est pas activer");
                }
            }

            

        }

    }

    // Update is called once per frame
    private void Update()
    {
        //GetActualScripts();
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
                    onSelected.callback.AddListener((eventData) => { DisplayPopUp(GetWidget(listWidget, _informationPopUp).ActualObject, _actionButton[indexTrigger].transform.position, _actionCapacity[indexTrigger].description, _actionCapacity[indexTrigger].name); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onSelected);

                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onDeselected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onDeselected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onDeselected.eventID = EventTriggerType.PointerExit;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    onDeselected.callback.AddListener((eventData) => { HidePopUp(GetWidget(listWidget, _informationPopUp).ActualObject); });
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
