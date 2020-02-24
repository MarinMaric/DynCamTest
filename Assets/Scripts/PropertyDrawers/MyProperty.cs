using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[Serializable]
public class MyProperty
{
    public int prime;
}

[CustomPropertyDrawer(typeof(MyProperty))]
public class MyPropertyDrawer : PropertyDrawer
{
    private readonly int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19 };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Prime"));
        var rect = new Rect(position.x, position.y, position.width, position.height);
        var prime = property.FindPropertyRelative("prime");
        var newValue = EditorGUI.IntPopup(rect, prime.intValue, primes.Select(primeValue => primeValue.ToString()).ToArray(), primes);

        if (newValue != prime.intValue)
        {
            prime.intValue = newValue;
            prime.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }
}