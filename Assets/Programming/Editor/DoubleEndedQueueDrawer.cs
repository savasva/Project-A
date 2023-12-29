using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(DoubleEndedQueue<SerializedProperty>))]
public class DoubleEndedQueueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Find the `queue` field inside the `DoubleEndedQueue` object.
        SerializedProperty queueProperty = property.FindPropertyRelative("queue");

        if (queueProperty != null)
        {
            // Convert the `queue` field to a list and render it.
            List<SerializedProperty> queueList = new List<SerializedProperty>();
            for (int i = 0; i < queueProperty.arraySize; i++)
            {
                SerializedProperty element = queueProperty.GetArrayElementAtIndex(i);
                SerializedProperty value = element.FindPropertyRelative("value");
                queueList.Add(value);
            }

            // Render the list as a property field.
            EditorGUI.PropertyField(position, property, label);
        }
        else
        {
            EditorGUI.LabelField(position, label, new GUIContent("Invalid Queue Property"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Return the height of the custom property drawer field.
        return EditorGUI.GetPropertyHeight(property, label);
    }
}
