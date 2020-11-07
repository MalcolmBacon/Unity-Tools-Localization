using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List", order = 1)]
public class MyScriptableObjectClass : ScriptableObject
{
    public string objectName = "New MyScriptableObject";
    public bool colourIsRandom = false;
    public Color thisColour = Color.white;
    public Vector3[] spawnPoints;
}
