using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
    bool dropdown;
    float height;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (dropdown) // If dropdown is enabled, extend the height of our property in the editor to display the value
        {
            return height + 25f;
        }
        return 20f;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label); // Draw label
        position.width -= 34;
        position.height = 18;

        Rect valueRect = new Rect(position);
        valueRect.x += 15;
        valueRect.width -= 15;

        Rect foldButtonRect = new Rect(position);
        foldButtonRect.width = 15;

        dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");

        position.x += 15;
        position.width -= 15;

        SerializedProperty key = property.FindPropertyRelative("key");
        key.stringValue = EditorGUI.TextField(position, key.stringValue);

        position.x += position.width + 2;
        position.width = 17;
        position.height = 17;

        //Draw a search button after the text field 
        //Texture searchIcon = (Texture)Resources.Load("search");
        GUIContent searchContent = new GUIContent("S");

        if (GUI.Button(position, searchContent))
        {
            TextLocalizerSearchWindow.Open();
        }

        position.x += position.width + 2;

        //Add or edit button
        //Texture storeIcon = (Texture)Resources.Load("store");
        GUIContent storeContent = new GUIContent("+");

        if (GUI.Button(position, storeContent))
        {
            TextLocalizerEditWindow.Open(key.stringValue);
        }

        if (dropdown) //If dropdown is active, draw a label with the value returned 
        {
            var value = LocalizationSystem.GetLocalizedValue(key.stringValue);
            GUIStyle style = GUI.skin.box;
            height = style.CalcHeight(new GUIContent(value), valueRect.width);

            valueRect.height = height;
            valueRect.y += 21;
            EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
        }

        EditorGUI.EndProperty();        
    }
}
