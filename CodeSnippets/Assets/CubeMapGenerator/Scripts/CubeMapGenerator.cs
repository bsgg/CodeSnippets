using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CubeMap
{
    public class CubeMapGenerator : MonoBehaviour
    {
        [Header("Camera to render the cubemap")]
        [SerializeField] private Camera m_CameraRender;

        [Header("Texture Size")]
        [SerializeField] private int m_SizeTexture = 512;

        [Header("Spots to place the camera")]
        [SerializeField] private List<Transform>    m_ListSpots;
       

        private string[]                            m_PictureName = { "Front", "Right", "Back", "Left", "Top", "Bottom" };
        private string                              m_Path;
        private bool                                m_BlockCubeMap;

        // GUI Stuff
        private Rect                                m_RectTextfield = new Rect(10, 10, 500, 50);
        private string                              m_Message = "";
        private bool                                m_ShowMessage = false;

        void Start()
        {
            m_Message = "Press C to generate Cubemaps from each Spot";
            if (m_ListSpots == null)
            {
                m_Message = "No spots to generate CubeMaps";

            }else if (m_ListSpots.Count == 0)
            {                
                m_Message = "No spots to generate CubeMaps";
            }  
            m_ShowMessage = true;
            m_SizeTexture = Mathf.Clamp(m_SizeTexture, 16, 4096);
        }

        // Update is called once per frame
        void Update()
        {
            if ((!m_BlockCubeMap) && (Input.GetKeyDown(KeyCode.C)))
            {
                if ((m_ListSpots != null) && (m_ListSpots.Count > 0))
                {                   
                    StartCoroutine(RoutineCubeMap(m_SizeTexture));
                }
            }
        }

        private IEnumerator RoutineCubeMap(int sizeTexture)
        {
            m_ShowMessage = false;
            m_BlockCubeMap = true;

            // Hide all sptos
            for (int iSpot = 0; iSpot < m_ListSpots.Count; iSpot++)
            {
                m_ListSpots[iSpot].gameObject.SetActive(false);
            }

            // Place the camera in each spoth, change it rotation and take the potho
            for (int iSpot = 0; iSpot < m_ListSpots.Count; iSpot++)
            {
                m_CameraRender.transform.position = m_ListSpots[iSpot].position;
                yield return new WaitForSeconds(0.2f);

                string id = iSpot.ToString();
                if (iSpot < 10)
                {
                    id = "0" + iSpot;
                }
                string path = Application.dataPath + "\\" + id +  "_Cubemap_";
                float yAngle = 0.0f;
                float xAngle = 0.0f;

                Quaternion rotCamera = m_CameraRender.transform.localRotation;
                string finalPath = "";
                // Take photo from front, back, left, right
                for (int i = 0; i < 4; i++)
                {
                    rotCamera = Quaternion.Euler(new Vector3(yAngle, yAngle, 0.0f));
                    m_CameraRender.transform.localRotation = rotCamera;
                    finalPath = path + m_PictureName[i] + ".png";

                    TakeSquarePhoto(m_CameraRender, finalPath, sizeTexture);
                    yAngle += 90.0f;
                    yield return new WaitForSeconds(0.5f);
                }

                // Take photo from top
                yAngle = 0.0f;
                xAngle = 90.0f;               
                rotCamera = Quaternion.Euler(new Vector3(xAngle, yAngle, 0.0f));
                m_CameraRender.transform.localRotation = rotCamera;
                finalPath = path + m_PictureName[4] + ".png";
                TakeSquarePhoto(m_CameraRender, finalPath, sizeTexture);
                yield return new WaitForSeconds(0.5f);

                // Take photo from bottom
                xAngle = -90.0f;
                rotCamera = Quaternion.Euler(new Vector3(xAngle, yAngle, 0.0f));
                m_CameraRender.transform.localRotation = rotCamera;
                finalPath = path + m_PictureName[5] + ".png";
                TakeSquarePhoto(m_CameraRender, finalPath, sizeTexture);
            }

            yield return new WaitForSeconds(0.2f);
            m_BlockCubeMap = false;
            m_ShowMessage = true;
        }

        /// <summary>
        /// Take a photo from the camera and save in a path
        /// </summary>
        /// <param name="cam">Camera used</param>
        /// <param name="path">Path</param>
        /// <param name="size">Texture Size</param>
        public static void TakeSquarePhoto(Camera cam, string path, int size)
        {
            int sqr = size;
            cam.aspect = 1.0f;
            RenderTexture tempRT = new RenderTexture(sqr, sqr, 24);
            cam.targetTexture = tempRT;
            cam.Render();

            RenderTexture.active = tempRT;
            Texture2D virtualPhoto =  new Texture2D(sqr, sqr, TextureFormat.RGB24, false);
            virtualPhoto.ReadPixels(new Rect(0, 0, sqr, sqr), 0, 0);

            RenderTexture.active = null; //can help avoid errors 
            cam.targetTexture = null;

            byte[] bytes;
            bytes = virtualPhoto.EncodeToPNG();

            System.IO.File.WriteAllBytes(path, bytes);
        }

       
        void OnGUI()
        {
            if (m_ShowMessage)
            {
                GUI.TextField(m_RectTextfield, m_Message);
            }
        }
    }
}
