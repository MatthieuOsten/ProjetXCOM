using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Util;
    [SerializeField] UiProperty property;
    static UiProperty staticProperty;

    [SerializeField] static GameObject MessageBox;
    [SerializeField] static GameObject InputBox;
    [SerializeField] static GameObject ClientBox;

    [SerializeField] Image _overlayRed;

    [Header("Widget")]
    [SerializeField] UIPopupYourTurn YourTurnPopup;


    [Header("SUBTITLE")]
    [SerializeField] TMP_Text subtitleComponent;
    static float _durationSubtitle;
    static TMP_Text subtitleCompo;


    [Header("HINTSTRING")]
    [SerializeField] Vector3 offset = new Vector3(0, 15, 0);
    [SerializeField] static GameObject HintstringList;

    void Awake()
    {
        MessageBox = property.MessageBox;
        InputBox = property.InputBox;
        ClientBox = property.ClientBox;
        staticProperty = property;



        HintstringList = new GameObject("HintstringList");
        HintstringList.transform.parent = transform;
        HintstringList.transform.SetSiblingIndex(0);

        subtitleCompo = subtitleComponent;



    }

    // Start is called before the first frame update
    void Start()
    {
        Util = this;

        InitPauseMenu();
    }

    // Set image, text from LevelData property in the LevelManager
    void InitPauseMenu()
    {
   
    }
    // Update is called once per frame
    void Update()
    {
        if(subtitleCompo == null) subtitleCompo = subtitleComponent;
        UpdateHintstring();
        UpdateSubtitle();

    }

    void UpdateHintstring()
    {
        for (int i = 0; i < HintstringList.transform.childCount; i++)
        {
            Transform hintstring = HintstringList.transform.GetChild(i);
            HintstringProperty hintPro = hintstring.GetComponent<HintstringProperty>();
            if (hintPro.relatedObject == null)
                continue;

            Vector3 position = Camera.main.WorldToScreenPoint(hintPro.relatedObject.transform.position);
            // Permet de voir si l'object est derriere la camera
            bool condition = position.x <0 ||position.y < 0 || position.z < 0;
            if(!condition )
            {
                  if(hintPro.offset == null && !condition)
                        hintstring.transform.position = position + offset;
                    else
                        hintstring.transform.position = position + hintPro.offset;
            }
            else
            {
                hintstring.transform.position = new Vector3(-1000,0,-10);
            }
        }

    }

    void UpdateSubtitle()
    {
        if(_durationSubtitle > 0)
        {
            _durationSubtitle -= Time.deltaTime;
            subtitleCompo.alpha = 1;

        }
        else
            subtitleCompo.alpha = 0;
    }

    public static void OverlayBlood(float health, float maxHealth)
    {
        float cap = maxHealth/0.75f;
        float oof = health/maxHealth;
        if(health <= cap)
        {
            Util._overlayRed.color = new Color(Util._overlayRed.color.r, Util._overlayRed.color.g, Util._overlayRed.color.b, 1f-(oof) ); 
        }
        else
        {
            Util._overlayRed.color = new Color(Util._overlayRed.color.r, Util._overlayRed.color.g, Util._overlayRed.color.b, 0 ); 

        }
    }

    /*
        This function will create a subtitle on the screen
    */
    public static void CreateSubtitle(string message = "Name: Hello I'm a subtitle text", float duration = 10f)
    {
        subtitleCompo.text = message;
        _durationSubtitle = duration + 2 ;

    }

    /*
        This function will create a subtitle on the screen
    */
    public static void CreateYourTurnMessage(string message, Color teamColor) 
    {
        UIManager.Util.YourTurnPopup.SetWidget(message,teamColor);
    }
    



    /*
        This function will create a hintstring at the top of your desired gameobject, if the gameobject is deleted
        the hintstring will be deleted too (in HintstringProperty.cs)
    */
    public static HintstringProperty CreateHintString(GameObject aGameObject, string message = "Use [F] to interact.", float minDistance = 50f , Sprite icon = null)
    {
        if(aGameObject == null)
        {
            Debug.Log("Attempt to create a hintstring on a non existant object (message : "+message);
            return null;
        }
        GameObject hintString = Instantiate(MessageBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        HintstringProperty component = hintString.GetComponent<HintstringProperty>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.setting = SettingHintstring.AlwaysShow;
        component.textComponent[0].text = message;
        if(icon == null)
        {
            component.icon.color = new Color(0,0,0,0);
        }
        else
            component.icon.sprite = icon;

        return component;
    }
    public static HintstringProperty CreateHintInput(GameObject aGameObject, string message = "[F]", float minDistance = 50f)
    {
        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintInput on a non existant object (message : " + message);
            return null;
        }
        GameObject hintString = Instantiate(InputBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        HintstringProperty component = hintString.GetComponent<HintstringProperty>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.textComponent[0].text = message;
        
        return component;
    }

    public static HintstringProperty CreateHintClient(GameObject aGameObject, string message = "[F]", float minDistance = 50f, Sprite icon = null)
    {
        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintInput on a non existant object (message : " + message);
            return null;
        }
        GameObject hintString = Instantiate(ClientBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        HintstringProperty component = hintString.GetComponent<HintstringProperty>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.textComponent[0].text = message;
        component.offset = new Vector3(-50, 150, 0);

        if (icon == null)
        {
            component.icon.color = new Color(0, 0, 0, 0);
        }
        else
            component.icon.sprite = icon;

        return component;
    }

    /// <summary> Cr�e des boites d'information dans l'univers 3D pour afficher la vie et d'autres info du personnages </summary>
    public static HintstringProperty CreateBoxActorInfo(GameObject aGameObject, string message = "Actor Name", float minDistance = 50f , Sprite icon = null)    
    {
        if(aGameObject == null)
        {
            Debug.Log("Attempt to create a hintstring on a non existant object (message : "+message);
            return null;
        }
        GameObject hintString = Instantiate(MessageBox, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        WidgetActorInfo component = hintString.GetComponent<WidgetActorInfo>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.setting = SettingHintstring.AlwaysShow;
        component.textComponent[0].text = message;
        if(component.icon != null)
        {
            if (icon == null)
            {
                component.icon.color = new Color(0, 0, 0, 0);
            }
            else
                component.icon.sprite = icon;
        }
        

        return component;
    }

    public static HintstringProperty CreateHitInfo(GameObject aGameObject, float health , float pa, float minDistance = 50f, Sprite icon = null)
    {
        if(LevelManager.GameState != GameState.Ingame)
        {
            Debug.Log("HitInfoBox pas créer car le GameState n'est pas sur Ingame");
            return null;
        }

        if (aGameObject == null)
        {
            Debug.Log("Attempt to create a hintstring on a non existant object (message : " );
            return null;
        }
        GameObject hintString = Instantiate(staticProperty.WidgetHitInfo, aGameObject.transform.position, Quaternion.identity, HintstringList.transform);
        WidgetHitInfo component = hintString.GetComponent<WidgetHitInfo>();
        component.relatedObject = aGameObject;
        component.MinDistance = minDistance;
        component.setting = SettingHintstring.AlwaysShow;
        component.IsTemp = true;

        string IsPositiveOrNegatif(float value)
        {
            if (value > 0)
                return "+"+value;
            else
                return ""+ value;
        }

        if(health != 0)
        {
            component.textComponent[0].text = IsPositiveOrNegatif(health)+ "PV";
        }
        else
        {
            component.textComponent[0].text = System.String.Empty;
        }
            
        if(pa != 0)
        {
            component.textComponent[1].text = IsPositiveOrNegatif(pa);
            component.textComponent[1].GetComponentInChildren<Image>().enabled = true;
        }
        else
        {
            component.textComponent[1].GetComponentInChildren<Image>().enabled = false;
            component.textComponent[1].text = System.String.Empty;
        }

        if (component.icon != null)
        {
            if (icon == null)
            {
                component.icon.color = new Color(0, 0, 0, 0);
            }
            else
                component.icon.sprite = icon;
        }
        return component;
    }



    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuStart");
    }
    public void Restart()
    {
        //SceneManager.LoadScene(LevelManager.Util.LevelData.sceneName);
    }
    public void Unpause()
    {
        //LevelManager.Util.IsPaused = false;
    }
}
