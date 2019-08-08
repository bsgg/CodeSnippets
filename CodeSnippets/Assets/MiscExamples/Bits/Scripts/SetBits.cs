using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBits : MonoBehaviour
{
    int bSequence = 8 + 3;

    void Start()
    {
        Debug.Log("Test: " + Convert.ToString(bSequence, 2));
    }

    void Update()
    {
        
    }
}
