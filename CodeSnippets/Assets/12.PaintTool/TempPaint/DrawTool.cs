using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.DrawTool
{
    public class DrawTool : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera;
        [SerializeField] private Shader m_DrawShader;
        private Material m_DrawMaterial;

        [Header("Brush settings")]
        [SerializeField]
        private Color m_BrushColor = Color.red;

        [Range(1, 500)]
        [SerializeField] private float m_BrushSize = 1;

        [Range(0, 1)]
        [SerializeField] private float m_BrushStrength = 0.3f;


        private RenderTexture m_RenderTextureSplat;

        [SerializeField]private GameObject m_Plane;
        [SerializeField] private Material m_PlaneMaterial;

        //Note that the MeshCollider on the GameObject must have Convex turned off
        // https://docs.unity3d.com/ScriptReference/RaycastHit-textureCoord.html
        private RaycastHit m_Hit;

        [SerializeField] private RenderTextureFormat m_RederTextureFormat = RenderTextureFormat.ARGBFloat;


       // private Texture2D tempTexture;

        // Use this for initialization
        void Start()
        {
            if (m_Camera == null)
            {
                Debug.Log("Missing Camera for Raycast");
            }

            if (m_DrawShader == null)
            {
                Debug.Log("Missing Draw shader");
            }
            else
            {
                // Create a material with the draw shader
                m_DrawMaterial = new Material(m_DrawShader);

                // Set the color for the material
                m_DrawMaterial.SetVector("_Color", m_BrushColor);
            }

            // Create render texture
            m_RenderTextureSplat = new RenderTexture(1024, 1024, 0, m_RederTextureFormat);

            m_PlaneMaterial = m_Plane.GetComponent<MeshRenderer>().material;


            /*tempTexture = new Texture2D(512, 512);
            m_PlaneMaterial.SetTexture("_MainTex", tempTexture);*/


        }

        // Update is called once per frame
        void Update()
        {
            if (m_Camera == null) return;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                
                if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out m_Hit))
                {
                    if (m_Hit.collider.tag == "Picture")
                    {

                       // Debug.Log("TextureCoord (" + m_Hit.textureCoord.x + "," + m_Hit.textureCoord.y + ")");

                       /* tempTexture.SetPixel((int)m_Hit.textureCoord.x, (int)m_Hit.textureCoord.y, m_BrushColor);
                        tempTexture.Apply();
                        m_PlaneMaterial.SetTexture("_MainTex", tempTexture);*/

                        // Debug.Log("Input.mousePosition (" + Input.mousePosition.x + "," + Input.mousePosition.y + ")");

                        // Send coordinates to the material where the input was donw
                        // Update the color
                        m_DrawMaterial.SetVector("_Color", m_BrushColor);
                        m_DrawMaterial.SetVector("_Coordinate", new Vector4(m_Hit.textureCoord.x, m_Hit.textureCoord.y));
                        m_DrawMaterial.SetFloat("_Strength", m_BrushStrength);
                        m_DrawMaterial.SetFloat("_Size", m_BrushSize);

                        // Create temporary render texture
                        RenderTexture temp = RenderTexture.GetTemporary(m_RenderTextureSplat.width, m_RenderTextureSplat.height, 0, m_RederTextureFormat);
                        Graphics.Blit(m_RenderTextureSplat, temp);
                        // use draw material only when we click on a certain position on a mesh
                        Graphics.Blit(temp, m_RenderTextureSplat, m_DrawMaterial);

                        // Release temporary render texture
                        RenderTexture.ReleaseTemporary(temp);

                       m_PlaneMaterial.SetTexture("_MainTex", m_RenderTextureSplat);
                    }
                }
            }
        }


        private void OnGUI()
        {
            // Debug
            GUI.DrawTexture(new Rect(0, 0, 512, 512), m_RenderTextureSplat, ScaleMode.ScaleToFit, false, 1);
        }
    }
}
