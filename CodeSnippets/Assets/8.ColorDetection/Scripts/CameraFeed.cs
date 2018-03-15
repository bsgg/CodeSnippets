using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace ColorDetection
{
    public class CameraFeed : MonoBehaviour
    {

        [SerializeField]
        private RawImage                            m_RawImage;
        [SerializeField]
        private AspectRatioFitter                   m_Fitter;
        [SerializeField]
        private RectTransform                       m_ImageParent;

        // Device cameras
        private WebCamDevice m_FrontCameraDevice;
        private WebCamDevice m_BackCameraDevice;
        private WebCamDevice m_ActiveCameraDevice;

        private WebCamTexture m_FrontCameraTexture;
        private WebCamTexture m_BackCameraTexture;
        private WebCamTexture m_ActiveCameraTexture;

        //Whether WCT size data is accurate
        private bool m_IsReady = false;
        public bool IsReady { get { return m_IsReady; } }

        //Readonly properties for active texture and device
        public WebCamTexture Texture { get { return m_ActiveCameraTexture; } }

        // Image rotation
        private Vector3 m_RotationVector = new Vector3(0.0f, 0.0f, 0.0f);

        // Image uvRect
        private Rect m_DefaultRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        private Rect m_FixedRect = new Rect(0.0f, 1.0f, 1.0f, -1.0f);

        // Image Parent's scale
        private Vector3 m_DefaultScale = new Vector3(1.0f, 1.0f, 1.0f);
        private Vector3 m_FixedScale = new Vector3(-1.0f, 1.0f, 1.0f);


        void Start()
        {
            // Check for device cameras
            if (WebCamTexture.devices.Length == 0)
            {
                Debug.Log("No devices cameras found");
                return;
            }

            // Get the device's cameras and create WebCamTextures with them
            m_FrontCameraDevice = WebCamTexture.devices.Last();
            m_BackCameraDevice = WebCamTexture.devices.First();

            m_FrontCameraTexture = new WebCamTexture(m_FrontCameraDevice.name);
            m_BackCameraTexture = new WebCamTexture(m_BackCameraDevice.name);

            // Set camera filter modes for a smoother looking image
            m_FrontCameraTexture.filterMode = FilterMode.Trilinear;
            m_BackCameraTexture.filterMode = FilterMode.Trilinear;

            // Set the camera to use by default
            SetActiveCamera(m_BackCameraTexture);

            // Set the texture into raw image
            m_RawImage.texture = m_ActiveCameraTexture;
        }

        // Set the device camera to use and start it
        public void SetActiveCamera(WebCamTexture cameraToUse)
        {
            if (m_ActiveCameraTexture != null)
            {
                m_ActiveCameraTexture.Stop();
            }

            m_ActiveCameraTexture = cameraToUse;
            m_ActiveCameraDevice = WebCamTexture.devices.FirstOrDefault(device =>
                device.name == cameraToUse.deviceName);

            m_ActiveCameraTexture.Play();
        }
        
        // Switch between the device's front and back camera
        public void SwitchCamera()
        {
            SetActiveCamera(m_ActiveCameraTexture.Equals(m_FrontCameraTexture) ?
                m_BackCameraTexture : m_FrontCameraTexture);
        }

        void Update()
        {            
            if (m_ActiveCameraTexture.width > 100)
            {
                m_IsReady = true;

                // Rotate image to show correct orientation 
                m_RotationVector.z = -m_ActiveCameraTexture.videoRotationAngle;
                m_RawImage.rectTransform.localEulerAngles = m_RotationVector;

                // Set AspectRatioFitter's ratio
                float videoRatio =  m_ActiveCameraTexture.width / m_ActiveCameraTexture.height;
                m_Fitter.aspectRatio = videoRatio;

                // Unflip if vertically flipped
                m_RawImage.uvRect = m_ActiveCameraTexture.videoVerticallyMirrored ? m_FixedRect : m_DefaultRect;

            }
            else
            {
                m_IsReady = false;
            }
        }
    }
}
