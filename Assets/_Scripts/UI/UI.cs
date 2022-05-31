using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
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
    TextMeshProUGUI myText;
    [SerializeField] private GameObject _textCompetence2;
    [SerializeField] private TextMeshProUGUI _textDebug;

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
    private void GetActualScripts()
    {
        _pC = (PlayerController)LevelManager.GetCurrentController();
        _cH = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>();
    }
    private void MaximumAmmo()
    {
         _myAmmoMax = _cH.GetWeaponCapacityAmmo(0);
    }

    private void ShadeBar()
    {
        

        if(_pC.GetCurrentActorSelected == null)
        {
            _barreAction.gameObject.SetActive(false); // desactive la barre d'action
            _barreAction.color = new Color(_barreAction.color.r, _barreAction.color.g, _barreAction.color.b, 0.1f);
            foreach (Image image in _children)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
            }
        }
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

    private void AdaptBar()
    {
        //foreach(Image myPoint in _actionPoint)
        if (_cH != null)
        {
            DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;

            myText = _textCompetence2.GetComponent<TextMeshProUGUI>();

            _tir.GetComponent<Image>().sprite = data.SpriteTir;
            _vigilance.GetComponent<Image>().sprite = data.SpriteVigilance;
            _competence1.GetComponent<Image>().sprite = data.SpriteCompetence;
            _competence2.GetComponent<Image>().sprite = data.SpriteCompetence2;
            _icone.GetComponent<Image>().sprite = data.icon;

            /*if (data.weapons[0] != null)
            {
                _myAmmoMax = data.weapons[0].Munition;

                if (_ammoImage.Count < _myAmmoMax)
                {
                    _ammoImage.Add(_myAmmo);
                    Debug.Log("fonctionne");
                }
            }*/



            foreach (Image myPoint in _actionPoint)
            {
                myPoint.GetComponent<Image>().sprite = data.PointAction;
            }


            if (data.SpriteCompetence2 == null)
            {
                _competence2.image.enabled = false;
                _competence2.enabled = false;

                myText.enabled = false;
            }
        }
        else
        {
            _competence2.image.enabled = true;
            _competence2.enabled = true;

            //myText.enabled = true;

        }
    }

   /* private void ActualActionPoint()
    {
        DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;

    }*/

    private void ActualAmmo()
    {
        DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
        //_ammoImage = new List<Image>(_myAmmoMax);

        if(_ammoImage.Count < _myAmmoMax)
        {
            _ammoImage.Add(_myAmmo);           
        }

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

    public void SetActionModeAttack()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Attack;
        } 
        else
        {
            _pC.SelectionMode = SelectionMode.Selection;
            _pC.ActionTypeMode = ActionTypeMode.None;
        }

    }
    public void SetActionModeOverwatch()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Overwatch;
        }
        else
        {
            _pC.SelectionMode = SelectionMode.Selection;
            _pC.ActionTypeMode = ActionTypeMode.None;
        }

    }
    public void SetActionModeCompetence()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Competence1;
        }
        else
        {
            _pC.SelectionMode = SelectionMode.Selection;
            _pC.ActionTypeMode = ActionTypeMode.None;
        }


    }
    public void SetActionModeCompetenceAlt()
    {
        if (_pC.SelectionMode != SelectionMode.Action)
        {
            _pC.SelectionMode = SelectionMode.Action;
            _pC.ActionTypeMode = ActionTypeMode.Competence2;
        }
        else
        {
            _pC.SelectionMode = SelectionMode.Selection;
            _pC.ActionTypeMode = ActionTypeMode.None;
        }


    }
    public void EndTurn()
    {
        if(_pC.CanPassTurn)
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
