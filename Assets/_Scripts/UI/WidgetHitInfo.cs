using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WidgetHitInfo : HintstringProperty
{
    Character _actor;
    Color _prevColorHealthText;
    Color _prevColorPaText;
    float _timeAnim = 0.5f;
    [Header("DEBUG")]
    [SerializeField]  bool Generate;

    protected override void Start() {
        //_actor = relatedObject.GetComponent<Character>();
        IgnoreRelatedObject = true;
        //textComponent[0].text = "";
        //textComponent[0].text = "";
        _prevColorHealthText = textComponent[0].color;
        _prevColorPaText = textComponent[1].color;
        textComponent[0].color = RemoveOpacity(textComponent[0].color);
        textComponent[1].color = RemoveOpacity(textComponent[1].color);
        offset = new Vector3(0, 30, 0);
        lifeTime = 2;
    }
    Color RemoveOpacity(Color _color)
    {
        _color.a = 0;
        return _color;
    }

    protected override void Update()
    {
        if (IsTemp)
        {
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(scale.x+ Time.deltaTime, scale.y + Time.deltaTime, scale.z + Time.deltaTime);
            }
                
            else
            {
                Debug.Log("Hintstring destroy because the lifetime is 0");
                Destroy(gameObject);
            }

        }
        if (Generate)
        {
            Start();
            Generate = false;
        }

        

        IncreaseOpacity(textComponent[0]);
        IncreaseOpacity(textComponent[1]);
        //base.Update();
      
        //textComponent[0].text = _actor.Data.name; Health hit
        //textComponent[1].text = _actor.Health+"/"+_actor.Data.Health; PA hit
       
    }

    void IncreaseOpacity(TMP_Text text)
    {
        float valueToAdd = Time.deltaTime / _timeAnim;
        Color _currentColor = text.color;
        if (_currentColor.a < 255)
        {
            _currentColor.a += valueToAdd;
        }
        text.color = _currentColor;
        

    }

}
