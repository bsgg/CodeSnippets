using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TextureBlendTest : MonoBehaviour {


    Material mat;
    bool showDecal = false;
	// Use this for initialization
	void Start ()
    {
        mat = this.GetComponent<Renderer>().sharedMaterial;
		
	}

    private void OnMouseDown()
    {
        showDecal = !showDecal;
        if (showDecal)
        {
            mat.SetFloat("_ShowDecal", 1);
        }else
        {
            mat.SetFloat("_ShowDecal", 0);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
