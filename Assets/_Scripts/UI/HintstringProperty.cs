using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SettingHintstring
{
    HideWithDistance,
    AlwaysShow
}

public class HintstringProperty : MonoBehaviour
{
    public GameObject relatedObject;
    public float MinDistance = 50f;
    public TMP_Text[] textComponent;
    public Image icon;
    public bool enable = true;

    public GameObject JaugeWidget;
    public GameObject JaugeProgression;
    public float progression = -1;

    public SettingHintstring setting;
    public Vector3 offset;



    GameObject Player;

    public bool IgnoreRelatedObject;
    public bool FollowRelatedObject = true;

    [SerializeField] protected float lifeTime =4 ;
    public bool IsTemp;

    /// <summary> Transform de la jauge du widget </summary>
    private RectTransform _rectJauge;

    protected virtual void Start()
    {
        _rectJauge = JaugeProgression.transform.GetComponent<RectTransform>();
    }
    // A chaque update on check lexistance du gameobject, si il est null on delete le hintstring
    protected virtual void Update()
    {
        if(IsTemp)
        {
            if (lifeTime > 0)
                lifeTime -= Time.deltaTime;
            else
            {
                Debug.Log("Hintstring destroy because the lifetime is 0");
                Destroy(gameObject);
            }
                
        }

        var isMissing = ReferenceEquals(relatedObject, null);
        if (!IgnoreRelatedObject && ( relatedObject == null || isMissing))
        {
            Debug.Log("Hintstring destroy because the related gameObject is killed");
            Destroy(gameObject);
        }

        if (!enable )
        {
            textComponent[0].gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
        }
        else
        {
            //Debug.Log("GameObject " + gameObject.name + " " + Vector3.Distance(Player.transform.position, relatedObject.transform.position));
            switch(setting)
            {
                case SettingHintstring.HideWithDistance:
                    if (relatedObject != null && Vector3.Distance(Player.transform.position, relatedObject.transform.position) < 3)
                        textComponent[0].gameObject.SetActive(true);
                    else
                        textComponent[0].gameObject.SetActive(false);
                    break;
                case SettingHintstring.AlwaysShow:
                        textComponent[0].gameObject.SetActive(true);
                    break;
                default:
                    break;

            }
            

            if(icon != null)
                icon.gameObject.SetActive(true);
        }

        if (progression > -1)
        {
            JaugeWidget.SetActive(true);            
            _rectJauge.sizeDelta = new Vector2(progression*100, _rectJauge.sizeDelta.y);
        }
        else
            JaugeWidget.SetActive(false);
        

    }
}
