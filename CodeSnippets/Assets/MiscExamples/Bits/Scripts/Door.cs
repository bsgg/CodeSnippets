using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyDoor.KeyType doorType = KeyDoor.KeyType.NONE;

    public int doorMaskValue { get; set; }

    void Start()
    {
        doorMaskValue = (int)doorType;
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
        if (attManager && (attManager.keys & doorMaskValue) != 0)
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
    }
}
