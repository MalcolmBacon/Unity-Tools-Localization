using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class LanguageLocalizationEditor : EditorWindow
{
    public LanguageList languageList;
    private string newKey;
    private int viewIndex = 1;
    [MenuItem("Window/Language Localization Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(LanguageLocalizationEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            languageList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(LanguageList)) as LanguageList;
        }
    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Language Localization Editor", EditorStyles.boldLabel);
        if (languageList != null)
        {
            if (GUILayout.Button("Show Selected Language List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = languageList;
            }
        }
        GUILayout.EndHorizontal();

        if (languageList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Language List"))
            {
                CreateNewLanguageList();
            }
        }

        GUILayout.Space(20);

        if (languageList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                GUI.FocusControl(null);
                if (viewIndex > 1)
                {
                    viewIndex--;
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                GUI.FocusControl(null);
                if (viewIndex < languageList.languageItemList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Language", GUILayout.ExpandWidth(false)))
            {
                GUI.FocusControl(null);
                AddNewLanguage();
            }
            if (GUILayout.Button("Delete Language", GUILayout.ExpandWidth(false)))
            {
                if (languageList.languageItemList.Any())
                {
                    GUI.FocusControl(null);
                    DeleteLanguage(viewIndex - 1);
                }
            }

            GUILayout.EndHorizontal();

            if (languageList.languageItemList == null)
            {
                Debug.Log("Language list is empty");
            }
            if (languageList.languageItemList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Language", viewIndex, GUILayout.ExpandWidth(false)), 1, languageList.languageItemList.Count);
                EditorGUILayout.LabelField("of   " + languageList.languageItemList.Count.ToString() + "  languages", GUILayout.MinWidth(100));//, GUILayout.ExpandWidth(false));
                if (GUILayout.Button("Use as language", new GUILayoutOption[] { GUILayout.Width(150), GUILayout.MaxWidth(150), GUILayout.MinWidth(150) }))
                {
                    SetGlobalLanguage();
                }
                GUILayout.EndHorizontal();


                languageList.languageItemList[viewIndex - 1].language = EditorGUILayout.TextField("Language Name", languageList.languageItemList[viewIndex - 1].language as string);

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                newKey = EditorGUILayout.TextField("Add New Key: ", newKey, GUILayout.MinWidth(310));
                if (GUILayout.Button("Add New Key", new GUILayoutOption[] { GUILayout.MaxWidth(150), GUILayout.MinWidth(150) }))
                {
                    if (string.IsNullOrEmpty(newKey))
                    {
                        EditorUtility.DisplayDialog("Invalid entry", "Please enter a value", "OK");
                    }
                    else
                    {
                        AddNewKey(newKey);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                GUILayout.Label("Translation List", EditorStyles.boldLabel);


                for (int i = 0; i < languageList.languageItemList[viewIndex - 1].translationList.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(languageList.languageItemList[viewIndex - 1].translationList[i].key, GUILayout.MaxWidth(150));

                    languageList.languageItemList[viewIndex - 1].translationList[i].translation = EditorGUILayout.TextField(languageList.languageItemList[viewIndex - 1].translationList[i].translation, GUILayout.MinWidth(155));
                    if (GUILayout.Button("Delete Key", new GUILayoutOption[] { GUILayout.MaxWidth(150), GUILayout.MinWidth(150) }))
                    {
                        DeleteKey(i);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                GUILayout.Label("Language list is empty");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(languageList);
        }
    }
    private void AddNewKey(string newKey)
    {
        newKey = CleanKeyInput(newKey);
        //Check to see if key already exists
        if (KeyAlreadyExists(newKey))
        {
            EditorUtility.DisplayDialog("Key already exists", "Please enter a unique key", "OK");
            return;
        }
        //Add new key to all languages
        for (int i = 0; i < languageList.languageItemList.Count; i++)
        {
            Translation translation = new Translation()
            {
                key = newKey
            };
            languageList.languageItemList[i].translationList.Add(translation);
        }
    }
    private void SetGlobalLanguage()
    {
        languageList.selectedLanguage = languageList.languageItemList[viewIndex - 1].language;
    }
    private void ResetGlobalLanguage()
    {
        languageList.selectedLanguage = string.Empty;
    }
    private bool KeyAlreadyExists(string newKey)
    {
        return languageList.languageItemList.Any(language => language.translationList.Any(t => t.key == newKey));
    }
    private void DeleteKey(int index)
    {
        //Delete key from all languages
        for (int i = 0; i < languageList.languageItemList.Count; i++)
        {
            languageList.languageItemList[i].translationList.RemoveAt(index);
        }
    }
    private string CleanKeyInput(string key)
    {
        return key.Trim();
    }
    private void AddNewLanguage()
    {
        List<Translation> translations = new List<Translation>();
        List<string> activeKeys = GetAllActiveKeys();
        foreach (string activeKey in activeKeys)
        {
            translations.Add(new Translation()
            {
                key = activeKey
            });
        }

        //Create new language, grab keys from previous language
        Language languageItem = new Language()
        {
            translationList = translations
        };

        languageItem.language = "New Language";
        languageList.languageItemList.Add(languageItem);
        viewIndex = languageList.languageItemList.Count;

        if (languageList.languageItemList.Count == 1)
        {
            SetGlobalLanguage();
        }
    }
    private void DeleteLanguage(int index)
    {
        bool removingSelectedLanguage = false;

        if (languageList.selectedLanguage == languageList.languageItemList[index].language)
        {
            removingSelectedLanguage = true;
        }

        languageList.languageItemList.RemoveAt(index);

        if (!languageList.languageItemList.Any())
        {
            ResetGlobalLanguage();
        }
        else
        {
            if (removingSelectedLanguage)
            {
                SetGlobalLanguage();
            }
        }
    }
    private List<string> GetAllActiveKeys()
    {
        List<string> activeKeys = new List<string>();
        if (languageList != null)
        {
            if (languageList.languageItemList.Count > 0)
            {
                activeKeys = languageList.languageItemList[0].translationList.Select(item => item.key).ToList();
            }
        }
        return activeKeys;
    }
    private void CreateNewLanguageList()
    {
        viewIndex = 1;
        languageList = CreateLanguageList.Create();
        if (languageList)
        {
            languageList.languageItemList = new List<Language>();
            string relPath = AssetDatabase.GetAssetPath(languageList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }
}
