using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public class _TextLocalizerUI : MonoBehaviour
{
    TextMeshProUGUI textField;
    public _LocalizedString localizedString;
    private void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = localizedString.value;
    }
}
