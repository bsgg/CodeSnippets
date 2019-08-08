using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeManager : MonoBehaviour
{
    static public int MAGIC = 16;

    public Text attributeDisplay;

    //32 bits long for attributes
    int attributes = 0; // No attributes

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "POWERUPATTRIBUTE")
        {
            PowerUpAttribute comp = other.GetComponent<PowerUpAttribute>();

            if (comp != null)
            {
                //attributes |= MAGIC; // Add attribute magic
                attributes |= comp.AttributeValue; // Add the attribute value
            }
        }
    }

    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        attributeDisplay.transform.position = screenPoint + new Vector3(0,-50,0);

        attributeDisplay.text = "Attributes: " + Convert.ToString(attributes, 2).PadLeft(8,'0');
    }
       
}
