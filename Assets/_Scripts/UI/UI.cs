using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private PlayerController _pC;
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
    [SerializeField] private List<Image> _ammo;
    [SerializeField] private int _myAmmoMax;
    [SerializeField] private int _ammoIndex;
    TextMeshProUGUI myText;
    [SerializeField] private GameObject _textCompetence2;

    public int AmmoIndex
    {
        get { return _ammoIndex; }
        set
        {
            _ammoIndex = value;
        }
    }

    public List<Image> Ammo
    {
        get { return _ammo; }
        set
        {
            _ammo = value;
        }       
    }

    // Start is called before the first frame update
    void Start()
    {
        _pC = FindObjectOfType<PlayerController>();
        //_weapon = FindObjectOfType<DataWeapon>();

      //  _ammo = new List<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _pC = (PlayerController) LevelManager.GetCurrentController();
       // DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;

        
        ShadeBar();

    }
    private void FixedUpdate()
    {
        AdaptBar();
    }

    private void ShadeBar()
    {
        if (_pC.OnEnemy)
        {         
           _barreAction.color = new Color(_barreAction.color.r,_barreAction.color.g,_barreAction.color.b,1f);
            foreach(Image image in _children)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }
            
        }

        if(_pC.OnEnemy == false)
        {
            _barreAction.color = new Color(_barreAction.color.r, _barreAction.color.g, _barreAction.color.b, 0.1f);
            foreach (Image image in _children)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
            }
        }
    }

    private void AdaptBar()
    {
        DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Actor_TestSoldier>().Data;

        _ammoIndex = _myAmmoMax;

        myText = _textCompetence2.GetComponent<TextMeshProUGUI>();

        _tir.GetComponent<Image>().sprite = data.SpriteTir;
        _vigilance.GetComponent<Image>().sprite = data.SpriteVigilance;
        _competence1.GetComponent<Image>().sprite = data.SpriteCompetence;
        _competence2.GetComponent<Image>().sprite = data.SpriteCompetence2;
        _icone.GetComponent<Image>().sprite = data.Icone;

        if (data.weapons[0] != null)
        {
            _myAmmoMax = data.weapons[0].Munition;

            if (_ammo.Count < _myAmmoMax)
            {
                _ammo.Add(_myAmmo);
                Debug.Log("fonctionne");
            }
        }
        

        for (int i = 0; i < _ammo.Count; i++)
        {

            if (i > AmmoIndex)
            {
                _ammo[i].color = new Color(_ammo[i].color.r, _ammo[i].color.g, _ammo[i].color.b, 0f);
            }

            else
            {
                _ammo[i].color = new Color(_ammo[i].color.r, _ammo[i].color.g, _ammo[i].color.b, 1f);
            }
        }

        foreach(Image myPoint in _actionPoint)
        {
            myPoint.GetComponent<Image>().sprite = data.PointAction;            
        }

        foreach(Image myAmmo in _ammo)
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

        if(data.SpriteCompetence2 == null)
        {
            _competence2.image.enabled = false;
            _competence2.enabled = false;

            myText.enabled = false;
        }

        else
        {
            _competence2.image.enabled = true;
            _competence2.enabled = true;

            myText.enabled = true;

        }
    }
}
