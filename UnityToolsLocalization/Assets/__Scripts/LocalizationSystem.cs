using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationSystem
{
    public enum Language
    {
        English,
        French
    }
    public static Language language = Language.English;

    private static Dictionary<string, string> localizedEN;
    private static Dictionary<string, string> localizedFR;
    public static bool isInit;
    public static CSVLoader csvLoader;
    public static void Init()
    {
        csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        UpdateDictionaries();

        isInit = true;
    }
    public static void UpdateDictionaries()
    {
        localizedEN = csvLoader.GetDictionaryValuesForLanguage("EN");
        localizedFR = csvLoader.GetDictionaryValuesForLanguage("FR");
    }
    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!isInit)
        {
            Init();
        }
        return localizedEN;

    }
    public static string GetLocalizedValue(string key)
    {
        if (!isInit)
        {
            Init();
        }

        string value = key;

        switch (language)
        {
            case Language.English:
                localizedEN.TryGetValue(key, out value);
                break;
            case Language.French:
                localizedFR.TryGetValue(key, out value);
                break;
        }

        return value;
    }
    public static void Add(string key, string value)
    {
        value = ValidateInput(value);

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Add(key, value);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }
    public static void Replace(string key, string value)
    {
        value = ValidateInput(value);

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Edit(key, value);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }
    public static void Remove(string key)
    {
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Remove(key);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }
    private static string ValidateInput(string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }
        return value;
    }
}
