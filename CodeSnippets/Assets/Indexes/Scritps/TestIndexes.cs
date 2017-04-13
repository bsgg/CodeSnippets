using System;
using UnityEngine;

public class TestIndexes : MonoBehaviour {

    TempData names = new TempData();

    [Flags]
    enum Reptile
    {
        BlackMamba = 2,
        CottonMouth = 4,
        Wiper = 8,
        Crocodile = 16,
        Aligator = 32
    }

    // Use this for initialization
    void Start () {

        // Use the indexer's set accessor
        names[4] = "Luis";

        // Use the indexer's get accessor
        for (int i = 0; i < names.Length; i++)
        {
            string debugLine = string.Format("Element #{0} = {1}", i, names[i]);
            Debug.Log(debugLine);
        }

        Debug.Log("Index: " + names["Ivan"]);


        int snakes = 14;
        Debug.Log((Reptile)snakes);
    }
    
}
