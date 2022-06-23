using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class WidgetActorInfo : HintstringProperty
{
    Character _actor;

    [Header("WIDGET SPECIFICITY")]
    [SerializeField] Image _iconActor;
    [SerializeField] Image _background;
    [SerializeField] Image _iconOverwatch;
    [SerializeField] Image _arrowSelected;
    [SerializeField] Image _back;
    [SerializeField] GameObject HealthPartPrefab;
    [SerializeField] GameObject PanelHealth;
    public bool IsFixed;
    private float _lowOpacity = 0.1f;
    private Image[] _imagesWidget;



    protected override void Start() {
        base.Start();
        

        if(relatedObject == null) return;


        _actor = relatedObject.GetComponent<Character>();
        AddHealthPart();
        _imagesWidget = transform.GetComponentsInChildren<Image>();

        offset = new Vector3(0, 45, 0);

        _iconActor.sprite = _actor.GetCharacterIcon();
        Color _color = _actor.GetTeamColor();
        _color.r = _color.r/0.75f;
        _color.g = _color.g/0.75f;
        _color.b = _color.b/0.75f;

        _background.color = _color;
        _background.gameObject.SetActive(false);
        JaugeProgression.GetComponent<Image>().color = _actor.GetTeamColor();




      
    }
    void AddHealthPart()
    {
         int childs = PanelHealth.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            DestroyImmediate(PanelHealth.transform.GetChild(i).gameObject);
        }
        for(int i = 0 ; i < _actor.MaxHealth; i++)
        {
            GameObject healthPart = Instantiate(HealthPartPrefab,Vector3.zero,Quaternion.identity, PanelHealth.transform);
            //healthPart.GetComponent<Image>().color = _actor.GetTeamColor();
        }
    }
    protected override void Update()
    {
        base.Update();
        if(!IsFixed)
        {
            _back.gameObject.SetActive(false);
        }
     
        Team currentTeam = LevelManager.GetCurrentController();
        Actor _selectedActor = null;
        if(currentTeam is PlayerController)
        {
            PlayerController player = (PlayerController)currentTeam;
            _selectedActor = player.GetCurrentActorSelected;
        }
        if(_actor == null) return; 

        _iconOverwatch.gameObject.SetActive(_actor.IsOverwatching);
       
        textComponent[0].text = System.String.Empty;
        textComponent[1].text = _actor.Health.ToString();
        textComponent[2].text = "x"+_actor.CurrentActionPoint ;
        progression = (float)_actor.Health/(float)_actor.Data.Health;

        bool frontView = false;
        // Si le personnage du widget correspond a la team qui joue
        if (_selectedActor != null && _actor.Owner == currentTeam)
        {
            // Si le personnage s�lectionner n'est pas celui du widget, on diminue l'opacit�
            if (_selectedActor != _actor)
            {
                _back.gameObject.SetActive(false);
                _background.gameObject.SetActive(false);
                for (int i = 0; i < _imagesWidget.Length; i++)
                {   
                    if(_imagesWidget[i] != _iconActor)
                        _imagesWidget[i].color = SetOpacity(_imagesWidget[i].color, 0);
                    else
                    {
                        if(!IsFixed)
                        {
                            Vector3 pos = _imagesWidget[i].rectTransform.localPosition;
                            pos.x = 0;
                            _imagesWidget[i].rectTransform.localPosition = pos; 
                        }
                    }
                }
                for (int i = 0; i < textComponent.Length; i++)
                {
                    textComponent[i].color = SetOpacity(textComponent[i].color, 0);
                }
            }
            else // Si on est la, c'est qu'on est sur le personnage s�lectionner par le widget
            {
                if(IsFixed) _back.gameObject.SetActive(true);
                
                _background.gameObject.SetActive(true);
                for (int i = 0; i < textComponent.Length; i++)
                {
                    textComponent[i].color = SetOpacity(textComponent[i].color, 1);

                }
                for (int i = 0; i < _imagesWidget.Length; i++)
                {
                    _imagesWidget[i].color = SetOpacity(_imagesWidget[i].color, 1);
                     
                    if(!IsFixed && _imagesWidget[i] == _iconActor )
                    {
                         Vector3 pos = _imagesWidget[i].rectTransform.localPosition;
                        pos.x = -33;
                        _imagesWidget[i].rectTransform.localPosition = pos; 
                    }   
                    
                }
                // met le widget en premier plan
                frontView = true;
            }

        }
        else // Si aucun personnage est selectionner
        {
            _back.gameObject.SetActive(false);
            _background.gameObject.SetActive(false);
            for (int i = 0; i < textComponent.Length; i++)
            {
                textComponent[i].color = SetOpacity(textComponent[i].color, 0.8f);
            }
            for (int i = 0; i < _imagesWidget.Length; i++)
            {
                _imagesWidget[i].color = SetOpacity(_imagesWidget[i].color, 0.8f);
                if(!IsFixed && _imagesWidget[i] == _iconActor )
                    {
                         Vector3 pos = _imagesWidget[i].rectTransform.localPosition;
                        pos.x = -33;
                        _imagesWidget[i].rectTransform.localPosition = pos; 
                    }
            }
            return;
        }

        if(!_actor.CanAction)
        {
            for (int i = 0; i < _imagesWidget.Length; i++)
            {
                _imagesWidget[i].color = SetOpacity(_imagesWidget[i].color, _lowOpacity);
            }
            for (int i = 0; i < textComponent.Length; i++)
            {
                textComponent[i].color = SetOpacity(textComponent[i].color, _lowOpacity);
            }
            return;
        }

        if(!IsFixed)
        {
            _back.gameObject.SetActive(false);
        }

        if(frontView && !IsFixed) transform.SetSiblingIndex(transform.parent.childCount-1);

    }

    Color SetOpacity(Color colorToChange, float value)
    {
        if(IsFixed) value = 1;
        colorToChange.a = value;
        return colorToChange;
    }

}
