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
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Image _icone;
    [SerializeField] private GameObject _iconeTeam;
    /// <summary> Correspond à la couleur qui sera afficher derrière la liste des personnages </summary>
    [SerializeField] private Image _glowTeam;
    [SerializeField] private Image weaponImage;

    [SerializeField] private Image _endTurnImage;
    [SerializeField] private WidgetVictory _widgetVictory;

    [Header("Ammo")]
    [SerializeField] private GameObject imageAmmo;
    [SerializeField] private GameObject parentAmmo;
    [Header("Action Point")]
    [SerializeField] private GameObject imageActionPoint;
    [SerializeField] private GameObject parentActionPoint;
    [Header("Icone Team")]
    [SerializeField] private GameObject imageIconeTeam;
    [SerializeField] private GameObject parentIconeTeam;
    [Header("List Objects")]
    [SerializeField] private List<GameObject> _actionPoint;
    [SerializeField] private List<GameObject> _ammo;
    [SerializeField] private List<GameObject> _teamImage;


    GameObject iconeTeam;

    [SerializeField] private int _myAmmoMax;
    [SerializeField] private int _actionPointMax;
    [SerializeField] private int _ammoImageIndex;

    [SerializeField] private GameObject _textCompetence2;
    [SerializeField] private TextMeshProUGUI _textDebug;

    public List<GameObject> TeamImage
    {
        get { return _teamImage; }
        set
        {
            _teamImage = value;
        }
    }

    [Header("ACTION BAR")]
    [SerializeField] private List<GameObject> _actionButton;
    [SerializeField] private List<DataCharacter.Capacity> _actionCapacity;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private GameObject _prefabButton;
    [SerializeField] private static string _soundResetSelection = "action_reset";
    [SerializeField] private int _difference;
    [SerializeField] private bool _updateActionBar;

    [Header("PopUp")]
    [SerializeField] public GameObject _objectPopUp;
    [SerializeField] private GameObject _prefabPopUp;

    public int AmmoIndex
    {
        get { return _ammoImageIndex; }
        set
        {
            _ammoImageIndex = value;
        }
    }
    public void SetVictory(string nameTeam, Color color)
    {
        _widgetVictory.gameObject.SetActive(true);
        _widgetVictory.Text = nameTeam;
        _widgetVictory.Color = color;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_objectPopUp != null && _objectPopUp.activeSelf == true)
        {
            _objectPopUp.SetActive(false);
        }
       // parentIconeTeam.transform.Translate(Vector2.right * Time.deltaTime);
        FindScripts();
    }

    // Update is called once per frame
    void Update()
    {      
        GetActualScripts();
        //ActualActionPoint();
        MaximumAmmo();
        ShadeBar();
        UpdateButtonInformation();
        WatchListTeam();
             
    }

    private void FixedUpdate()
    {
        UpdateActionBar();
        ActualAmmo();
    }
    private void LateUpdate() {
        GetActualScripts();    
    }

    void WatchListTeam()
    {
         // gere l'affichage de leur etat
        for (int i = 0; i < _teamImage.Count; i++)
        {
            if(_teamImage[i] == null) return;
            RectTransform rectTrans = _teamImage[i].GetComponent<RectTransform>();
             rectTrans.pivot = new Vector2(0,1);
             
            // Si un personnage est selectionner, on le met en surbrillance
            if(_pC.GetCurrentCharactedSelected != null)
            {
                if(i == _pC.CharacterIndex)
                {
                    rectTrans.localScale = new Vector3(1.3f,1.3f,1.5f);
                }

                else
                {
                    rectTrans.localScale = new Vector3(0.8f,0.8f,1f);
                }
            }
            else // Aucun perso selectionner, on les met tous de la meme opacity
            {
                rectTrans.localScale = new Vector3(1f,1f,1f);
            }
           
        }
    }
    private void FindScripts()
    {
        _pC = FindObjectOfType<PlayerController>();
    }

    //recupere les scripts du character selectionner
    private void GetActualScripts()
    {
        if((Team)_pC != LevelManager.GetCurrentController() || _teamImage.Count <= 0)
        {
            foreach(GameObject image in _teamImage)
            {
                Destroy(image);
            }
            _teamImage = new List<GameObject>();
            _pC = (PlayerController)LevelManager.GetCurrentController();
            _endTurnImage.color = _pC.Color;

            ListTeam(iconeTeam);   
            
        }

        
        
        if(_pC != null)
        {
             if(_cH != _pC.GetCurrentCharactedSelected)
                HidePopUp();

            _cH = _pC.GetCurrentCharactedSelected;
            
        }

        else
        {
            Debug.LogWarning("UI : Attention l'UI n'arrive pas à récupérer le PlayerController depuis le Level Manager");
        }        
    }

    //recupere les mun max de munition dans l'arme
    private void MaximumAmmo()
    {
        if(_cH == null)
        {
            _myAmmoMax = 0;
            return;
        }
         _myAmmoMax = _cH.GetWeaponCapacityAmmo(0);
    }

    //gere si la barre doit etre invisible ou non
    private void ShadeBar()
    {
       // DataCharacter data = _pC.GetCurrentCharactedSelected.Data;
        

        if (_layoutGroup != null)
        {
            Image _barreAction = _layoutGroup.GetComponent<Image>();

            List<Image> children = new List<Image>();

            foreach (var button in _actionButton)
            {
                children.Add(button.GetComponent<Image>());
            }
           

            //Si pas en mode action quasi invisible
            if (_cH == null)
            {
                _icone.color = Color.clear;
                _icone.sprite = null;
                _layoutGroup.gameObject.SetActive(false); // desactive la barre d'action
                _barreAction.color = new Color(_barreAction.color.r, _barreAction.color.g, _barreAction.color.b, 0f);
                foreach (Image image in children)
                {
                    Color _color = image.color;
                    image.color = new Color(_color.r, _color.g, _color.b, 0.3f);
                }
                HidePopUp();
            }
            //si en mode action apparente
            else
            {
                _icone.sprite = _cH.GetCharacterIcon();
                _icone.color = Color.white;

                _barreAction.color = new Color(_barreAction.color.r, _barreAction.color.g, _barreAction.color.b, 0f);
                foreach (Image image in children)
                {
                     Color _color = image.color;
                    image.color = new Color(_color.r, _color.g, _color.b, 1f);
                }
                _layoutGroup.gameObject.SetActive(true);  // reactive la barre d'action

            }

        }

    }

    //void WatchAbilitiesButton()
    //{
    //        DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
    //        // On vérifie si la compétence peut être utilisable en jeu et lors du développement
    //        if (data.AbilityAvailable)
    //        {
    //            _competence1.gameObject.SetActive(true);
    //            _competence1.GetComponentInChildren<TextMeshProUGUI>().text = _cH.GetAbilityName;
    //        }
    //        else
    //            _competence1.gameObject.SetActive(false);

    //         // On vérifie si la compétence peut être utilisable en jeu et lors du développement
    //        if (data.AbilityAltAvailable)
    //        {
    //            _competence2.gameObject.SetActive(true);
    //            _competence2.GetComponentInChildren<TextMeshProUGUI>().text = _cH.GetAbilityAltName;
    //        }
    //        else
    //            _competence2.gameObject.SetActive(false);

    //}

    /// <summary>
    /// Chech si je dois recharger
    /// </summary>
    /// <param name="_reload"></param>
    void WatchReloadButton(GameObject _reload)
    {
        Color colorReload = _reload.GetComponent<Image>().color;
        //si doit recharger
            if (_cH.GetWeaponCurrentAmmo() < _myAmmoMax)
            {
                colorReload.a = 1f;

                _reload.GetComponent<Image>().color = colorReload;
                _reload.GetComponent<Button>().interactable = true;
                _reload.GetComponentInChildren<ButtonAction>().ICONE.color = Color.white;
                // Debug.Log("marche");
            }
            else
            {
               colorReload.a = 0.5f;
               
               _reload.GetComponent<Image>().color = colorReload;
               _reload.GetComponent<Button>().interactable = false;
               Color _color = Color.white;
               _color.a = 0.5f;
               _reload.GetComponentInChildren<ButtonAction>().ICONE.color = _color;
            } 

    }

    /// <summary>
    /// Met le bouton requis pour la competence
    /// </summary>
    /// <param name="competenceInstance"></param>
    private void AdaptIcone(GameObject competenceInstance, int _currentCooldown)
    {
        Color colorCompetence1 = competenceInstance.GetComponent<Image>().color;
        ButtonAction buttonAction = competenceInstance.GetComponentInChildren<ButtonAction>();
        Image image =  competenceInstance.GetComponent<Image>();
        Button button = competenceInstance.GetComponent<Button>();

        if (_currentCooldown == 0)
        {
            colorCompetence1.a = 1f;
            button.interactable = true;
            image.color = colorCompetence1;
            buttonAction.Cooldown.text = string.Empty;
             Color _color = Color.white;
            _color.a = 0.5f;
            buttonAction.ICONE.color = _color;
        }

        else
        {
            colorCompetence1.a = 0.5f;
            image.color = colorCompetence1;
            button.interactable = false;
            buttonAction.Cooldown.text = Mathf.RoundToInt(_currentCooldown).ToString();

            Color _color = Color.white;
            _color.a = 0.5f;
            buttonAction.ICONE.color = _color;
        }
    }

    //Gere l'affichage du nombre actuel des munitions
    private void ActualAmmo()
    {
        if (_cH != null && _cH.Ammo != null)
        {
            
            if(_myAmmoMax == 0 || _myAmmoMax > 0)
            {
                weaponImage.gameObject.SetActive(false);
                parentAmmo.SetActive(false);
            }

            if (_ammo.Count < _myAmmoMax)
            {
                //Instantie le nombre de d'image, ajoute au bon gameobject et à une liste
                for (int i = 0; i < _myAmmoMax; i++)
                {
                    GameObject addImageAmmo = Instantiate(imageAmmo, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                    addImageAmmo.transform.SetParent(parentAmmo.transform, false);
                    _ammo.Add(addImageAmmo);
                }
            }

            if(_myAmmoMax != 0 || _myAmmoMax !> 0)
            {
                weaponImage.gameObject.SetActive(true);
                parentAmmo.SetActive(true);
            }

            // Montre ou rend invisible le point d'action si utiliser
            for (int i = 0; i < _ammo.Count; i++)
            {
                Image ammoImage = _ammo[i].GetComponent<Image>();
                Color _color = ammoImage.color;
                if (i >= _cH.GetWeaponCurrentAmmo())
                {
                    ammoImage.color = new Color(_color.r, _color.g, _color.b, 0f);
                }

                else
                {
                    ammoImage.color = new Color(_color.r, _color.g, _color.b, 1f);
                }
            }

          
        }
        else
        {
           // 
            for (int i = 0; i < _ammo.Count; i++)
            {
                Image ammoImage = _ammo[i].GetComponent<Image>();
                Color _color = ammoImage.color;
                ammoImage.color = new Color(_color.r, _color.g, _color.b, 0f);
            }

            weaponImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Adapt les icones pour chaque membre de la team
    /// </summary>
    /// <param name="addIconeTeam"></param>
    private void ListTeam(GameObject addIconeTeam)
    {
        iconeTeam = addIconeTeam;
        _glowTeam.color = _pC.Data.Color;

        foreach (GameObject goCharacter in _pC.CharacterPlayer)
        {
            Character character = goCharacter.GetComponent<Character>();

            // Instantie le nombre de d'image, ajoute au bon gameobject et à une liste
          
                if(character == null)  continue;
                addIconeTeam = Instantiate(imageIconeTeam, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                WidgetActorInfo widget = addIconeTeam.GetComponent<WidgetActorInfo>();
                widget.relatedObject = character.gameObject;
                widget.IsFixed = true;
                addIconeTeam.transform.SetParent(parentIconeTeam.transform, false);
                _teamImage.Add(addIconeTeam);
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
        if (_pC.CanPassTurn)
        {
            LevelManager.Instance.PassedTurn = true;

        }

    }
    public void ChangeCharacter(int characterID = -1)
    {
        if (_pC != null && _pC.CanPlay)
        {
            if(characterID != -1)
                _pC.CharacterIndex = characterID;
            else
                _pC.CharacterChange();

        }

    }

    // -------- Gestion de la barre de tache et du PopUp a patir d'ici -------- //

    /// <summary>
    /// Recupere les données d'un personnage "DataCharacter" 
    /// </summary>
    private DataCharacter GetData()
    {
        DataCharacter data;

        if (_cH != null)
        {
            // Recupere la base de donnée du personnage selectionner
            data = _cH.Data;
            return data;
        }
        else
        {

            if (_cH != null)
            {
                data = _cH.Data;
                return data;
            }

            return new DataCharacter();
        }

    }

    private void UpdateButtonInformation()
    {
        if(_cH == null) return;
        // ---- Initialise chaque bouttons en rapport avec les capacités actuel ---- //
        for (int i = 0; i < _actionButton.Count; i++)
        {
            /*
            _actionButton[i].GetComponent<ButtonAction>().Input.text = (i + 1).ToString();*/

            if (_actionCapacity.Count < i) { break; }

            if (_actionCapacity.Count < i) { break; }
            
            // Nettoie la liste d'action du boutton
            _actionButton[i].GetComponent<Button>().onClick.RemoveAllListeners();

            // -- Initialise les données du boutton initialiser -- //

            // Insert l'action effectuer si le boutton est appuyer
            int index = i;
            _actionButton[i].GetComponent<Button>().onClick.AddListener(() => SetActionMode(_actionCapacity[index].typeA, _actionCapacity[index].sound));
            _actionButton[i].GetComponent<Button>().image.color = _pC.Color;

            // // Verifie que l'objet a une icone et l'affiche
            // if (_actionCapacity[i].icon != null)
            // {
            //     _actionButton[i].GetComponent<Image>().sprite = _actionCapacity[i].icon;
            // }

            if (_actionCapacity[index].typeA == ActionTypeMode.Reload)
            {
                WatchReloadButton(_actionButton[i]);
            }

            if (_actionCapacity[index].typeA == ActionTypeMode.Competence1)
            {
                AdaptIcone(_actionButton[i], _cH.GetCurrentAbilityCooldown);
            }

            if (_actionCapacity[index].typeA == ActionTypeMode.Competence2)
            {
                AdaptIcone(_actionButton[i], _cH.GetCurrentAbilityAltCooldown);
            }
                
                
                for(int ii = 0 ; ii < _actionButton[i].transform.childCount; ii++)
                {
                    if(_actionButton[i].transform.GetChild(ii).TryGetComponent<Image>( out Image iimage) )
                    {
                        iimage.sprite = _actionCapacity[i].icon;
                    }
                }
                

                        
                
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
                    onSelected.callback.AddListener((eventData) => { DisplayPopUp(_actionCapacity[indexTrigger] , _actionButton[indexTrigger].transform.position, _actionCapacity[indexTrigger].description, _actionCapacity[indexTrigger].name); });
                    
                    
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
        if (_cH != null)
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
                    Debug.Log("on rajoute Start "+_actionButton.Count+" End "+_actionCapacity.Count );

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
                    Debug.Log("upprime les bouttons en trop  Start "+_actionButton.Count+" End "+_actionCapacity.Count );
                    InstantiateButton(_actionButton.Count, _actionCapacity.Count, _prefabButton, _layoutGroup);

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
    private void DisplayPopUp( DataCharacter.Capacity data  , Vector3 position, string description = " ", string title = "Information" )
    {
        if(_cH.IsMoving) return; // [UI] [GRID] Empêcher la prévisualisation quand notre perso se déplace

        GridManager.ResetCasesPreview(_pC.GetCurrentCharactedSelected.CurrentCase.GridParent);
        
        if(data.typeA == ActionTypeMode.Overwatch)
        {
            _pC.GetCurrentCharactedSelected.PreviewOverwatch();
            _pC.IsPreviewing = true;
        }
        else
        {
            DataWeapon weapon = _pC.GetWeaponFromActionMode(data.typeA);
            if(data.typeA != ActionTypeMode.Reload)
                _pC.GetCurrentCharactedSelected.AttackRange(weapon, weapon.Range.casePreviewRange);

            _pC.IsPreviewing = true;    
        }

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
            DataWeapon weapon = _pC.GetWeaponFromActionMode(data.typeA);
            PopupActionBar _popup = _objectPopUp.GetComponent<PopupActionBar>();
            _popup.SetWidget(title,description, weapon.CostPoint.ToString(), _pC.Color ,weapon.Cooldown);
            // // Change le titre du PopUp
            // ModifyTextBox(_objectPopUp, "Title", title);
            // // Change la description du PopUp
            // ModifyTextBox(_objectPopUp, "Description", description);

            if (_objectPopUp.activeSelf == false) { _objectPopUp.SetActive(true); }
        }

    }

    private void HidePopUp()
    {
        // Si le popUp est initialiser, le cache
        if (_objectPopUp != null)
        {
            if (_objectPopUp.activeSelf == true) { _objectPopUp.SetActive(false); }
            
            if(_pC.GetCurrentCharactedSelected != null && !_pC.GetCurrentCharactedSelected.IsMoving)
            {
                _pC.IsPreviewing = false;  
                GridManager.ResetCasesPreview(_pC.GetCurrentCharactedSelected.CurrentCase.GridParent); 
            }
               

        }
    }

    /// <summary>
    /// Met a jour la boite de texte enfant d'un GameObject
    /// </summary>
    /// <param name="nameChild">Enfant a chercher du GameObject</param>
    /// <param name="valueString">Chaine de charactere a inserer</param>
    private void ModifyTextBox(GameObject parent, string nameChild, string valueString)
    {
        TextMeshProUGUI textMesh;
        Text text;
        Transform textBox;

        // -- Initialise le texte de la boite de texte de "Description" -- //
        textBox = parent.transform.Find(nameChild);
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
    public void SetActionMode(ActionTypeMode actionType, string sound = null)
    {
        // Si le playerController est null alors quitte la fonction
        if (_pC == null || _cH.IsMoving) { return; }

        if (sound == null && _soundResetSelection != null ) { sound = _soundResetSelection; }



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
