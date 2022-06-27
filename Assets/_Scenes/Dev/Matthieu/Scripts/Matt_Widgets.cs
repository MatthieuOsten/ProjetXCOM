using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Matt_Widgets : MonoBehaviour
{

    #if UNITY_EDITOR
    [SerializeField] protected bool _debugMode;
    [SerializeField] protected bool DebugMode { get { return _debugMode; } set { _debugMode = value; } }
    #endif

    public virtual void StartSystem() {}
    public virtual void UpdateSystem() {}

}
