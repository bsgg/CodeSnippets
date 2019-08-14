using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public enum KeyType { NONE = 0, RED = 1, PURPLE = 2, GREEN = 4, BLUE = 8, GOLD = 16 };

    public KeyType Key = KeyType.NONE;

    public int keyMaskValue { get; set; }

    public bool grantsAllKeys = false;

    void Start()
    {
        keyMaskValue = (int)Key;
    }

}
