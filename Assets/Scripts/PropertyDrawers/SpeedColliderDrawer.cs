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
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button(new GUIContent("+", "Adds new collider.")))
        {
            property.FindPropertyRelative("speedColliders").arraySize++;
        }
        if(GUILayout.Button(new GUIContent("-", "Remove a collider.")))
        {
            if(property.FindPropertyRelative("speedColliders").arraySize > 0)
                property.FindPropertyRelative("speedColliders").arraySize--;
        }
        EditorGUILayout.EndHorizontal();
    }
}
