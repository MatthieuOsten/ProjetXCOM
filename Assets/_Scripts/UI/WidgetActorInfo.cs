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
        textComponent[0].text = _actor.Data.ClassName; 
        textComponent[1].text = _actor.Health+"/"+_actor.Data.Health;
        progression = (float)_actor.Health/(float)_actor.Data.Health;

        base.Update();
    }

}
