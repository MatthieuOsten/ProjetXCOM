using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Data))]
public class EditorActor : Editor
{

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        GUI.color = Color.red;
    }
}
