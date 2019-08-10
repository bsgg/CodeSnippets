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
    public int attributes = 0; // No attributes

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

                switch (comp.powerUpType)
                {
                    case PowerUpAttribute.PowerUpType.AddAttribute:

                        attributes |= comp.AttributeValue; // Add the attribute value (OR)
                     break;

                    case PowerUpAttribute.PowerUpType.RevertAttribute:

                        // Revert the attribute (And and not)
                        // Example, the attribute Matig has value 8 = 0 1 0 0 0
                        // The character has 8 (Magic) +  2 (Fly) attributes =  0 1 0 1 0
                        // To revert, we need to negate Magic = 1 0 1 1 1
                        // And we need to use & to prevent the rest of attributes to be changed
                        //    Current Attributes     =     0 1 0 1 0
                        // Remove Magic              =  &  1 0 1 1 1   
                        // Result only Fly           =     0 0 0 1 0
                        attributes &= ~comp.AttributeValue;
                    break;

                    case PowerUpAttribute.PowerUpType.AddMultiple:
                        // Add multiples
                        attributes |= ((int)PowerUpAttribute.AttributeType.MAGIC | (int)PowerUpAttribute.AttributeType.INTELLIGENCE | (int)PowerUpAttribute.AttributeType.CHARISMA);
                     break;

                    case PowerUpAttribute.PowerUpType.RevertMultiple:
                        // Revert multiples
                        attributes &= ~((int)PowerUpAttribute.AttributeType.INTELLIGENCE | (int)PowerUpAttribute.AttributeType.MAGIC);
                    break;

                    case PowerUpAttribute.PowerUpType.Reset:
                        // Revert multiples
                        attributes = 0;
                    break;

                }


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
