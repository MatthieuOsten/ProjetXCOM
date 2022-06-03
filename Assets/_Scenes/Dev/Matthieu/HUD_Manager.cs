//usingusing System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HUD_Manager : MonoBehaviour
{
    [SerializeField] private PlayerController _pC;
    [SerializeField] private DataCharacter _dataCH;

    [Header("ACTION BAR")]
    [SerializeField] private List<GameObject> _actionButton;
    [SerializeField] private List<DataCharacter.Capacity> _actionCapacity;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private GameObject _prefabButton;
    [SerializeField] private int _difference;
    [SerializeField] private bool _updateActionBar;

    [Header("PopUp")]
    [SerializeField] private GameObject _objectPopUp;
    [SerializeField] private GameObject _prefabPopUp;

    private void Start()
    {
        UpdateActionBar();
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
    /// Recupere le script du PlayerControlleur et du Character actuellement selectionner
    /// </summary>
    private void GetActualScripts()
    {
        // Recupere le "PlayerController" qui est actuellement entrain de jouer
        _pC = (PlayerController)LevelManager.GetCurrentController();

        // Si le "PlayerController" n'est pas null alors recupere les données du personnage actuel
        if (_pC != null)
        {
            _dataCH = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
        }

    }

    /// <summary>
    /// Recupere les données d'un personnage "DataCharacter" 
    /// </summary>
    private DataCharacter GetData()
    {
        DataCharacter data;

        if (_pC != null)
        {
            // Recupere la base de donnée du personnage selectionner
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

    private void UpdateButtonInformation()
    {
        // ---- Initialise chaque bouttons en rapport avec les capacités actuel ---- //
        for (int i = 0; i < _actionButton.Count; i++)
        {
            if (_actionCapacity.Count < i) { break; }

            // Nettoie la liste d'action du boutton
            _actionButton[i].GetComponent<Button>().onClick.RemoveAllListeners();

            // -- Initialise les données du boutton initialiser -- //

            // Insert l'action effectuer si le boutton est appuyer
            int index = i;
            _actionButton[i].GetComponent<Button>().onClick.AddListener(() => SetActionMode(_actionCapacity[index].typeA));

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
                    onSelected.callback.AddListener((eventData) => { DisplayPopUp(_actionButton[indexTrigger].transform.position, _actionCapacity[indexTrigger].description, _actionCapacity[indexTrigger].name); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onSelected);

                    // Initialise un event "EventTrigger"
                    EventTrigger.Entry onDeselected = new EventTrigger.Entry();
                    // Nettoie la liste d'evenement
                    onDeselected.callback.RemoveAllListeners();
                    // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
                    onDeselected.eventID = EventTriggerType.PointerExit;
                    // Insert dans sa liste de reaction, l'affichage de la pop-up de description
                    onDeselected.callback.AddListener((eventData) => { HidePopUp(); });
                    // Ajoute le composant et ces parametres dans le boutton
                    eventTrigger.triggers.Add(onDeselected);
                }

            }

        }
    }

    /// <summary>
    /// Met a jour la barre d'action, ses boutton par rapport a l'actuel "Actor" selectionner
    /// </summary>
    private void UpdateActionBar()
    {
        if (_dataCH != null)
        {
            // Recupere les données du personnage actuellement selectionner
            DataCharacter data = GetData();

            // Initialise "_actionCapacity" en y rentrant toute les capacité du personnage
            //_actionCapacity.Clear();
            //_actionCapacity.Add(data.Weapon);
            //_actionCapacity.Add(data.WeaponAbility);
            //_actionCapacity.Add(data.WeaponAbilityAlt);

            _actionCapacity.Clear();
            _actionCapacity = data.ListCapacity;

            // Nettoie la liste des actions
            if (_actionCapacity.Count > 0)
            {
                _actionCapacity.RemoveAll(item => item.name == null);
            }

                // Verifie si il contient assez de bouton comparer au nombre de capacité du personnage
                if (_actionButton.Count != _actionCapacity.Count)
                {

                    //Debug.Log("Nombre de boutton : " + _actionButton.Count);
                    //Debug.Log("Nombre de capacités : " + _actionCapacity.Count);

                    // Si il y a moins de boutons que de capacités alors rajoute des boutons
                    if (_actionButton.Count < _actionCapacity.Count)
                        {
                            InstantiateButton(_actionButton.Count,_actionCapacity.Count, _prefabButton, _layoutGroup);
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

                            InstantiateButton(_actionButton.Count, _actionCapacity.Count,_prefabButton,_layoutGroup);

                        }

                // Verifie que les donnée soit bien initialiser avant de procedé
                if (data != null && _actionCapacity.Count > 0 && _actionButton.Count > 0)
                {
                    UpdateButtonInformation();
                }

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
    private void DisplayPopUp(Vector3 position, string description = " ", string title = "Information")
    {

        if (_objectPopUp == null)
        {
            if (_prefabPopUp != null)
            {
                // Initialise le popup si il n'existe pas et que la prefab a etais definit
                _objectPopUp = Instantiate(_objectPopUp, Vector3.zero, Quaternion.identity, transform);
            }
            else
            {
                return;
            }

        }

        // Si le popUp est initialiser, l'affiche et change son texte
        if (_objectPopUp != null)
        {
            // Deplace le popUp a l'endroit indiquer
            _objectPopUp.transform.position = position;
            // Change le titre du PopUp
            ModifyTextBox("Title", title);
            // Change la description du PopUp
            ModifyTextBox("Description", description);

            if (_objectPopUp.activeSelf == false) { _objectPopUp.SetActive(true); }
        }

    }

    private void HidePopUp()
    {
        // Si le popUp est initialiser, le cache
        if (_objectPopUp != null)
        {
            if (_objectPopUp.activeSelf == true) { _objectPopUp.SetActive(false); }
        }
    }

    /// <summary>
    /// Met a jour la boite de texte enfant d'un GameObject
    /// </summary>
    /// <param name="nameChild">Enfant a chercher du GameObject</param>
    /// <param name="valueString">Chaine de charactere a inserer</param>
    private void ModifyTextBox(string nameChild, string valueString)
    {
        TextMeshProUGUI textMesh;
        Text text;
        Transform textBox;

        // -- Initialise le texte de la boite de texte de "Description" -- //
        textBox = _objectPopUp.transform.Find(nameChild);
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