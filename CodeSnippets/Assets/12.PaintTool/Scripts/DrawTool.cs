using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTool : MonoBehaviour
{

    [SerializeField] private Camera m_Camera;
    [SerializeField] private Shader m_DrawShader;


    [SerializeField] private RenderTexture m_SplatMap;
    [SerializeField] private Material m_SnowMaterial;
    [SerializeField] private Material m_DrawMaterial;


	// Use this for initialization
	void Start ()
    {
        m_DrawMaterial = new Material(m_DrawShader);
        m_DrawMaterial.SetVector("_Color", Color.red);


        m_SnowMaterial = GetComponent<MeshRenderer>().material;

        m_SplatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);

        //m_SnowMaterial.SetTexture("_splat", m_SplatMap);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
