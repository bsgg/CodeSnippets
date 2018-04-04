using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.PaintTool
{
    public class PaintTool : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private PaintToolUI m_UI;

        [Header("Picture Holder")]
        [SerializeField] private GameObject m_PictureHolder;
        [SerializeField] private SpriteRenderer[] m_SpriteRendererList;
        private int m_IndexPicture = 0;

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

        private bool m_Saving = true;

        private void Start()
        {
            m_Saving = true;

            m_3DCanvasCollider = m_Preview3DCanvas.GetComponent<Collider>();
            StartCoroutine(SetPicture(1));

            m_Saving = false;
        }        

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                DoAction();
            }

            UpdateBrushCursor();
        }

        private void ResetTexture()
        {
            RenderTexture.active = m_CanvasTexture;
            Texture2D tex = new Texture2D(m_CanvasTexture.width, m_CanvasTexture.height, TextureFormat.RGB24, false);
            for (int x = 0; x < m_CanvasTexture.width; x++)
            {
                for (int y = 0; y < m_CanvasTexture.height; y++)
                {
                    Color c = m_SpriteRendererList[m_IndexPicture].sprite.texture.GetPixel(x, y);

                    tex.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f, 1.0f));
                }
            }

            tex.Apply();

            RenderTexture.active = null;
            m_BaseMaterial.mainTexture = tex;
        }

        public IEnumerator SetPicture(int index)
        {
            m_IndexPicture = index;
            for (int i=0; i < m_SpriteRendererList.Length; i++)
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

            yield break;
        }

        private void DoAction()
        {
            if (m_Saving) return;
            Vector3 uvWorldPosition = Vector3.zero;

            if (TryGetUVOnHit(ref uvWorldPosition))
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
            m_Saving = true;
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
            m_Saving = true;
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
            m_Saving = false;
        }
    }
}
