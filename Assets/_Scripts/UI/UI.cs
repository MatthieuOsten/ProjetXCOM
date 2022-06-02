using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private PlayerController _pC;
    [SerializeField] private Character _cH;

    [SerializeField] private Image _barreAction;
    [SerializeField] private Button _tir;
    [SerializeField] private Button _vigilance;
    [SerializeField] private Button _competence1;
    [SerializeField] private Button _competence2;
    [SerializeField] private Button _reload;
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

            _tir.GetComponent<Image>().sprite = data.SpriteTir;
            _vigilance.GetComponent<Image>().sprite = data.SpriteVigilance;
            _competence1.GetComponent<Image>().sprite = data.SpriteCompetence;
            _competence2.GetComponent<Image>().sprite = data.SpriteCompetence2;
            _icone.GetComponent<Image>().sprite = data.icon;

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

            }

            else
            {
                _reload.gameObject.SetActive(false);
            }

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

        else
        {
            _competence2.enabled = true;
            _competence2.image.enabled = true;
            //myText.enabled = true;
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
        if(_cH.Ammo != null)
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

                addIconeTeam.GetComponent<Image>().sprite = character.GetCharacterIcon();
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
