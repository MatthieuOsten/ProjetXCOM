using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor {
 protected virtual void OnGUI()
     {
         if (Event.current.type == EventType.MouseDrag)
             SceneView.currentDrawingSceneView.Repaint();

           System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            HandleUtility.Repaint();
            Debug.Log("FUCK YOU UNITY");
     }
}

//     using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(Something))]
// public class SomethingEditor : Editor
// {

//     protected virtual void OnSceneGUI()
//     {
//         if (Event.current.type == EventType.MouseDrag)
//             SceneView.currentDrawingSceneView.Repaint();
//     }

// }
