using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public PowerUpAttribute.AttributeType keyAttribute = PowerUpAttribute.AttributeType.NONE;

    public int keyMaskValue {get; set;}

    public bool GrantsAllAttributes = false;

    void Start()
    {
        keyMaskValue = (int)keyAttribute;
    }

}
