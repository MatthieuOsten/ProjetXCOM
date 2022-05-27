using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class WidgetActorInfo : HintstringProperty
{
    Character _actor;

    protected override void Start() {
        _actor = relatedObject.GetComponent<Character>();
    }

    protected override void Update()
    {
        textComponent[0].text = _actor.Data.name; 
        textComponent[1].text = _actor.Health+"/"+_actor.Data.Health;
        textComponent[2].text = _actor._currentActionPoint + "/" + _actor.Data.ActionPoints + " PA ";
        progression = (float)_actor.Health/(float)_actor.Data.Health;

        if (LevelManager.GetCurrentController() == _actor.Owner)
            JaugeProgression.GetComponent<Image>().color = Color.green;
        else
            JaugeProgression.GetComponent<Image>().color = Color.red;


        base.Update();
    }

}
