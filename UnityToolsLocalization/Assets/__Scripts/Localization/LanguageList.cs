using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageList : ScriptableObject
{
    [HideInInspector]
    public List<Language> languageItemList;
    [HideInInspector]
    public string selectedLanguage;
}
