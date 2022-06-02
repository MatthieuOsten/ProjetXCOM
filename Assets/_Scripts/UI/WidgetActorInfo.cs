using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class WidgetActorInfo : HintstringProperty
{
    Character _actor;
    [SerializeField] Image _iconActor;
    float _lowOpacity = 0.1f;

    protected override void Start() {
        _actor = relatedObject.GetComponent<Character>();
        offset = new Vector3(0, 23, 0);

        _iconActor.sprite = _actor.GetCharacterIcon();
    }

    protected override void Update()
    {
        if (LevelManager.GetCurrentController() == _actor.Owner)
            JaugeProgression.GetComponent<Image>().color = Color.green;
        else
            JaugeProgression.GetComponent<Image>().color = Color.red;

        Team currentTeam = LevelManager.GetCurrentController();
        Actor _selectedActor = null;
        if(currentTeam is PlayerController)
        {
            PlayerController player = (PlayerController)currentTeam;
            _selectedActor = player.GetCurrentActorSelected;
        }
        Image[] images = gameObject.GetComponentsInChildren<Image>();

        
        // Si le personnage du widget correspond � la team qui joue
        if (_selectedActor != null && _actor.Owner == currentTeam)
        {
            // Si le personnage s�lectionner n'est pas celui du widget, on diminue l'opacit�
            if (_selectedActor != _actor)
            {
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
        else
        {
            for (int i = 0; i < textComponent.Length; i++)
            {
                textComponent[i].color = SetOpacity(textComponent[i].color, 1);
            }
            for (int i = 0; i < images.Length; i++)
            {
         
                images[i].color = SetOpacity(images[i].color, 1);
            }
        }
        //textComponent[0].text = _actor.Data.name; 
        textComponent[0].text = "";
        textComponent[1].text = _actor.Health+"/"+_actor.Data.Health;
        textComponent[2].text = _actor.CurrentActionPoint + "/" + _actor.MaxActionPoint + " PA ";
        progression = (float)_actor.Health/(float)_actor.Data.Health;

        if(!_actor.CanAction)
        {
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


        base.Update();
    }

    Color SetOpacity(Color colorToChange, float value)
    {
        colorToChange.a = value;
        return colorToChange;
    }

}
