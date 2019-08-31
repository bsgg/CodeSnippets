using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAttribute : MonoBehaviour
{
    public enum PowerUpType { AddAttribute, RevertAttribute, AddMultiple, RevertMultiple, Reset, Toggle };

    public PowerUpType powerUpType = PowerUpType.AddAttribute;

   

    public enum AttributeType {NONE = 0, INVISIBLE = 1, INTELLIGENCE = 2, CHARISMA = 4, FLY = 8, MAGIC = 16};

    public AttributeType attributeType = AttributeType.NONE;

    public int AttributeValue { get; set; }

    /*public static int MAGIC = 16;
    public static int FLY = 8;
    public static int CHARISMA = 4;
    public static int INTELLIGENCE = 2;
    public static int INVISIBLE = 1;*/

    private void Start()
    {
        AttributeValue = (int)attributeType;
    }

}
