using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    [SerializeField] public int key;

    [SerializeField] public Node left;

    [SerializeField] public Node right;

    public Node(int k)
    {
        key = k;
    }

    public void AddNode(Node n)
    {
        // Small value than the current
        if (n.key < key)
        {
            // Stop when left is null
            if (left == null)
            {
                left = n;
            }
            else
            {
                // Recursive
                left.AddNode(n);
            }
        }
        else  if (n.key > key)
        {
            // Stop when right is null
            if (right == null)
            {
                right = n;
            }
            else
            {
                // Recursive
                right.AddNode(n);
            }
        }

        // Equal nothing happens
    }

    public void Visit()
    {
        if (left != null)
        {
            left.Visit();
        }

        Debug.Log("<color=cyan>" + key + "</color>");

        if (right != null)
        {
            right.Visit();
        }
    }

    public Node Search(int value)
    {
        if (key == value )
        {
           // Debug.Log("<color=cyan>" + "Found Value: " + key + "</color>");
            return this;

        }else if ((value < key) && (left != null))
        {
            return left.Search(value);
        }
        else if ((value > key) && (right != null))
        {
            return right.Search(value);
        }

        return null;
    }
    
}

[System.Serializable]
public class BinaryTree
{
    [SerializeField] private Node root = null;

    public void AddNode(Node n)
    {
        if (root == null)
        {
            root = n;
        }
        else
        {
            root.AddNode(n);
        }
    }

    public void Traverse()
    {
        root.Visit();
    }

    public Node Search(int value)
    {
        Node seachedNode = root.Search(value);
        return seachedNode;        

    }
}


public class BinaryTreeTest : MonoBehaviour
{
    [SerializeField] private BinaryTree m_tree;

    [SerializeField] private NodeObject m_nodePrefab;

   private void Start()
    {
        m_tree = new BinaryTree();

        Node n = new Node(5);

        NodeObject rootObj = Instantiate(m_nodePrefab);
        rootObj.Initialize(n);
        m_tree.AddNode(n);

        for (int i=0; i< 50; i++)
        {
            int randomValue = Random.Range(0, 100);
            Node randNode = new Node(randomValue);            

            NodeObject newNode = Instantiate(m_nodePrefab);
            newNode.Initialize(randNode);

            m_tree.AddNode(randNode);

        }

        Node fixedNode = new Node(20);

        NodeObject newNode3 = Instantiate(m_nodePrefab);
        newNode3.Initialize(fixedNode);

       // m_tree.AddNode(fixedNode);

        Debug.Log("<color=cyan>" + "Traverse Tree" + "</color>");
        m_tree.Traverse();

        Node searchNode = m_tree.Search(20);

        if (searchNode != null)
        {
            Debug.Log("<color=cyan>" + "Node Found " + searchNode.key + "</color>");
        }else
        {
            Debug.Log("<color=cyan>" + "Node Not found" + "</color>");
        }
    }


}
