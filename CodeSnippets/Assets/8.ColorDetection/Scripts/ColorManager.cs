using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace ColorDetection
{
    [Serializable()]
    public class ColorElement
    {
        [SerializeField]
        private string m_NameColor;
        /// <summary>
        /// Name of this color
        /// </summary>
        public string NameColor
        {
            get { return m_NameColor; }
        }

        [SerializeField]
        private Color m_DefaultColor;
        /// <summary>
        /// Default color
        /// </summary>
        public Color DefaultColor
        {
            get { return m_DefaultColor; }
        }

        [SerializeField]
        private LABColor m_LABDefaultColor;
        /// <summary>
        /// Default color
        /// </summary>
        public LABColor LABDefaultColor
        {
            get { return m_LABDefaultColor; }
        }

        [SerializeField]
        private Color[] m_ListVariantColors;
        /// <summary>
        /// List the extra colors for the default color
        /// </summary>
        public Color[] ListVariantColors
        {
            get { return m_ListVariantColors; }
        }


        [SerializeField]
        private LABColor[] m_ListLABVariantColors;
        /// <summary>
        /// List the extra colors for the default color
        /// </summary>
        public LABColor[] ListLABVariantColors
        {
            get { return m_ListLABVariantColors; }
        }

        /// <summary>
        /// Sets LAB Space color, for the default color and the variant color
        /// </summary>
        public void ConvertToLABSpace()
        {
            m_LABDefaultColor = new LABColor(m_DefaultColor);

            if (m_ListVariantColors != null)
            {
                m_ListLABVariantColors = new LABColor[m_ListVariantColors.Length];
                for (int i = 0; i < m_ListVariantColors.Length; i++)
                {
                    m_ListLABVariantColors[i] = new LABColor(m_ListVariantColors[i]);
                }
            }
        }                   
    }

    public class ColorManager : MonoBehaviour
    {
        [Tooltip("Camera Feed script")]
        [SerializeField]
        private CameraFeed                              m_CamFeed;       

        [Header("Detection Camera Area")]
        [Range(1,7)][SerializeField]
        private int                                     m_DetectionCameraArea;   

        [SerializeField]
        private ColorElement[]                          m_ListColorElements;       

        [SerializeField]
        private Color                                   m_DetectCameraColor;

        [SerializeField]
        private bool                                    m_DetectByHSV = false;

        [SerializeField]
        private string                                  m_NameBestColorCameraColor;

        [SerializeField]
        private MeshRenderer                            m_MeshDetectColor;
        private Material                                m_MatRenderDetectColor;

        [SerializeField]
        private MeshRenderer                            m_MeshMatchedColor;
        private Material                                m_MatRenderMatchColor;

        void Start()
        {
            m_MatRenderDetectColor = m_MeshDetectColor.materials[0];
            m_MatRenderMatchColor = m_MeshMatchedColor.materials[0];

            // Init camera colors
            if (m_ListColorElements != null)
            {
                for (int i=0; i< m_ListColorElements.Length; i++)
                {
                    m_ListColorElements[i].ConvertToLABSpace();
                }
            }


            StartCoroutine(DetectCameraPixelsRoutine());
        }
        Color detectedColor;
        Color matchedColor;
        IEnumerator DetectCameraPixelsRoutine()
        {
            //Waste time while webcam is not initialized
            if (!m_CamFeed.IsReady)
            {
                yield return new WaitForEndOfFrame();
            }

            //Check detection bounds are within the bounds of the image
            if (m_DetectionCameraArea > m_CamFeed.Texture.height / 2 || m_DetectionCameraArea > m_CamFeed.Texture.width / 2)
            {
                yield break;
            }

            while (true)
            {
                //Calculate the centre pixel of the texture
                int centreX = m_CamFeed.Texture.width / 2;
                int centreY = m_CamFeed.Texture.height / 2;

                //Get pixels in the specified area from the centre
                Color[] detectPixels = m_CamFeed.Texture.GetPixels(centreX, centreY, m_DetectionCameraArea, m_DetectionCameraArea);
                m_DetectCameraColor = ColorUtility.GetAverageColor(detectPixels);

                // Sets the color in the mesh
                m_MatRenderDetectColor.SetColor("_Color", m_DetectCameraColor);

                if (m_DetectByHSV)
                {
                    matchedColor = GetColorMatchHSVColor(m_DetectCameraColor, m_ListColorElements);
                }
                else
                {
                    matchedColor = GetColorMatchLABColor(m_DetectCameraColor, m_ListColorElements);
                }               
                
                m_MatRenderMatchColor.SetColor("_Color", matchedColor);

                yield return new WaitForSeconds(0.3f);
            }
        }

        /// <summary>
        /// Gets the match color between a camera color and a list of colors elements using HSV Space
        /// </summary>
        /// <param name="cameraColor"></param>
        /// <param name="listColors"></param>
        /// <returns></returns>
        public Color GetColorMatchHSVColor(Color cameraColor, ColorElement[] listColors)
        {
            Color bestColor = Color.white;

            if (listColors != null && listColors.Length > 0)
            {
                float bestDistance = float.MaxValue;

                for (int i = 0; i < listColors.Length; i++)
                {
                    int numberDistances = 1;
                    if (listColors[i].ListVariantColors != null)
                    {
                        numberDistances += listColors[i].ListVariantColors.Length;
                    }
                    float[] distances = new float[numberDistances];

                    // Get the distance to the default color
                    distances[0] = ColorUtility.HSVDistance(cameraColor, listColors[i].DefaultColor);

                    // Get the rest of distances
                    if (listColors[i].ListVariantColors != null)
                    {
                        for (int iColor = 0; iColor < listColors[i].ListVariantColors.Length; iColor++)
                        {
                            distances[(iColor + 1)] = ColorUtility.HSVDistance(cameraColor, listColors[i].ListVariantColors[iColor]);
                        }
                    }

                    // Compare all distances and gets the best
                    for (int iDistance = 0; iDistance < distances.Length; iDistance++)
                    {
                        if (distances[iDistance] < bestDistance)
                        {
                            bestDistance = distances[iDistance];
                            bestColor = listColors[i].DefaultColor;
                            m_NameBestColorCameraColor = listColors[i].NameColor;
                        }
                    }
                }
            }
            return bestColor;
        }

        /// <summary>
        /// Gets the match color between a camera color and a list of colors elements using LAB Space
        /// </summary>
        /// <param name="cameraColor"></param>
        /// <param name="listColors"></param>
        /// <returns></returns>
        public Color GetColorMatchLABColor(Color cameraColor, ColorElement[] listColors)
        {
            Color bestColor = Color.white;

            if (listColors != null && listColors.Length > 0)
            {
                LABColor labCurrentColor = new LABColor(cameraColor);
                float bestDistance = float.MaxValue;

                for (int i= 0; i< listColors.Length; i++ )
                {
                    int numberDistances = 1;
                    if (listColors[i].ListVariantColors != null)
                    {
                        numberDistances += listColors[i].ListVariantColors.Length;
                    }
                    float[] distances = new float[numberDistances];

                    // Get the distance to the default color
                    distances[0] = ColorUtility.CIE2000ColorDistance(labCurrentColor, listColors[i].LABDefaultColor);

                    // Get the rest of distances
                    if (listColors[i].ListVariantColors != null)
                    {
                        for (int iColor = 0; iColor < listColors[i].ListVariantColors.Length; iColor++)
                        {
                            distances[(iColor + 1)] = ColorUtility.CIE2000ColorDistance(labCurrentColor, listColors[i].ListLABVariantColors[iColor]);
                        }
                    }

                    // Compare all distances and gets the best
                    for (int iDistance = 0; iDistance < distances.Length; iDistance++)
                    {                       
                        if (distances[iDistance] < bestDistance)
                        {                            
                            bestDistance = distances[iDistance];
                            bestColor = listColors[i].DefaultColor;
                            m_NameBestColorCameraColor = listColors[i].NameColor;
                        }
                    }
                }
            }
            return bestColor;
        }        
    }
}
