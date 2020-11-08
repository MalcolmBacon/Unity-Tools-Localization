using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class _LocalizationSystem : MonoBehaviour
{
    public static bool isInit;
    private static LanguageList languageList;
    private static List<LanguageDictionary> dictionaryList;
    public static void Init()
    {
        dictionaryList = new List<LanguageDictionary>();

        SetUpLanguageSettings();

        isInit = true;
    }
    private static void SetUpLanguageSettings()
    {
        languageList = (LanguageList)AssetDatabase.LoadAssetAtPath("Assets/LanguageList.asset", typeof(LanguageList));

        LoadDictionaries();
    }
    private static void LoadDictionaries()
    {

        if (languageList != null)
        {
            for (int i = 0; i < languageList.languageItemList.Count; i++)
            {
                LanguageDictionary languageDictionary = new LanguageDictionary()
                {
                    language = languageList.languageItemList[i].language
                };

                for (int j = 0; j < languageList.languageItemList[i].translationList.Count; j++)
                {
                    languageDictionary.dictionary.Add(
                        languageList.languageItemList[i].translationList[j].key,
                        languageList.languageItemList[i].translationList[j].translation
                    );
                    dictionaryList.Add(languageDictionary);
                }
            }
        }
    }
    public static string GetLocalizedValue(string key)
    {
        if (!isInit)
        {
            Init();
        }

        string value = null;

        if (dictionaryList.Any())
        {
            LanguageDictionary selectedLanguage = dictionaryList.FirstOrDefault(dict => dict.language == languageList.selectedLanguage);

            if (selectedLanguage != null)
            {
                selectedLanguage.dictionary.TryGetValue(key, out value);
                return value;
            }
        }
        return null;
    }
}
