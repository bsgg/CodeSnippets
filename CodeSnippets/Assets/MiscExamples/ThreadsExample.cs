using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadsExample : MonoBehaviour
{
    public float[] randomList;

    private System.Threading.Thread thread;

    void Start()
    {
        randomList = new float[1000000];

        thread = new System.Threading.Thread(UpdateThread);
        thread.Start();

        

    }
    private void UpdateThread()
    {
        while (true)
        {
            Generate();
        }

    }    

    private void Generate()
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < randomList.Length; i++)
        {
            randomList[i] = (float)rnd.NextDouble();
        }
    }
}
