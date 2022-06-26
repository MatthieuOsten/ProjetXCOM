using System;
using UnityEngine;
using UnityEditor;

namespace Matt_CustomAttribues
{
    #region ReadOnly

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        public class ReadOnly : PropertyAttribute
        {

        }

        [CustomPropertyDrawer(typeof(ReadOnly))]
        public class ReadOnlyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label);
                GUI.enabled = true;
            }
        }

    #endregion 

}
