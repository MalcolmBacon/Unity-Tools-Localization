using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UseMyScriptableObject : MonoBehaviour
{
    public MyScriptableObjectClass myScriptableObject;
    private List<Light> myLights;

    // Use this for initialization
    void Start()
    {
        myLights = new List<Light>();
        foreach (Vector3 spawn in myScriptableObject.spawnPoints) 
        {
            GameObject myLight = new GameObject("Light");
            myLight.AddComponent<Light>();
            myLight.transform.position = spawn;
            myLight.GetComponent<Light>().enabled = false;
            if (myScriptableObject.colourIsRandom)
            {
                myLight.GetComponent<Light>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            }
            else
            {
                myLight.GetComponent<Light>().color = myScriptableObject.thisColour;
            }
            myLights.Add(myLight.GetComponent<Light>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Turning on lights");
            foreach (Light light in myLights)
            {
                light.enabled = !light.enabled;
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Updating lights");
            UpdateLights();
        }

    }

    void UpdateLights()
    {
        foreach (var myLight in myLights)
        {
            myLight.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
}