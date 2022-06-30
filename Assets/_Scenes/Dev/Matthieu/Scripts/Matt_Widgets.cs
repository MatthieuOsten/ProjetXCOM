using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Matt_Widgets : MonoBehaviour
{

    #if UNITY_EDITOR
    public virtual void SystemDebug() { 
    
            Debug.Log("Debug objet " + gameObject.name + " actif");
    
    }
    #endif

    [SerializeField] public Matt_HUDManager_V2 hudManager;

    public virtual void SystemStart() {
    
        InitHUDManager();
    
    }
    public virtual void SystemUpdate() {}
    public virtual void SystemReset() { }

    protected void InitHUDManager()
    {
        if (hudManager == null)
        {
            hudManager = FindObjectOfType<Matt_HUDManager_V2>();
        }
    }
    protected void InitHUDManager(bool valueBool)
    {
        if (hudManager == null && !valueBool)
        {

            hudManager = FindObjectOfType<Matt_HUDManager_V2>();

        } else if (valueBool) {

            hudManager = FindObjectOfType<Matt_HUDManager_V2>();

        }
    }
}
