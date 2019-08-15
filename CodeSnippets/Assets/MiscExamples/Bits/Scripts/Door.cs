using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyDoor.KeyType doorType = KeyDoor.KeyType.NONE;

    private int doorMaskValue;

    void Start()
    {
        

        // If door is purple the attribute is red and blue
        if (doorType == KeyDoor.KeyType.PURPLE)
        {
            doorMaskValue = (((int)KeyDoor.KeyType.RED) | ((int)KeyDoor.KeyType.BLUE));
        }else
        {
            doorMaskValue = (int)doorType;
        }
    }

    public void EnableTrigger()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void DisableTrigger()
    {
        GetComponent<BoxCollider>().isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AttributeManager attManager = collision.gameObject.GetComponent<AttributeManager>();
        // Check if attManager has all the keys, not just one
        // Example
        // Door is Blue & Red      = 1 0 0 1
        // Att only Blue           = 1 0 0 0 &
        // Door & Attrib? != 0?    = 1 0 0 0
        // Result Yes
        if (attManager && ((attManager.keys & doorMaskValue) == doorMaskValue))
        {
            Debug.Log("Has attribute " + doorMaskValue.ToString());
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            Debug.Log("Not Has attribute " + doorMaskValue.ToString());
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<BoxCollider>().isTrigger = false;

        AttributeManager attManager = other.gameObject.GetComponent<AttributeManager>();

        if (attManager)
        {
            attManager.keys &= ~doorMaskValue;
        }
    }
}
