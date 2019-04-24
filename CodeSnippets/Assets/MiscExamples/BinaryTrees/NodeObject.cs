using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro Text;

    [SerializeField] private Node node;

    public void Initialize(Node n)
    {
        node = n;
        Text.text = node.key.ToString();
    }
}
