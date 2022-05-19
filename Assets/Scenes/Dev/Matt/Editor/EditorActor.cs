using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Actor))]
public class EditorActor : Editor
{

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        ObjectPreview prev;
        prev.DrawPreview(Rect.zero);

        GUI.enabled = true;

        GUI.color = Color.red;
    }
}
