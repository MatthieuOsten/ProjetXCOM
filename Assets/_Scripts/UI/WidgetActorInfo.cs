using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class WidgetActorInfo : HintstringProperty
{
    Character _actor;
    [Header("WIDGETS")]
    [SerializeField] Image _iconActor;
    [SerializeField] Image _background;
    [SerializeField] Image _iconOverwatch;
    [SerializeField] Image _arrowSelected;
    float _lowOpacity = 0.1f;

    protected override void Start() {
        _actor = relatedObject.GetComponent<Character>();
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

    protected override void Update()
    {
        base.Update();
        // if (LevelManager.GetCurrentController() == _actor.Owner)
        //     JaugeProgression.GetComponent<Image>().color = Color.green;
        // else
        //     JaugeProgression.GetComponent<Image>().color = Color.red;

        Team currentTeam = LevelManager.GetCurrentController();
        Actor _selectedActor = null;
        if(currentTeam is PlayerController)
        {
            PlayerController player = (PlayerController)currentTeam;
            _selectedActor = player.GetCurrentActorSelected;
        }
        Image[] images = gameObject.GetComponentsInChildren<Image>();

        _iconOverwatch.gameObject.SetActive(_actor.IsOverwatching);
       

            //textComponent[0].text = _actor.Data.name; 
        textComponent[0].text = "";
        textComponent[1].text = _actor.Health+"/"+_actor.Data.Health;
        textComponent[2].text = _actor.CurrentActionPoint + "/" + _actor.MaxActionPoint + " PA ";
        progression = (float)_actor.Health/(float)_actor.Data.Health;

        // Si le personnage du widget correspond a la team qui joue
        if (_selectedActor != null && _actor.Owner == currentTeam)
        {
            // Si le personnage s�lectionner n'est pas celui du widget, on diminue l'opacit�
            if (_selectedActor != _actor)
            {
                _arrowSelected.gameObject.SetActive(false);

                _background.gameObject.SetActive(false);
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].color = SetOpacity(images[i].color, _lowOpacity);
                }
                for (int i = 0; i < textComponent.Length; i++)
                {
                    textComponent[i].color = SetOpacity(textComponent[i].color, _lowOpacity);
                }
            }
            else // Si on est la, c'est qu'on est sur le personnage s�lectionner par le widget
            {
                _arrowSelected.gameObject.SetActive(true);
                _arrowSelected.GetComponent<Animator>().SetBool("ScaleLoop", false);
                _background.gameObject.SetActive(true);
                for (int i = 0; i < textComponent.Length; i++)
                {
                    textComponent[i].color = SetOpacity(textComponent[i].color, 1);

                }
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].color = SetOpacity(images[i].color, 1);
                }
                // met le widget en premier plan
                transform.SetSiblingIndex(transform.parent.childCount-1);
            }

        }
        else // Si aucun personnage est selectionner
        {
            if(_actor.Owner.CanPlay && _actor.CanAction)
            {
                _arrowSelected.gameObject.SetActive(true);
                _arrowSelected.GetComponent<Animator>().SetBool("ScaleLoop", true);
            }
            else
            {
                _arrowSelected.gameObject.SetActive(false);
                _arrowSelected.GetComponent<Animator>().SetBool("ScaleLoop", true);
            }
           
            _background.gameObject.SetActive(false);
            for (int i = 0; i < textComponent.Length; i++)
            {
                textComponent[i].color = SetOpacity(textComponent[i].color, 0.8f);
            }
            for (int i = 0; i < images.Length; i++)
            {
         
                images[i].color = SetOpacity(images[i].color, 0.8f);
            }
            return;
        }
   

        if(!_actor.CanAction)
        {
            _arrowSelected.gameObject.SetActive(false);
            _arrowSelected.GetComponent<Animator>().SetBool("ScaleLoop", true);
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = SetOpacity(images[i].color, _lowOpacity);
            }
            for (int i = 0; i < textComponent.Length; i++)
            {
                textComponent[i].color = SetOpacity(textComponent[i].color, _lowOpacity);
            }
            return;
        }


     
    }

    Color SetOpacity(Color colorToChange, float value)
    {
        colorToChange.a = value;
        return colorToChange;
    }

}
