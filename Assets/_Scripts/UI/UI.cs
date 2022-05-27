using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private PlayerController _pC;
    [SerializeField] private Image _barreAction;
    [SerializeField] private Button _tir;
    [SerializeField] private Button _vigilance;
    [SerializeField] private Button _competence1;
    [SerializeField] private Button _competence2;
    [SerializeField] private Image _icone;
    [SerializeField] private List<Image> _children;
    [SerializeField] private List<Image> _actionPoint;
    [SerializeField] private List<Image> _ammo;
    TextMeshProUGUI myText;
    [SerializeField] private GameObject _textCompetence2;

    [SerializeField] private TextMeshProUGUI _textDebug;
    // Start is called before the first frame update
    void Start()
    {
        _pC = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _pC = (PlayerController) LevelManager.GetCurrentController();

        ShadeBar();
        AdaptBar();


        if (_pC.CharacterPlayer[_pC.CharacterIndex] == null)
        {
            _textDebug.text = "";
            return;
        }
        string debugString = "";
        /*
            Character : Name
            Action Point : i / i
            Health : 1 / 1
            State : Alive
            Ammo : 2/2
        */
        Character _char = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>();
        DataCharacter data = _char.Data;

        debugString += $"Character :{data.name} \n";
        debugString += $"Action Point :  {_char._currentActionPoint} / {data.ActionPoints} \n";
        debugString += $"Health : {_char.Health} \n";
        debugString += $"State :{_char.State} \n";
        debugString += $"Ammo :{_char.Ammo} /  \n";
        _textDebug.text = debugString;

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
        DataCharacter data = _pC.CharacterPlayer[_pC.CharacterIndex].GetComponent<Character>().Data;
        myText = _textCompetence2.GetComponent<TextMeshProUGUI>();

        _tir.GetComponent<Image>().sprite = data.SpriteTir;
        _vigilance.GetComponent<Image>().sprite = data.SpriteVigilance;
        _competence1.GetComponent<Image>().sprite = data.SpriteCompetence;
        _competence2.GetComponent<Image>().sprite = data.SpriteCompetence2;
        _icone.GetComponent<Image>().sprite = data.icon;

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
        LevelManager.Instance.PassedTurn = true;
    }

}
