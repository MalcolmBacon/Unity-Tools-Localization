﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TextLocalizerEditWindow : EditorWindow
{
    public static void Open(string key)
    {
        //Create instance of window and show as utility window 
        TextLocalizerEditWindow window = ScriptableObject.CreateInstance<TextLocalizerEditWindow>();
        window.titleContent = new GUIContent("Localizer Window");
        window.ShowUtility();
        window.key = key;
    }
    public string key;
    public string value;
    private void OnGUI()
    {
        // Create text field for key and word wrapped text area to input the value
        key = EditorGUILayout.TextField("Key :", key);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Value:", GUILayout.MaxWidth(50));

        EditorStyles.textArea.wordWrap = true;
        value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        //Add a button that will add/edit value in localization system
        if (GUILayout.Button("Add"))
        {
            if (!string.IsNullOrEmpty(LocalizationSystem.GetLocalizedValue(key)))
            {
                LocalizationSystem.Replace(key, value);
            }
            else
            {
                LocalizationSystem.Add(key, value);
            }
        }

        minSize = new Vector2(460, 250);
        maxSize = minSize;

    }

}
public class TextLocalizerSearchWindow : EditorWindow
{
    public static void Open()
    {
        //Draw rect based on mouse position
        TextLocalizerSearchWindow window = ScriptableObject.CreateInstance<TextLocalizerSearchWindow>();

        window.titleContent = new GUIContent("Localization Search");

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect r = new Rect(mouse.x - 250, mouse.y + 10, 10, 10);
        window.ShowAsDropDown(r, new Vector2(500, 300));
    }

    public string value;
    public Vector2 scroll;
    public Dictionary<string, string> dict;

    private void OnEnable()
    {
        dict = LocalizationSystem.GetDictionaryForEditor();
    }
    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();

        GetSearchResults();
    }

    private void GetSearchResults()
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        EditorGUILayout.BeginVertical();
        //Create scroll section that checks every key pair in dictionary for matching
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (KeyValuePair<string, string> item in dict)
        {
            if (item.Key.ToLower().Contains(value.ToLower()) || item.Value.ToLower().Contains(value.ToLower()))
            {
                EditorGUILayout.BeginHorizontal("box");
                //Texture closeIcon = (Texture)Resources.Load("close");
                GUIContent content = new GUIContent("X");

                if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + item.Key + "?", "This will remove the element from localization, are you sure?", "Confirm"))
                    {
                        LocalizationSystem.Remove(item.Key);
                        AssetDatabase.Refresh();
                        LocalizationSystem.Init();
                        dict = LocalizationSystem.GetDictionaryForEditor();
                    }
                }

                EditorGUILayout.TextField(item.Key);
                EditorGUILayout.LabelField(item.Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
