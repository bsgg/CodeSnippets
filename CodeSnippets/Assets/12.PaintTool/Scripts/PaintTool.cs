using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.PaintTool
{
    [System.Serializable]
    public class SpriteMask
    {
        public string ID;

        public Sprite Mask;
        public Sprite Outline;
    }


    public class PaintTool : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private PaintToolUI m_UI;

        [Header("Picture Holder")]
        [SerializeField] private GameObject m_PictureHolder;
        [SerializeField] private SpriteRenderer[] m_SpriteRendererList;
        private int m_IndexPicture = 0;

        //[SerializeField] private bool m_EnableMask = false;
        [SerializeField] private SpriteMask[] m_SpriteList;
        [SerializeField] private Material m_OutlineSpriteMaterial; // Material with the outline sprite 
        [SerializeField] private Material m_FlatColorBackgroundMaterial; // Material with a simple color background
        [SerializeField] private Material m_RenderTextureMaskedMaterial; // Material with the render texture and the mask for the final preview

        [Header("Brush")]
        // The cursor that overlaps the model
        [SerializeField] private GameObject m_BrushCursor;
        // Container for the brushes painted
        [SerializeField] private GameObject m_BrushContainer;
        [SerializeField] private GameObject m_BrushPrefab;
        // Limit number of brushes
        private int m_BrushCounter = 0;
        [SerializeField] private int m_MaxBrushCount = 1000;

        [Header("Cameras")]
        // Scene camera for the 3D Model
        [SerializeField] private Camera m_SceneCamera;
        // Canvas camera for for painting
        [SerializeField] private Camera m_CanvasCamera;

        [Header("Render Texture")]
        // Render Texture that looks at our Base Texture and the painted brushes
        [SerializeField] private RenderTexture m_CanvasTexture;
        // The material of our base texture (Were we will save the painted texture)
        [SerializeField] private Material m_BaseMaterial; 

        [SerializeField]
        private GameObject m_Preview3DCanvas;
        private Collider m_3DCanvasCollider;  

        private bool m_Block = true;

        private void Start()
        {
            m_Block = true;

            m_3DCanvasCollider = m_Preview3DCanvas.GetComponent<Collider>();
            StartCoroutine(SetPicture(0));

            m_Block = false;
        }        

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                DoAction();
            }

            //UpdateBrushCursor();
        }

        private void ResetTexture()
        {
            RenderTexture.active = m_CanvasTexture;
            Texture2D tex = new Texture2D(m_CanvasTexture.width, m_CanvasTexture.height, TextureFormat.RGB24, false);
            for (int x = 0; x < m_CanvasTexture.width; x++)
            {
                for (int y = 0; y < m_CanvasTexture.height; y++)
                {
                    //Color c = m_SpriteRendererList[m_IndexPicture].sprite.texture.GetPixel(x, y);

                    tex.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
            }

            tex.Apply();

            RenderTexture.active = null;
            m_BaseMaterial.mainTexture = tex;
        }

        public IEnumerator SetPicture(int index)
        {
            if (index >= m_SpriteList.Length) 
            {
                Debug.LogError("PaintTool: SetPicture index out of range");
                yield break;
            }

            m_IndexPicture = index;

            //Set materials
            // Set Outline material _MainTex
            m_OutlineSpriteMaterial.SetTexture("_MainTex", m_SpriteList[index].Outline.texture);

            // Set mask
            m_RenderTextureMaskedMaterial.SetTexture("_Mask", m_SpriteList[index].Mask.texture);          
            

            // Background color
            float randR = Random.Range(0.0f, 1.0f);
            float randG = Random.Range(0.0f, 1.0f);
            float randB = Random.Range(0.0f, 1.0f);
            m_FlatColorBackgroundMaterial.SetColor("_Color", new Color(randR, randG, randB, 1.0f));

            yield return new WaitForEndOfFrame();

            ResetTexture();
            yield return new WaitForEndOfFrame();

            /*for (int i=0; i < m_SpriteRendererList.Length; i++)
            {
                if (i == index)
                {
                    m_SpriteRendererList[i].gameObject.SetActive(true);

                    ResetTexture();

                    yield return new WaitForEndOfFrame();

                }
                else
                {
                    m_SpriteRendererList[i].gameObject.SetActive(false);
                }
                
            }

            yield break;*/
        }

        private void DoAction()
        {
            if (m_Block) return;
            Vector3 uvWorldPosition = Vector3.zero;

            if (TryHit(ref uvWorldPosition))
            {
                // Instantiate brush obj
                GameObject brushObj;

                //Instance brush
                brushObj = Instantiate(m_BrushPrefab);

                //Set the brush color
                brushObj.GetComponent<SpriteRenderer>().color = m_UI.BrushColor;

                brushObj.transform.parent = m_BrushContainer.transform; //Add the brush to our container to be wiped later
                brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
                brushObj.transform.localScale = Vector3.one * m_UI.BrushSize;//The size of the brush

                m_BrushCounter++;
                //If we reach the max brushes available, flatten the texture and clear the brushes
                if (m_BrushCounter >= m_MaxBrushCount)
                {                    
                    m_BrushCursor.SetActive(false);
                    m_Block = true;
                    Invoke("SaveTexture", 0.1f);
                }
            }           
        }

        // Updates at realtime the painting cursor on the mesh
        private void UpdateBrushCursor()
        {
            Vector3 uvWorldPosition = Vector3.zero;
            if (TryHit(ref uvWorldPosition) && !m_Block)
            {
                m_BrushCursor.SetActive(true);
                m_BrushCursor.transform.position = uvWorldPosition + m_BrushContainer.transform.position;
            }
            else
            {
                m_BrushCursor.SetActive(false);
            }
        }

        private bool TryHit(ref Vector3 uvWorldPoint)
        {
            if (m_3DCanvasCollider == null) return false;

            // Get if hit on object
            RaycastHit hit;
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
            Ray cursorRay = m_SceneCamera.ScreenPointToRay(cursorPos);


            //int layerMask = 1 << 10;
            //if (Physics.Raycast(cursorRay, out hit, 200.0f, layerMask))
            if (Physics.Raycast(cursorRay, out hit, 200.0f))
            {
                // Ignore UI
                int fingerID = -1;
#if !UNITY_EDITOR
     fingerID = 0; 
#endif
                if (EventSystem.current.IsPointerOverGameObject(fingerID))    // is the touch on the GUI
                {
                    return false;
                }

                
                if (hit.collider.tag == m_3DCanvasCollider.tag)
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
            m_BrushCounter = 0;

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

        public void OnErasePress()
        {
            m_Block = true;
            m_BrushCounter = 0;

            ResetTexture();

            // Clear all brushes
            foreach (Transform child in m_BrushContainer.transform)
            {
                Destroy(child.gameObject);
            }

            Invoke("ShowCursor", 0.1f);
        }

        public void OnSavePress()
        {
            StartCoroutine(SaveOnDisk());
        }

        private IEnumerator SaveOnDisk()
        {
            m_Block = true;
            m_UI.Hide();
            yield return new WaitForEndOfFrame();

            RenderTexture.active = null;
            string fullPath = Application.persistentDataPath + "\\PaintTool\\";
            System.DateTime date = System.DateTime.Now;            

            if (!System.IO.Directory.Exists(fullPath))
            {
                System.IO.Directory.CreateDirectory(fullPath);
            }

            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();


            string fileName = "PaintTool_" + date.Day+date.Month+date.Year + "_"+ date.Hour + date.Minute + date.Second + ".png";
            System.IO.File.WriteAllBytes(fullPath + fileName, bytes);

            Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + fileName);

            yield return new WaitForSeconds(3.0f);

            m_UI.Show();
            Invoke("ShowCursor", 0.1f);
        }

        //Show again the user cursor (To avoid saving it to the texture)
        private void ShowCursor()
        {
            m_Block = false;
        }

        public void OnPictureBtnPress(int id)
        {
            StartCoroutine(SetPicture(id));
        }
    }
}
