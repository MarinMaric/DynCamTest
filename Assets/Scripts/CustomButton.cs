using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CustomButton : PropertyAttribute
{
    public string text;
    public CustomButton(string t) { text = t; }
}

[CustomPropertyDrawer(typeof(CustomButton))]
[CanEditMultipleObjects]
public class CustomButtonPropertyDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        
        if(property.FindPropertyRelative("text").stringValue == "Add Curve")
        {
            if (GUI.Button(position, "Add Curve"))
            {
                var dynCam = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.activeCameraIndex];
                dynCam.AddCurve();
            }
        }
        if(property.FindPropertyRelative("text").stringValue == "Clear Curve")
        {
            if (GUI.Button(position, "Clear Curve"))
            {
                var dynCam = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.activeCameraIndex];
                dynCam.ClearCurve();
            }
        }

    }
}