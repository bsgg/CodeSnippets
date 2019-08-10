using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public PowerUpAttribute.AttributeType doorTypeMask = PowerUpAttribute.AttributeType.MAGIC;

    private int attributeMask;

    void Start()
    {
        attributeMask = (int)doorTypeMask;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AttributeManager attManager = collision.gameObject.GetComponent<AttributeManager>();
        if (attManager && (attManager.attributes & attributeMask) != 0)
        {
            Debug.Log("Has attribute " + doorTypeMask.ToString());
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            Debug.Log("Not Has attribute " + doorTypeMask.ToString());

            GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<BoxCollider>().isTrigger = false;
    }

   

    
    void Update()
    {
        
    }
}
