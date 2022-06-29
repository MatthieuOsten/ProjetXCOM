using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Matt_HUDManager_V3 : MonoBehaviour
{
    [SerializeField] private PlayerController _pC;
    [SerializeField] private Character _cH;

    [Header("DATA CHARACTER")]
    [SerializeField] private int _actionPointMax;
    [SerializeField] private int _myAmmoMax;
    [SerializeField] private Image _icone;

    [Header("AMMO")]
    [SerializeField] private int _ammoImageIndex;
    [SerializeField] private GameObject imageAmmo;
    [SerializeField] private Image weaponImage;
    [SerializeField] private GameObject parentAmmo;
    
    [Header("ICON TEAM")]
    [SerializeField] private GameObject _iconeTeam;
    [SerializeField] private GameObject imageIconeTeam;
    [SerializeField] private GameObject parentIconeTeam;
    /// <summary> Correspond à la couleur qui sera afficher derrière la liste des personnages </summary>
    [SerializeField] private Image _glowTeam;

    [Header("LIST OBJECTS")]
    [SerializeField] private List<GameObject> _actionPoint;
    [SerializeField] private List<GameObject> _ammo;
    [SerializeField] private List<GameObject> _teamImage;

    [Header("TEXT")]
    [SerializeField] private GameObject _textCompetence2;
    [SerializeField] private TextMeshProUGUI _textDebug;

    [Header("ACTION BAR")]
    [SerializeField] private List<GameObject> _actionButton;
    [SerializeField] private List<DataCharacter.Capacity> _actionCapacity;
    [SerializeField] private GameObject _layoutGroup;
    [SerializeField] private GameObject _prefabButton;
    [SerializeField] private static string _soundResetSelection = "action_reset";
    [SerializeField] private int _difference;
    [SerializeField] private bool _updateActionBar;

    [Header("POPUP")]
    [SerializeField] public GameObject _objectPopUp;
    [SerializeField] private GameObject _prefabPopUp;

    public List<GameObject> TeamImage
    {
        get { return _teamImage; }
        set
        {
            _teamImage = value;
        }
    }

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
        WatchListTeam();

    }

    private void FixedUpdate()
    {
        ActualAmmo();
    }

    void WatchListTeam()
    {
        // gere l'affichage de leur etat
        for (int i = 0; i < _teamImage.Count; i++)
        {
            if (_teamImage[i] == null) return;
            RectTransform rectTrans = _teamImage[i].GetComponent<RectTransform>();
            rectTrans.pivot = new Vector2(0, 1);

            // Si un personnage est selectionner, on le met en surbrillance
            if (_pC.GetCurrentCharactedSelected != null)
            {
                if (i == _pC.CharacterIndex)
                {
                    rectTrans.localScale = new Vector3(1.3f, 1.3f, 1.5f);
                }

                else
                {
                    rectTrans.localScale = new Vector3(0.8f, 0.8f, 1f);
                }
            }
            else // Aucun perso selectionner, on les met tous de la meme opacity
            {
                rectTrans.localScale = new Vector3(1f, 1f, 1f);
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
        if ((Team)_pC != Matt_LevelManager.GetCurrentController())
        {
            foreach (GameObject image in _teamImage)
            {
                Destroy(image);
            }
            _teamImage = new List<GameObject>();
            _pC = (PlayerController)Matt_LevelManager.GetCurrentController();

            ListTeam(_iconeTeam);

        }



        if (_pC != null)
        {
            if (_cH != _pC.GetCurrentCharactedSelected)

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
        if (_cH == null)
        {
            _myAmmoMax = 0;
            return;
        }
        _myAmmoMax = _cH.GetWeaponCapacityAmmo(0);
    }



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
            _reload.GetComponentInChildren<ButtonAction>().ICONE.color = colorReload;
            // Debug.Log("marche");
        }


        else
        {
            colorReload.a = 0.5f;
            _reload.GetComponent<Image>().color = colorReload;
            _reload.GetComponent<Button>().interactable = false;
            _reload.GetComponentInChildren<ButtonAction>().ICONE.color = colorReload;
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
        Image image = competenceInstance.GetComponent<Image>();
        Button button = competenceInstance.GetComponent<Button>();

        if (_currentCooldown == 0)
        {
            colorCompetence1.a = 1f;
            button.interactable = true;
            image.color = colorCompetence1;
            buttonAction.Cooldown.text = string.Empty;
            buttonAction.ICONE.color = colorCompetence1;
        }

        else
        {
            colorCompetence1.a = 0.5f;
            image.color = colorCompetence1;
            button.interactable = false;
            buttonAction.Cooldown.text = Mathf.RoundToInt(_currentCooldown).ToString();
            buttonAction.ICONE.color = colorCompetence1;
        }
    }

    //Gere l'affichage du nombre actuel des munitions
    private void ActualAmmo()
    {
        if (_cH != null && _cH.Ammo != null)
        {

            if (_myAmmoMax == 0 || _myAmmoMax > 0)
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

            if (_myAmmoMax != 0 || _myAmmoMax! > 0)
            {
                weaponImage.gameObject.SetActive(true);
                parentAmmo.SetActive(true);
            }

            // Montre ou rend invisible le point d'action si utiliser
            SetDisplayAmmo(true);

        }
        else
        {
            SetDisplayAmmo(false);
        }
    }

    /// <summary>
    /// Met a jour les munitions pour les afficher ou cacher
    /// </summary>
    private void SetDisplayAmmo(bool visible)
    {

        if (visible)
        {
            parentAmmo.SetActive(true);

            for (int i = 0; i < _ammo.Count; i++)
            {
                // Recupere l'animator de la munition actuellement verifier
                Animator animation = _ammo[i].GetComponent<Animator>();

                // Si les munition sont en dessous du nombre maximum les active sinon desactive les munitions en trop
                if (i < _myAmmoMax) { _ammo[i].gameObject.SetActive(true); }
                else { _ammo[i].gameObject.SetActive(false); }

                // Si les munitions sont en dessous du nombre actuel allume la munition
                if (i < _myAmmoMax)
                {
                    if (animation.GetBool("On") == false)
                        animation.SetBool("On", true);

                    if (animation.GetBool("Actual") == true)
                        animation.SetBool("Actual", false);
                }
                // Si la munition est la derniere munition active l'animation de la munition actuel
                else if (i == _myAmmoMax)
                {
                    if (animation.GetBool("On") == false)
                        animation.SetBool("On", true);

                    if (animation.GetBool("Actual") == false)
                        animation.SetBool("Actual", true);
                }
                // Si la munition est au dessus de la munition actuel eteint la munition
                else
                {
                    if (animation.GetBool("On") == true)
                        animation.SetBool("On", false);

                    if (animation.GetBool("Actual") == true)
                        animation.SetBool("Actual", false);
                }
            }
        }
        else
        {
            parentAmmo.SetActive(false);
        }

    }

    /// <summary>
    /// Adapt les icones pour chaque membre de la team
    /// </summary>
    /// <param name="addIconeTeam"></param>
    private void ListTeam(GameObject addIconeTeam)
    {
        _iconeTeam = addIconeTeam;
        _glowTeam.color = _pC.Data.Color;

        foreach (GameObject goCharacter in _pC.CharacterPlayer)
        {
            Character character = goCharacter.GetComponent<Character>();

            // Instantie le nombre de d'image, ajoute au bon gameobject et à une liste

            if (character == null) continue;
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
            Matt_LevelManager.Instance.PassedTurn = true;

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
