using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Language 
{
    public string language;
    public List<Translation> translationList;
    public Language()
    {
        language = string.Empty;
        translationList = new List<Translation>();
    }
}
