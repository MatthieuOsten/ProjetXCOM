using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private PlayerController _pC;
    [SerializeField] private Character _cH;

    [SerializeField] private Image _barreAction;

    [SerializeField] private Image _icone;
    [SerializeField] private Image _iconeTeam;
    /// <summary> Correspond à la couleur qui sera afficher derrière la liste des personnages </summary>
    [SerializeField] private Image _glowTeam;


    [SerializeField] private GameObject imageAmmo;
    [SerializeField] private GameObject parentAmmo;
    [SerializeField] private GameObject imageActionPoint;
    [SerializeField] private GameObject parentActionPoint;
    [SerializeField] private GameObject imageIconeTeam;
    [SerializeField] private GameObject parentIconeTeam;
    [SerializeField] private List<Image> _children;
    [SerializeField] private List<GameObject> _actionPoint;
    [SerializeField] private List<GameObject> _ammo;
    [SerializeField] private List<GameObject> _teamImage;

    [SerializeField] private int _myAmmoMax;
    [SerializeField] private int _actionPointMax;
    [SerializeField] private int _ammoImageIndex;

    TextMeshProUGUI myText;
    [SerializeField] private GameObject _textCompetence2;
    [SerializeField] private TextMeshProUGUI _textDebug;

    [Header("BOUTTON")]
    [SerializeField] private Button _tir;
    [SerializeField] private Button _vigilance;
    [SerializeField] private Button _competence1;
    [SerializeField] private Button _competence2;
    [SerializeField] private Button _reload;

    [Header("PopUp")]
    [SerializeField] private GameObject _objectPopUp;
    [SerializeField] private GameObject _prefabPopUp;

     [SerializeField] private bool _updateActionBar;

    [Header("ACTION BAR")]
    [SerializeField] private List<GameObject> _actionButton;
    [SerializeField] private List<DataCharacter.Capacity> _actionCapacity;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private GameObject _prefabButton;

    [SerializeField] private int _difference;

    public int AmmoIndex
    {
        get { return _ammoImageIndex; }
        set
        {
            _ammoImageIndex = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FindScripts();
    }

    // Update is called once per frame
    void Update()
    {      
        GetActualScripts();
        ActualActionPoint();
        MaximumAmmo();
        ShadeBar();
        ListTeam();
        UpdateButtonInformation();
    }

    private void FixedUpdate()
    {
        AdaptBar();
        ActualAmmo();
    }
    private void FindScripts()
    {
        _pC = FindObjectOfType<PlayerController>();
    }

    //recupere les scripts du character selectionner
    private void GetActualScripts()
    {
        _pC = (PlayerController)LevelManager.GetCurrentController();
        _cH = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>();

    }

    //recupere les mun max de munition dans l'arme
    private void MaximumAmmo()
    {
         _myAmmoMax = _cH.GetWeaponCapacityAmmo(0);
    }

    //gere si la barre doit etre invisible ou non
    private void ShadeBar()
    {
        
        //Si pas en mode action quasi invisible
        if(_pC.GetCurrentActorSelected == null)
        {
            _barreAction.gameObject.SetActive(false); // desactive la barre d'action
            _barreAction.color = new Color(_barreAction.color.r, _barreAction.color.g, _barreAction.color.b, 0.1f);
            foreach (Image image in _children)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
            }
        }
        //si en mode action apparente
        else
        {
            _barreAction.color = new Color(_barreAction.color.r, _barreAction.color.g, _barreAction.color.b, 1f);
            foreach (Image image in _children)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }
            _barreAction.gameObject.SetActive(true);  // reactive la barre d'action

        }
    }

    //Recupere les images correspondant aux capacites
    private void AdaptBar()
    {
        if (_cH != null)
        {
            DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;

            myText = _textCompetence2.GetComponent<TextMeshProUGUI>();

            // Met a jour l'icone du personnage
            _icone.GetComponent<Image>().sprite = data.icon;

            List<Button> _actionButton = new List<Button>();

            _actionButton.Add(_tir.GetComponent<Button>());
            //_actionButton.Add(_vigilance.GetComponent<Button>());
            _actionButton.Add(_competence1.GetComponent<Button>());
            _actionButton.Add(_competence2.GetComponent<Button>());
            _actionButton.Add(_reload.GetComponent<Button>());

            List<DataCharacter.Capacity> _actionCapacity = data.ListCapacity;

            WatchReloadButton();

            

            //     for (int i = 0; i < _actionCapacity.Count; i++)
            // {
            //     if (_actionCapacity[i].name != "null")
            //     {
            //         if (_actionCapacity[i].icon != null)
            //             _actionButton[i].GetComponent<Image>().sprite = _actionCapacity[i].icon;

            //         _actionButton[i].gameObject.SetActive(true);

            //         // Recupere le "EventTrigger" du boutton
            //         EventTrigger eventTrigger;

            //         if (_actionButton[i].TryGetComponent<EventTrigger>(out eventTrigger))
            //         {
            //             eventTrigger.triggers.Clear();

            //             // Initialise un event "EventTrigger"
            //             EventTrigger.Entry onSelected = new EventTrigger.Entry();
            //             // Nettoie la liste d'evenement
            //             onSelected.callback.RemoveAllListeners();
            //             // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
            //             onSelected.eventID = EventTriggerType.PointerEnter;
            //             // Insert dans sa liste de reaction, l'affichage de la pop-up de description
            //             int indexTrigger = i;
            //             onSelected.callback.AddListener((eventData) => { DisplayPopUp(_actionButton[indexTrigger].transform.position, _actionCapacity[indexTrigger].description, _actionCapacity[indexTrigger].name); });
            //             // Ajoute le composant et ces parametres dans le boutton
            //             eventTrigger.triggers.Add(onSelected);

            //             // Initialise un event "EventTrigger"
            //             EventTrigger.Entry onDeselected = new EventTrigger.Entry();
            //             // Nettoie la liste d'evenement
            //             onDeselected.callback.RemoveAllListeners();
            //             // Le met en mode "UpdateSelected" afin de detecter lorsque la souris est sur le boutton
            //             onDeselected.eventID = EventTriggerType.PointerExit;
            //             // Insert dans sa liste de reaction, l'affichage de la pop-up de description
            //             onDeselected.callback.AddListener((eventData) => { HidePopUp(); });
            //             // Ajoute le composant et ces parametres dans le boutton
            //             eventTrigger.triggers.Add(onDeselected);
            //         }
            //     } else
            //     {
            //         _actionButton[i].gameObject.SetActive(false);
            //     }

            //}


            // foreach (Image myPoint in _actionPoint)
            // {
            //     myPoint.GetComponent<Image>().sprite = data.PointAction;
            // }


            // if (data.SpriteCompetence2 == null)
            // {
            //     _competence2.image.enabled = false;
            //     _competence2.enabled = false;

            //     myText.enabled = false;
            // }


            WatchAbilitiesButton();

        }

        else
        {
            _competence2.enabled = true;
            _competence2.image.enabled = true;
            //myText.enabled = true;
        }       
    }

    void WatchAbilitiesButton()
    {
            DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
            // On vérifie si la compétence peut être utilisable en jeu et lors du développement
            if (data.AbilityAvailable)
            {
                _competence1.gameObject.SetActive(true);
                _competence1.GetComponentInChildren<TextMeshProUGUI>().text = _cH.GetAbilityName;
            }
            else
                _competence1.gameObject.SetActive(false);

             // On vérifie si la compétence peut être utilisable en jeu et lors du développement
            if (data.AbilityAltAvailable)
            {
                _competence2.gameObject.SetActive(true);
                _competence2.GetComponentInChildren<TextMeshProUGUI>().text = _cH.GetAbilityAltName;
            }
            else
                _competence2.gameObject.SetActive(false);

    }

    void WatchReloadButton()
    {
            if (_cH.GetWeaponCapacityAmmo() > 0)
                {
                    Color colorReload = _reload.GetComponent<Image>().color;
                    _reload.gameObject.SetActive(true);

                    if (_cH.GetWeaponCurrentAmmo() < _myAmmoMax)
                    {
                        colorReload.a = 1f;
                        _reload.GetComponent<Image>().color = colorReload;
                        _reload.interactable = true;
                    }

                    else
                    {
                        colorReload.a = 0.3f;
                        _reload.GetComponent<Image>().color = colorReload;
                        _reload.interactable = false;
                    }

                     //_actionCapacity[4].SetName("null");
                }

                else
                {
                
                    _reload.gameObject.SetActive(false);
                }
    }

    private void UpdateButtonInformation()
    {
         _actionCapacity.Clear();
            _actionCapacity = _cH.Data.ListCapacity;
        // ---- Initialise chaque bouttons en rapport avec les capacit�s actuel ---- //
        for (int i = 0; i < _actionButton.Count; i++)
        {
            if (_actionCapacity.Count < i) { break; }

            // Nettoie la liste d'action du boutton
            _actionButton[i].GetComponent<Button>().onClick.RemoveAllListeners();

            // -- Initialise les donn�es du boutton initialiser -- //

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

    private void ActualActionPoint()
    {
        _actionPointMax = _cH.MaxActionPoint;
        if(_actionPointMax < _cH.CurrentActionPoint)
        {
            _actionPointMax = _cH.CurrentActionPoint;
        }
        if (_actionPoint.Count < _actionPointMax)
        {
            
            for (int i = _actionPoint.Count; i < _actionPointMax; i++)
            {
                GameObject addImageAction = Instantiate(imageActionPoint, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                addImageAction.transform.SetParent(parentActionPoint.transform, false);
                _actionPoint.Add(addImageAction);
            }                     
        }

        for (int i = 0; i < _actionPoint.Count; i++)
        {
            if (i >= _cH.CurrentActionPoint)
            {
                _actionPoint[i].GetComponent<Image>().color = new Color(_actionPoint[i].GetComponent<Image>().color.r, _actionPoint[i].GetComponent<Image>().color.g, _actionPoint[i].GetComponent<Image>().color.b, 0f);
            }

            else
            {
                _actionPoint[i].GetComponent<Image>().color = new Color(_actionPoint[i].GetComponent<Image>().color.r, _actionPoint[i].GetComponent<Image>().color.g, _actionPoint[i].GetComponent<Image>().color.b, 1f);
            }
        }

    }

    //Gere l'affichage du nombre actuel des munitions
    private void ActualAmmo()
    {
        if(_cH != null && _cH.Ammo != null)
        {
            if (_ammo.Count < _myAmmoMax)
            {
                for (int i = 0; i < _actionPointMax; i++)
                {
                    GameObject addImageAmmo = Instantiate(imageAmmo, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                    addImageAmmo.transform.SetParent(parentAmmo.transform, false);
                    _ammo.Add(addImageAmmo);
                }
            }

            for (int i = 0; i < _ammo.Count; i++)
            {
                if (i >= _cH.GetWeaponCurrentAmmo())
                {
                    _ammo[i].GetComponent<Image>().color = new Color(_ammo[i].GetComponent<Image>().color.r, _ammo[i].GetComponent<Image>().color.g, _ammo[i].GetComponent<Image>().color.b, 0f);
                }

                else
                {
                    _ammo[i].GetComponent<Image>().color = new Color(_ammo[i].GetComponent<Image>().color.r, _ammo[i].GetComponent<Image>().color.g, _ammo[i].GetComponent<Image>().color.b, 1f);
                }
            }
        }
    }

    private void ListTeam()
    {
        _glowTeam.color = _pC.Data.Color;
        foreach(Character character in _pC.Squad)
        {
            if(_teamImage.Count < _pC.Squad.Length)
            {
                GameObject addIconeTeam = Instantiate(imageIconeTeam, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                addIconeTeam.transform.SetParent(parentIconeTeam.transform, false);
                _teamImage.Add(addIconeTeam);
                

                if (character == null)
                {
                    Color colorIcone = addIconeTeam.GetComponent<Image>().color;
                    colorIcone.r = 0.5f;
                    colorIcone.g = 0f;
                    colorIcone.b = 0f;
                    colorIcone.a = 0.3f;
                    addIconeTeam.GetComponent<Image>().color = colorIcone;
                }

                if(character == _pC.GetCurrentCharactedSelected)
                {
                    addIconeTeam.GetComponent<Image>().sprite = character.GetCharacterIcon();
                    Color colorIcone = addIconeTeam.GetComponent<Image>().color;
                    colorIcone.a = 1f;
                    addIconeTeam.GetComponent<Image>().color = colorIcone;
                }

                else
                {
                    addIconeTeam.GetComponent<Image>().sprite = character.GetCharacterIcon();
                    Color colorIcone = addIconeTeam.GetComponent<Image>().color;
                    colorIcone.a = 0.5f;
                    addIconeTeam.GetComponent<Image>().color = colorIcone;
                }
            }
        }
    }

    public void SetActionModeAttack()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            AudioManager.PlaySoundAtPosition("action_attack", Vector3.zero);
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Attack;
        } 
        else
        {
            ResetSelection();
        }

    }
    public void SetActionModeOverwatch()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            AudioManager.PlaySoundAtPosition("action_overwatch", Vector3.zero);
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Overwatch;
        }
        else
        {
            ResetSelection();
        }

    }
    public void SetActionModeCompetence()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            AudioManager.PlaySoundAtPosition("action_competence1", Vector3.zero);
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Competence1;
        }
        else
        {
            ResetSelection();
        }


    }
    public void SetActionModeCompetenceAlt()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            AudioManager.PlaySoundAtPosition("action_competence2", Vector3.zero);
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Competence2;
        }
        else
        {
            ResetSelection();
        }


    }

    void ResetSelection()
    {
        _pC.SelectionMode = SelectionMode.Selection;
        _pC.ActionTypeMode = ActionTypeMode.None;
        AudioManager.PlaySoundAtPosition("action_reset", Vector3.zero);

    }

    public void SetActionModeReload()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            AudioManager.PlaySoundAtPosition("action_reload", Vector3.zero);
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Reload;
        }
        else
        {
            ResetSelection();
        }

    }
    public void EndTurn()
    {
        if(_pC.CanPassTurn)
        {
            LevelManager.Instance.PassedTurn = true;
            
        }
            
    }

    /* if (_pC.GetCurrentActorSelected == null)
       {
           _textDebug.text = "";
           return;
       }
       string debugString = "";

           Character : Name
           Action Point : i / i
           Health : 1 / 1
           State : Alive
           Ammo : 2/2

       Character _char = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>();

       debugString += $"Character :{_char.GetCharacterName()} \n";
       debugString += $"Action Point :  {_char.CurrentActionPoint} / {_char.MaxActionPoint} \n";
       debugString += $"Health : {_char.Health} \n";
       debugString += $"State :{_char.State} \n";
       debugString += $"Ammo :{_char.Ammo} /  \n";
       _textDebug.text = debugString;*/
}
