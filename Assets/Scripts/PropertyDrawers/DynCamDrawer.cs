using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(DynCamera))]
public class DynCamDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //EditorGUILayout.PropertyField(property);
        Rect pos = EditorGUI.PrefixLabel(position, new GUIContent("CamGO", "Camera game object"));
        EditorGUI.PropertyField(pos, property.FindPropertyRelative("camGO"), GUIContent.none);
        //pos.y += 18;
        //EditorGUI.PrefixLabel(pos, new GUIContent("Change Collider", "Collider used for triggering the camera"));
        //EditorGUI.PropertyField(pos, property.FindPropertyRelative("changeCollider"), GUIContent.none );
        EditorGUI.EndProperty();
    }

}