using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Platform
{
    public string Name;
    public float Probability;
}


public class GameplayProbability : MonoBehaviour
{
    [SerializeField] private Platform[] platformProbabilities;

    [SerializeField] private string selectedPlatform;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(TestPlaforms());
     

       
    }

    private IEnumerator TestPlaforms()
    {
        float[] probabilities = new float[platformProbabilities.Length];
        for (int i = 0; i < platformProbabilities.Length; i++)
        {
            probabilities[i] = platformProbabilities[i].Probability;
        }

        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(1.0f);

            int index = ChooseElement(probabilities);
            selectedPlatform = platformProbabilities[index].Name;


            Debug.Log("<color=cyan>" + "selectedPlatform: " + selectedPlatform + "</color> ");

        }
    }

    private int ChooseElement(float[] probs)
    {
        float total = 0;

        for (int i=0; i< probs.Length; i++)
        {
            total += probs[i];
        }

        float randomPoint = Random.value * total;

        Debug.Log("<color=cyan>" + "Selected Random: " + randomPoint + "</color> ");

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }

        Debug.Log("<color=cyan>" + "Selected probs: " + probs.Length + "</color> ");

        return probs.Length - 1;
    }
    
}
