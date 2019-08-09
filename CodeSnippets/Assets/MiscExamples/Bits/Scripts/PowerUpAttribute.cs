using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAttribute : MonoBehaviour
{
    public enum PowerUpType { AddAttribute, RevertAttribute, AddMultiple, RevertMultiple, Reset };

    public PowerUpType powerUpType = PowerUpType.AddAttribute;

    public int AttributeValue = 0;

    public static int MAGIC = 16;
    public static int FLY = 8;
    public static int CHARISMA = 4;
    public static int INTELLIGENCE = 2;
    public static int INVISIBLE = 1;
   
}
