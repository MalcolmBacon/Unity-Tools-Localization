using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateLanguageList
{
    [MenuItem("Assets/Create/Language List")]
    public static LanguageList Create()
    {
        LanguageList asset = ScriptableObject.CreateInstance<LanguageList>();

        AssetDatabase.CreateAsset(asset, "Assets/LanguageList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
