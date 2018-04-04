using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.PaintTool
{
    public class PaintTool : MonoBehaviour
    {
        [SerializeField] private PaintToolUI m_UI;

        [Header("Picture Holder")]
        [SerializeField] private Sprite[] m_PictureList;
        [SerializeField] private GameObject m_PictureHolder;

        





        [SerializeField] private GameObject m_BrushCursor; //The cursor that overlaps the model
        [SerializeField] private GameObject m_BrushContainer; // Container for the brushes painted

        [SerializeField] private Camera m_SceneCamera; // Scene camera for the 3D Model
        [SerializeField] private Camera m_CanvasCamera; // Canvas camera for for painting

        [SerializeField] private Sprite m_CursorPaint; // Sprite for Cursor
        [SerializeField] private RenderTexture m_CanvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
        [SerializeField] private Material m_BaseMaterial; // The material of our base texture (Were we will save the painted texture)


        [SerializeField]
        private GameObject m_3DCanvasObject;
        private Collider m_3DCanvasCollider;

        [SerializeField] private float m_BrushSize =0.2f; //The size of our brush
        private Color m_BrushColor = Color.red; //The selected color

        [SerializeField] private GameObject m_BrushPrefab;

        // Limit number of brushes
        [SerializeField] private int m_brushCounter = 0;
        [SerializeField] private int MAX_BRUSH_COUNT = 1000;

        private bool m_Saving = true;

        //[SerializeField] private GameObject m_SpritePicture;
        [SerializeField] private Texture2D m_Picture;

        [SerializeField] private ColorPicker m_ColorPicker;


        // TODO ADD DIFFERENT PICTURES

        private void InitializeCanvas()
        {
            RenderTexture.active = m_CanvasTexture;

            Texture2D tex = new Texture2D(m_CanvasTexture.width, m_CanvasTexture.height, TextureFormat.RGB24, false);

            for (int x = 0; x < m_CanvasTexture.width; x++)
            {
                for (int y = 0; y < m_CanvasTexture.height; y++)
                {
                    Color c = m_Picture.GetPixel(x, y);

                    tex.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
            }

            tex.Apply();

            RenderTexture.active = null;
            m_BaseMaterial.mainTexture = tex;
        }

        public IEnumerator SetPicture(int index)
        {
            m_PictureHolder.SetActive(false);
            if (index < m_PictureList.Length)
            {
                Sprite tempS = m_PictureHolder.GetComponent<Sprite>();
                if (tempS != null)
                {
                    tempS = m_PictureList[index];

                    yield return new WaitForEndOfFrame();
                    // Initialize canvas

                    RenderTexture.active = m_CanvasTexture;
                    Texture2D tex = new Texture2D(m_CanvasTexture.width, m_CanvasTexture.height, TextureFormat.RGB24, false);

                    for (int x = 0; x < m_CanvasTexture.width; x++)
                    {
                        for (int y = 0; y < m_CanvasTexture.height; y++)
                        {
                            Color c = tempS.texture.GetPixel(x, y);

                            tex.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                        }
                    }

                    tex.Apply();

                    RenderTexture.active = null;
                    m_BaseMaterial.mainTexture = tex;
                }
            }

            m_PictureHolder.SetActive(true);
            yield break;
        }

        private void Start()
        {
            m_Saving = true;

            m_3DCanvasCollider = m_3DCanvasObject.GetComponent<Collider>();
            StartCoroutine(SetPicture(0));


            /*m_PictureHolder.SetActive(false);

            InitializeCanvas();

            if (m_3DCanvasObject != null)
            {
                
            }*/

            m_Saving = false;
            //m_PictureHolder.SetActive(true);

        }        

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                DoAction();
            }

            UpdateBrushCursor();
        }

        private void DoAction()
        {
            if (m_Saving) return;
            Vector3 uvWorldPosition = Vector3.zero;

            if (TryGetUVOnHit(ref uvWorldPosition))
            {
                // Instantiate brush obj
                GameObject brushObj;

                brushObj = Instantiate(m_BrushPrefab); //Instance brush



                brushObj.GetComponent<SpriteRenderer>().color = new Color(m_BrushColor.r, m_BrushColor.g, m_BrushColor.b, 1.0f); //Set the brush color


                m_BrushColor.a = m_BrushSize * 2.0f;// Brushes have alpha to have a merging effect when painted over.
                brushObj.transform.parent = m_BrushContainer.transform; //Add the brush to our container to be wiped later
                brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
                brushObj.transform.localScale = Vector3.one * m_BrushSize;//The size of the brush

                m_brushCounter++;
                //If we reach the max brushes available, flatten the texture and clear the brushes
                if (m_brushCounter >= MAX_BRUSH_COUNT)
                {                    
                    m_BrushCursor.SetActive(false);
                    m_Saving = true;
                    Invoke("SaveTexture", 0.1f);
                }
            }           
        }

        // Updates at realtime the painting cursor on the mesh
        private void UpdateBrushCursor()
        {
            Vector3 uvWorldPosition = Vector3.zero;
            if (TryGetUVOnHit(ref uvWorldPosition) && !m_Saving)
            {
                m_BrushCursor.SetActive(true);
                m_BrushCursor.transform.position = uvWorldPosition + m_BrushContainer.transform.position;
            }
            else
            {
                m_BrushCursor.SetActive(false);
            }
        }

        private bool TryGetUVOnHit(ref Vector3 uvWorldPoint)
        {
            if (m_3DCanvasCollider == null) return false;

            // Get if hit on object
            RaycastHit hit;
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
            Ray cursorRay = m_SceneCamera.ScreenPointToRay(cursorPos);

            if (Physics.Raycast(cursorRay, out hit, 200.0f))
            {
                if (hit.collider == m_3DCanvasCollider)
                {
                    // Get textureCoord
                    Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
                    

                    uvWorldPoint.x = pixelUV.x - m_CanvasCamera.orthographicSize;//To center the UV on X
                    uvWorldPoint.y = pixelUV.y - m_CanvasCamera.orthographicSize;//To center the UV on Y
                    uvWorldPoint.z = 0.0f;
                    return true;
                }
            }

            return false;
        }

       

        private void SaveTexture()
        {
            m_brushCounter = 0;

            RenderTexture.active = m_CanvasTexture;

            Texture2D tex = new Texture2D(m_CanvasTexture.width, m_CanvasTexture.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, m_CanvasTexture.width, m_CanvasTexture.height), 0, 0);
            tex.Apply();

            RenderTexture.active = null;
            m_BaseMaterial.mainTexture = tex;	//Put the painted texture as the base

            // Clear all brushes
            foreach (Transform child in m_BrushContainer.transform)
            {
                Destroy(child.gameObject);
            }

            Invoke("ShowCursor", 0.1f);
           
        }

        //Show again the user cursor (To avoid saving it to the texture)
        void ShowCursor()
        {
            m_Saving = false;
        }


    }
}
