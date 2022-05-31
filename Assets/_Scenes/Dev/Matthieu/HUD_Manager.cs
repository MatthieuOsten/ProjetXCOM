using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD_Manager : MonoBehaviour
{
    [SerializeField] private PlayerController _pC;
    [SerializeField] private Character _cH;
    // [SerializeField] private DataWeapon _weapon;
    [SerializeField] private Image _barreAction;
    [SerializeField] private Button _tir;
    [SerializeField] private Button _vigilance;
    [SerializeField] private Button _competence1;
    [SerializeField] private Button _competence2;
    [SerializeField] private Image _icone;
    [SerializeField] private Image _myAmmo;
    [SerializeField] private List<Image> _children;
    [SerializeField] private List<Image> _actionPoint;
    [SerializeField] private List<Image> _ammoImage;
    [SerializeField] private int _myAmmoMax;
    [SerializeField] private int _ammoImageIndex;
    [SerializeField] private TextMeshProUGUI __myText;
    [SerializeField] private GameObject _textCompetence2;
    [SerializeField] private TextMeshProUGUI _textDebug;

    [SerializeField] private List<Button> _actionButton;
    //[SerializeField] private List<DataCapacity> _actionCapacity;
    [SerializeField] private GameObject _layoutGroup;

    public static UIManager Util;
    [SerializeField] UiProperty property;
    static UiProperty staticProperty;

    [SerializeField] static GameObject MessageBox;
    [SerializeField] static GameObject InputBox;
    [SerializeField] static GameObject ClientBox;

    [SerializeField] Image _overlayRed;

    // [Header("PAUSE")]
    // [SerializeField] GameObject MenuPause;
    // [SerializeField] TMP_Text MapName;
    // [SerializeField] TMP_Text MapDescription;
    // [SerializeField] Image MapPreview;

    // [SerializeField] TMP_Text commandText;

    [Header("SUBTITLE")]
    [SerializeField] TMP_Text subtitleComponent;
    static float _durationSubtitle;
    static TMP_Text subtitleCompo;


    [Header("HINTSTRING")]
    [SerializeField] Vector3 offset = new Vector3(0, 15, 0);
    [SerializeField] static GameObject HintstringList;

    void Awake()
    {
        MessageBox = property.MessageBox;
        InputBox = property.InputBox;
        ClientBox = property.ClientBox;
        staticProperty = property;



        HintstringList = new GameObject("HintstringList");
        HintstringList.transform.parent = transform;
        HintstringList.transform.SetSiblingIndex(0);

        subtitleCompo = subtitleComponent;



    }

    // Start is called before the first frame update
    void Start()
    {
        Util = this;

        InitPauseMenu();
        FindScripts();
    }

    // Set image, text from LevelData property in the LevelManager
    void InitPauseMenu()
    {
        // if(LevelManager.Util != null && LevelManager.Util.LevelData != null)
        // {
        //     MapName.text = LevelManager.Util.LevelData.Name;
        //     MapDescription.text = LevelManager.Util.LevelData.Description;
        //     MapPreview.sprite = LevelManager.Util.LevelData.PreviewImage;
        //     commandText.text = LevelManager.Util.LevelData.CommandText;
        // }
        // else
        // {
        //     Debug.LogError("LevelData in levelManager is not set, InitPauseMenu function fail");
        // }



    }
    // Update is called once per frame
    void Update()
    {
        if (subtitleCompo == null) subtitleCompo = subtitleComponent;
        UpdateHintstring();
        UpdateSubtitle();

        // if(LevelManager.Util.IsPaused)
        //     MenuPause.SetActive(true);
        // else
        //     MenuPause.SetActive(false);
    }

    void UpdateHintstring()
    {
        for (int i = 0; i < HintstringList.transform.childCount; i++)
        {
            Transform hintstring = HintstringList.transform.GetChild(i);
            HintstringProperty hintPro = hintstring.GetComponent<HintstringProperty>();
            if (hintPro.relatedObject == null)
                continue;

            Vector3 position = Camera.main.WorldToScreenPoint(hintPro.relatedObject.transform.position);

            // Permet de voir si l'object est derriere la camera
            bool condition = position.x < 0 || position.y < 0 || position.z < 0;
            if (!condition)
            {
                if (hintPro.offset == null && !condition)
                    hintstring.transform.position = position + offset;
                else
                    hintstring.transform.position = position + hintPro.offset;
            }
            else
            {
                hintstring.transform.position = new Vector3(-1000, 0, -10);
            }

        }

    }

    void UpdateSubtitle()
    {
        if (_durationSubtitle > 0)
        {
            _durationSubtitle -= Time.deltaTime;
            subtitleCompo.alpha = 1;

        }
        else
            subtitleCompo.alpha = 0;
    }

    public static void OverlayBlood(float health, float maxHealth)
    {
        float cap = maxHealth / 0.75f;
        float oof = health / maxHealth;
        if (health <= cap)
        {
            Util._overlayRed.color = new Color(Util._overlayRed.color.r, Util._overlayRed.color.g, Util._overlayRed.color.b, 1f - (oof));
        }
        else
        {
            Util._overlayRed.color = new Color(Util._overlayRed.color.r, Util._overlayRed.color.g, Util._overlayRed.color.b, 0);

        }
    }

    /*
        This function will create a subtitle on the screen
    */
    public static void CreateSubtitle(string message = "Name: Hello I'm a subtitle text", float duration = 10f)
    {
        subtitleCompo.text = message;
        _durationSubtitle = duration + 2;
    }




    /*
        This function will create a hintstring at the top of your desired gameobject, if the gameobject is deleted
        the hintstring will be deleted too (in HintstringProperty.cs)
    */
    public static HintstringProperty CreateHintString(GameObject aGameObject, string message = "Use [F] to interact.", float minDistance = 50f, Sprite icon = null)
    {
        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintstring on a non existant object (message : " + message);
            return null;
        }
        GameObject hintString = Instantiate(MessageBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        HintstringProperty component = hintString.GetComponent<HintstringProperty>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.setting = SettingHintstring.AlwaysShow;
        component.textComponent[0].text = message;
        if (icon == null)
        {
            component.icon.color = new Color(0, 0, 0, 0);
        }
        else
            component.icon.sprite = icon;

        return component;
    }
    public static HintstringProperty CreateHintInput(GameObject aGameObject, string message = "[F]", float minDistance = 50f)
    {
        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintInput on a non existant object (message : " + message);
            return null;
        }
        GameObject hintString = Instantiate(InputBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        HintstringProperty component = hintString.GetComponent<HintstringProperty>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.textComponent[0].text = message;

        return component;
    }

    public static HintstringProperty CreateHintClient(GameObject aGameObject, string message = "[F]", float minDistance = 50f, Sprite icon = null)
    {
        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintInput on a non existant object (message : " + message);
            return null;
        }
        GameObject hintString = Instantiate(ClientBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        HintstringProperty component = hintString.GetComponent<HintstringProperty>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.textComponent[0].text = message;
        component.offset = new Vector3(-50, 150, 0);

        if (icon == null)
        {
            component.icon.color = new Color(0, 0, 0, 0);
        }
        else
            component.icon.sprite = icon;

        return component;
    }


    public static HintstringProperty CreateBoxActorInfo(GameObject aGameObject, string message = "Actor Name", float minDistance = 50f, Sprite icon = null)
    {
        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintstring on a non existant object (message : " + message);
            return null;
        }
        GameObject hintString = Instantiate(MessageBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        WidgetActorInfo component = hintString.GetComponent<WidgetActorInfo>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.setting = SettingHintstring.AlwaysShow;
        component.textComponent[0].text = message;
        if (icon == null)
        {
            component.icon.color = new Color(0, 0, 0, 0);
        }
        else
            component.icon.sprite = icon;

        return component;
    }

    public static HintstringProperty CreateHitInfo(GameObject aGameObject, float health, float pa, float minDistance = 50f, Sprite icon = null)
    {
        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintstring on a non existant object (message : ");
            return null;
        }
        GameObject hintString = Instantiate(staticProperty.WidgetHitInfo, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        WidgetHitInfo component = hintString.GetComponent<WidgetHitInfo>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.setting = SettingHintstring.AlwaysShow;
        component.IsTemp = true;

        string IsPositiveOrNegatif(float value)
        {
            if (value > 0)
                return "+" + value;
            else
                return "" + value;
        }

        if (health != 0)
        {
            component.textComponent[0].text = IsPositiveOrNegatif(health) + "PV";
        }

        if (pa != 0)
            component.textComponent[1].text = IsPositiveOrNegatif(pa) + "PA";

        if (component.icon != null)
        {
            if (icon == null)
            {
                component.icon.color = new Color(0, 0, 0, 0);
            }
            else
                component.icon.sprite = icon;
        }
        return component;
    }



    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuStart");
    }

    public void Restart()
    {
        //SceneManager.LoadScene(LevelManager.Util.LevelData.sceneName);
    }
    public void Unpause()
    {
        //LevelManager.Util.IsPaused = false;
    }

    public int AmmoIndex
    {
        get { return _ammoImageIndex; }
        set
        {
            _ammoImageIndex = value;
        }
    }

    public List<Image> Ammo
    {
        get { return _ammoImage; }
        set
        {
            _ammoImage = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FindScripts();

        //_weapon = FindObjectOfType<DataWeapon>();

        //  _ammoImage = new List<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //ActualActionPoint();
        GetActualScripts();
        MaximumAmmo();
        ShadeBar();
    }

    private void FixedUpdate()
    {
        AdaptBar();
        ActualAmmo();
    }
    private void FindScripts()
    {
        _pC = FindObjectOfType<PlayerController>();
        //_cH = FindObjectOfType<Character>();
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
        if (_pC.GetCurrentActorSelected == null)
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

            _myText = _textCompetence2.GetComponent<TextMeshProUGUI>();

            _tir.GetComponent<Image>().sprite = data.SpriteTir;
            _vigilance.GetComponent<Image>().sprite = data.SpriteVigilance;
            _competence1.GetComponent<Image>().sprite = data.SpriteCompetence;
            _competence2.GetComponent<Image>().sprite = data.SpriteCompetence2;
            _icone.GetComponent<Image>().sprite = data.icon;

            foreach (Image myPoint in _actionPoint)
            {
                myPoint.GetComponent<Image>().sprite = data.PointAction;
            }


            if (data.SpriteCompetence2 == null)
            {
                _competence2.image.enabled = false;
                _competence2.enabled = false;

                _myText.enabled = false;
            }
        }
        else
        {
            _competence2.image.enabled = true;
            _competence2.enabled = true;

            //_myText.enabled = true;

        }
    }

    /* private void ActualActionPoint()
     {
         DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;

     }*/

    //Gere l'affichage du nombre actuel des munitions
    private void ActualAmmo()
    {
        DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
        //_ammoImage = new List<Image>(_myAmmoMax);

        //Ajoutes les images 
        if (_ammoImage.Count < _myAmmoMax)
        {
            _ammoImage.Add(_myAmmo);
        }

        //Rend invisible les images pour convenir au nombre actuel de munitions
        for (int i = 0; i < _ammoImage.Count; i++)
        {
            if (i >= _cH.GetWeaponCurrentAmmo())
            {
                _ammoImage[i].color = new Color(_ammoImage[i].color.r, _ammoImage[i].color.g, _ammoImage[i].color.b, 0f);
            }


            else
            {
                _ammoImage[i].color = new Color(_ammoImage[i].color.r, _ammoImage[i].color.g, _ammoImage[i].color.b, 1f);
            }


        }

        //remplace les images par les images de munitions
        foreach (Image myAmmo in _ammoImage)
        {
            myAmmo.GetComponent<Image>().sprite = data.Ammo;

            if (data.Ammo == null)
            {
                myAmmo.enabled = false;
            }
            else
            {
                myAmmo.enabled = true;
            }
        }
    }

    public void SetActionMode(ActionTypeMode actionType)
    {

        if (_pC.SelectionMode != SelectionMode.Action)
        {
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = actionType;
        }
        else
        {
            _pC.SelectionMode = SelectionMode.Selection;
            _pC.ActionTypeMode = ActionTypeMode.None;
        }

    }

    public void EndTurn()
    {
        if (_pC.CanPassTurn)
            LevelManager.Instance.PassedTurn = true;
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