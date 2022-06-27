using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
#endif

namespace Matt_LayoutSystem
{

    /// <summary>
    /// Proprite prennent en compte lunite de mesure
    /// </summary>
    [System.Serializable]
    public struct Properties
    {
        /// <summary>
        /// Unite de mesure
        /// </summary>
        public enum Units
        {
            auto,
            pixel,
            percentage
        }

        [SerializeField] private float _value;
        [SerializeField] private Units _unit;

        public Units Unit { 
            get { return _unit; } 
            set { _unit = value; }
        }

        public float Value
        {
            // Retourne la valeur mais la force a se mettre a jour a chaque recuperation
            get { return (Value + 0); }

            set
            {

                if (Unit == Units.percentage)
                {
                    if (value > 0 && value <= 100)
                    {
                        _value = (int)value;
                    }
                    else if (value > 100) { _value = 100; }
                    else { _value = 0; }

                }

                if (Unit == Units.pixel)
                {
                    _value = value;
                }
            }

        }
        
        public void UpdateValue()
        {
            Unit = _unit;
            Value = _value;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class Offset
    {
        [SerializeField] private Units _unit;
        [Space(10)]
        [SerializeField] private Properties _left;
        [SerializeField] private Properties _right;
        [SerializeField] private Properties _top;
        [SerializeField] private Properties _bottom;

        /// <summary>
        /// Unite de mesure
        /// </summary>
        public enum Units
        {
            none,
            auto,
            pixel,
            percentage
        }


        public Properties Left { get { return _left; } set { _left = value; } }
        public Properties Right { get { return _right; } set { _right = value; } }
        public Properties Top { get { return _top; } set { _top = value; } }
        public Properties Bottom { get { return _bottom; } set { _bottom = value; } }

        public Units Unit
        {
            get { return _unit; }
            set
            {
                if (value == Units.auto)
                {
                    _unit = value;
                }
                else
                {
                    _left.Unit   = (Properties.Units)value--;
                    _right.Unit  = (Properties.Units)value--;
                    _top.Unit    = (Properties.Units)value--;
                    _bottom.Unit = (Properties.Units)value--;
                    _unit = value;
                }
            }
        }

        public void OnValidate()
        {
            Unit = _unit;
        }

    }

    [System.Serializable]
    public struct Layout
    {

        [SerializeField] private string name;
        [SerializeField] private int index;
        [SerializeField] private int count;

        [SerializeField] private Offset margin, padding;

        public void OnValidate()
        {
            margin.OnValidate();
            padding.OnValidate();
        }

    }

    #region UNITY_EDITOR
    #if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(Properties))]
    public class PropertiesDrawer : PropertyDrawer
    {
        int valueSize = 30;
        int fieldSpace = 10;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var amountField = new PropertyField(property.FindPropertyRelative("_value"));
            var unitField = new PropertyField(property.FindPropertyRelative("_unit"));

            // Add fields to the container.
            container.Add(amountField);
            container.Add(unitField);

            return container;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("_value");
            var unitProperty = property.FindPropertyRelative("_unit");

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            // Calculate rects
            var valueRect = new Rect(position.x, position.y, valueSize, position.height);
            var unitRect = new Rect(position.x + valueRect.width + fieldSpace, position.y, (position.x - valueRect.width), position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
            EditorGUI.PropertyField(unitRect, unitProperty, GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            // Stop action
            EditorGUI.EndProperty();
        }

    }

    #endif
    #endregion
}
