using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpeedTriggerList))]
public class SpeedColliderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("speedColliders"));
        if(GUILayout.Button(new GUIContent("+", "Adds new collider.")))
        {
            property.FindPropertyRelative("speedColliders").arraySize++;
        }
    }
}
