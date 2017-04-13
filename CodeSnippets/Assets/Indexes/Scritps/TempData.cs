using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TempData
{
    // Array of temperature values
    private string[] dataNames = new string[5] { "Bea", "Ivan", "Alberto", "Mario", "Noe" };


    // To enable client code to validate input 
    // when accessing your indexer.
    public int Length
    {
        get { return dataNames.Length; }
    }

    // Indexer declaration.
    // If index is out of range, the temps array will throw the exception.
    public string this[int index]
    {
        get
        {
            if (index < dataNames.Length)
            {
                return dataNames[index];
            }else
            {
                return "NULL";
            }           
        }
        set
        {
            if (index < dataNames.Length)
            {
                dataNames[index] = value;
            }           
        }
    }

    // This method finds the day or returns -1
    private int GetName(string name)
    {

        for (int j = 0; j < dataNames.Length; j++)
        {
            if (dataNames[j] == name)
            {
                return j;
            }
        }

        throw new System.ArgumentOutOfRangeException(name, "name must be in the form \"Bea\", \"Ivan\", etc");
    }

    // Indexer declaration.
    // If index is out of range, the temps array will throw the exception.
    public int this[string name]
    {
        get
        {
            return (GetName(name));
        }
    }    
}
