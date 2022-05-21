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
    public TMP_Text textComponent;
    public Image icon;
    public bool enable = true;

    public GameObject JaugeWidget;
    public GameObject JaugeProgression;
    public float progression = -1;

    public SettingHintstring setting;
    public Vector3 offset;

    GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    // A chaque update on check lexistance du gameobject, si il est null on delete le hintstring
    void Update()
    {
        var isMissing = ReferenceEquals(relatedObject, null);
        if (relatedObject == null || isMissing)
        {
            Debug.Log("Hintstring destroy because the related gameObject is killed");
            Destroy(gameObject);
        }

        if (!enable )
        {
            textComponent.gameObject.SetActive(false);
            icon.gameObject.SetActive(false);
        }
        else
        {
            //Debug.Log("GameObject " + gameObject.name + " " + Vector3.Distance(Player.transform.position, relatedObject.transform.position));
            switch(setting)
            {
                case SettingHintstring.HideWithDistance:
                    if (relatedObject != null && Vector3.Distance(Player.transform.position, relatedObject.transform.position) < 3)
                        textComponent.gameObject.SetActive(true);
                    else
                        textComponent.gameObject.SetActive(false);
                    break;
                case SettingHintstring.AlwaysShow:
                        textComponent.gameObject.SetActive(true);
                    break;
                default:
                    break;

            }
            

           
            icon.gameObject.SetActive(true);
            //textComponent.enabled = true;
        }

        if (progression > -1)
        {
            JaugeWidget.SetActive(true);
            RectTransform compo = JaugeProgression.transform.GetComponent<RectTransform>();
            compo.sizeDelta = new Vector2(progression, compo.sizeDelta.y);
        }
        else
            JaugeWidget.SetActive(false);
        

    }
}
