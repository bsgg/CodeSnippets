using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.DrawTool
{
    public class DrawTool1 : MonoBehaviour
    {
        
        [SerializeField] private GameObject m_Plane;
        [SerializeField] private Material m_PlaneMaterial;

        [SerializeField]
        private Color m_BrushColor = Color.red;

        [Range(1, 100)]
        [SerializeField]
        private int m_BrushSize = 1;

        // Use this for initialization
        Texture2D tex;

        [SerializeField] private Vector2Int m_IntTextCoord;
        [SerializeField] private Vector2 m_TextCoord;


        [SerializeField] private Image m_PreviewImage;
        [SerializeField] private Texture2D m_Picture;

        void Start()
        {
            float ySize = Camera.main.orthographicSize * 2.0f;
            float xSize = ySize * Screen.width / Screen.height;
            //m_Plane.transform.localScale = new Vector3(xSize/10, xSize/10, 1);


            m_PlaneMaterial = m_Plane.GetComponent<MeshRenderer>().material;


            Texture2D tempText = m_PlaneMaterial.GetTexture("_MainTex") as Texture2D;
            // tex = new Texture2D(tempText.width, tempText.height);

            // Set the texture
            tex = new Texture2D(m_Picture.width, m_Picture.height);

            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    Color c = m_Picture.GetPixel(x, y);

                    tex.SetPixel(x,y, c);
                    tempText.SetPixel(x, y, c);

                }
            }
            tex.Apply();

            //What happens to points drawn outside the texture boundaries 
            // depends on whether the texture is set to clamp or repeat: clamp means the circle won't draw past the edges, and repeat makes the circle start over from the other side (this behavior is inherent to SetPixel).
            m_PlaneMaterial.SetTexture("_MainTex", tex);

            Rect rec = new Rect(0, 0, tex.width, tex.height);
            Sprite spr = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100);
            m_PreviewImage.sprite = spr;

            float ratio = (tex.width / tex.height);

            m_PreviewImage.preserveAspect = true;

            


            /*if (m_AspectRatioFitter != null)
            {
                m_AspectRatioFitter.aspectRatio = ratio;
            }*/
        }

        void Update()
        {
            if (tex != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Input.GetKey(KeyCode.Mouse0))
                {                    
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.Log(" HIT " + hit.collider.tag);
                        if (hit.collider.tag == "Picture")
                        {
                            

                            // Find the u,v coordinate of the Texture
                            Vector2 uv;
                            uv.x = (hit.point.x - hit.collider.bounds.min.x) / hit.collider.bounds.size.x;
                            uv.y = (hit.point.y - hit.collider.bounds.min.y) / hit.collider.bounds.size.y;


                            // m_TextCoord = hit.textureCoord;

                            // Set Color in each pixel
                            int xCoord = (int)(-uv.x * tex.width);
                            int yCoord = (int)(uv.y * tex.height);

                            //int xCoord = (int)(hit.textureCoord.x);
                            //int yCoord = (int)(hit.textureCoord.y);
                            //m_IntTextCoord = new Vector2Int(xCoord, yCoord);


                            // Paint pixel
                            Circle(tex, xCoord, yCoord, m_BrushSize, m_BrushColor);
                            // Pixel in coords
                            /*tex.SetPixel(xCoord, yCoord, m_BrushColor);

                            tex.SetPixel(xCoord - 1, yCoord - 1, m_BrushColor);  // Left Up
                            tex.SetPixel(xCoord + 1, yCoord - 1, m_BrushColor); // Right Up
                            tex.SetPixel(xCoord, yCoord - 1, m_BrushColor); // Up

                            tex.SetPixel(xCoord + 1, yCoord, m_BrushColor); // Right
                            tex.SetPixel(xCoord + 1, yCoord + 1, m_BrushColor); // Down Right
                            tex.SetPixel(xCoord, yCoord + 1, m_BrushColor); // Down   
                            tex.SetPixel(xCoord - 1, yCoord + 1, m_BrushColor); // Down Left

                            tex.SetPixel(xCoord - 1, yCoord, m_BrushColor); // Right

                            tex.Apply();*/



                            //Circle(tex, xCoord, yCoord, 1, m_BrushColor);
                            /*tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height), m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) + 1, m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height), m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) - 1, m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height), m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) + 1, m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) - 1, m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) + 1, m_BrushColor);
                            tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) - 1, m_BrushColor);
                            tex.Apply();*/
                        }
                    }
                }
                /*if (Input.GetKey(KeyCode.Mouse1))
                {
                    for (int i = 0; i < 128; i++)
                    {
                        for (int j = 0; j < 128; j++)
                        {
                            tex.SetPixel(i, j, Color.white);
                        }
                    }
                    tex.Apply();
                }*/
            }
        }

        public void Circle(Texture2D in_tex, int cx, int cy, int r, Color col)
        {
            int x, y, px, nx, py, ny, d;
            Color32[] tempArray = tex.GetPixels32();

            for (x = 0; x <= r; x++)
            {
                d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
                for (y = 0; y <= d; y++)
                {
                    px = cx + x;
                    nx = cx - x;
                    py = cy + y;
                    ny = cy - y;

                    /*tempArray[py * 1024 + px] = col;
                    tempArray[py * 1024 + nx] = col;
                    tempArray[ny * 1024 + px] = col;
                    tempArray[ny * 1024 + nx] = col;*/
                    if ((py * tex.width + px) < tempArray.Length)
                    {
                        tempArray[py * tex.width + px] = col;
                    }

                    if ((py * tex.width + nx) < tempArray.Length)
                    {
                        tempArray[py * tex.width + nx] = col;
                    }

                    if ((ny * tex.width + px) < tempArray.Length)
                    {
                        tempArray[ny * tex.width + px] = col;
                    }

                    if ((ny * tex.width + nx) < tempArray.Length)
                    {
                        tempArray[ny * tex.width + nx] = col;
                    }
                    
                }
            }
            in_tex.SetPixels32(tempArray);
            in_tex.Apply();
        }

    }
}
    
