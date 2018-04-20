using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        foreach(Vector3 v in vertices)
        {
            Debug.Log("Vertex: " + v);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
