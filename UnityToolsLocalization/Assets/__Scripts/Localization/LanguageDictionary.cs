using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguageDictionary 
{
    public string language;
    public Dictionary<string, string> dictionary;
    public LanguageDictionary()
    {
        dictionary = new Dictionary<string, string>();
    }
}
