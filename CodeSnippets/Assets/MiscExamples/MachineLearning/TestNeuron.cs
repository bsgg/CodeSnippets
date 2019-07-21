using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public float[] weights;
    public float bias;

    public Neuron()
    {

    }

    public Neuron(float[] in_weights, float in_bias)
    {
        weights = in_weights;
        bias = in_bias;
    }

    public float FeedForward(float[] inputs)
    {
        // Weight inputs, add bias, then use the activation function
        
        //float total = np.dot(self.weights, inputs) + self.bias;

        float total = 0.0f;
        // Dot = (w1 * x1)  + (w2 *x2)
        for (int i=0; i<inputs.Length; i++)
        {
            total += weights[i] * inputs[i];
        }

        total += bias;



        return Sigmoid(total);
    }


    // Activation function f(x) = 1/(1 + e^(-x))
    private float Sigmoid(float x)
    {
        return (1 / (1 + Mathf.Exp(-x)));
    }

    // Derivative of sigmoid: f'(x) = f(x) * (1 - f(x))
    private float derived_sigmoid(float x)
    {
        float fx = Sigmoid(x);

        return fx * (1 - fx);

    }

}


public class TestNeuron : MonoBehaviour
{
    private Neuron n;
    void Start()
    {
        n = new Neuron();
        n.bias = 4;
        //w1 = 0, w2 = 1
        n.weights = new float[] {0 , 1};

        //x1 = 2, x2 = 3
        float[] inputs = new float[] {2,3 };

        Debug.Log("NEURON TEST: " + n.FeedForward(inputs));

        // Example 2
        // A neural network with:
        // 2 inputs
        // a hidden layer with 2 neurons(h1, h2)
        // an output layer with 1 neuron(o1)
        // Each neuron has the same weights and bias:

        float[] weights = new float[] { 0,1 };
        float bias = 0;

        // Hidden layers
        Neuron h1 = new Neuron(weights, bias);
        Neuron h2 = new Neuron(weights, bias);

        float out_h1 = h1.FeedForward(inputs);
        float out_h2 = h2.FeedForward(inputs);

        // Output
        Neuron o1 = new Neuron(weights, bias);

        float[] out_o1_Input = new float[] { out_h1, out_h2 };
        float out_o1 = o1.FeedForward(out_o1_Input);

        Debug.Log("NEURON TEST 2: " + out_o1);


    }

    

    
}
