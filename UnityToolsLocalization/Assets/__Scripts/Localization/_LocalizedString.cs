using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct _LocalizedString 
{
    public string key;
    public _LocalizedString(string key)
    {
        this.key = key;
    }
    public string value 
    {
        get 
        {
            return _LocalizationSystem.GetLocalizedValue(key);
        }
    }
    public static implicit operator _LocalizedString(string key)
    {
        return new _LocalizedString(key);
    }
}
