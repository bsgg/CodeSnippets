using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace PolygoTool
{
    [SerializeField]
    public class PolygonData
    {
        public List<Vector3> ListVertices;

        public PolygonData()
        {
            ListVertices = new List<Vector3>();
        }
    }

    public class PolygonToolControl : MonoBehaviour
    {
        [SerializeField]
        private Material m_PolygonMaterial;
        [SerializeField]
        private GameObject m_PointObjectPrefab;

        PolygonData m_PolygonData = new PolygonData();

        List<GameObject> m_ListObjectPoints = new List<GameObject>();

        [SerializeField]
        private float m_CollisionRadius = 1.0f;

        [SerializeField]
        private int m_MaxPoints = 50;

        [SerializeField]
        private float m_DistanceToCamera = 0.0f;

        [SerializeField]
        private bool m_LoadPolygon = true;

        [SerializeField]
        private PolygonToolUI m_ToolUI;

        [SerializeField]
        private KeyCode m_KeyMenu = KeyCode.Q;

        private string m_LogTag = "<color=#008080ff>[PolygonToolControl]</color>";

        private bool m_PolygonWasSaved = true;

        [SerializeField]
        private Camera m_Camera;


        private void Start()
        {
            if (m_LoadPolygon)
            {
                LoadPolygon();
                CreatePoints();
                HidePoints();
                m_ToolUI.Hide();
                m_ToolUI.Message = "Polygon Tool";
                m_PolygonWasSaved = true;
            }
        }

        private void Update()
        {
            // Toggle menu
            if (Input.GetKeyUp(m_KeyMenu))
            {
                m_ToolUI.Toggle();

                if (m_ToolUI.IsVisible)
                {
                    if (m_PolygonData.ListVertices.Count > 0)
                    {
                        // Only delete
                        m_ToolUI.StartButton.interactable = false;
                        m_ToolUI.SaveButton.interactable = false;
                        m_ToolUI.DeleteButton.interactable = true;
                    }
                    else
                    {
                        // Only start
                        m_ToolUI.StartButton.interactable = true;
                        m_ToolUI.SaveButton.interactable = false;
                        m_ToolUI.DeleteButton.interactable = false;

                    }

                    if (m_ListObjectPoints.Count > 0)
                    {
                        // Show points, change buttons
                        ShowPoints();
                        m_ToolUI.ShowPointsButton.gameObject.SetActive(false);
                        m_ToolUI.HidePointsButton.gameObject.SetActive(true);
                        m_ToolUI.ShowPointsButton.interactable = true;
                        m_ToolUI.HidePointsButton.interactable = true;
                    }
                    else
                    {
                        HidePoints();
                        m_ToolUI.ShowPointsButton.interactable = false;
                        m_ToolUI.HidePointsButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    // Always hide points
                    HidePoints();

                    // Polygon wasn't saved                     
                    if (!m_PolygonWasSaved)
                    {
                        DeletePolygon();
                    }
                }

            }

            if (m_Mode == EPolygonMode.CREATION)
            {
                HandlePointsMouseCreation();
            }

        }

        private void HandlePointsMouseCreation()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Input.mousePosition: " + Input.mousePosition);

                Vector3 pointMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_DistanceToCamera);
                Vector3 mouseToWorld = m_Camera.ScreenToWorldPoint(pointMouse);

                // mouseToWorld.z = m_DistanceToCamera;
                Debug.Log("Input.mousePosition: " + pointMouse + " mouseToWorld: " + mouseToWorld);


                // Debug.Log("Mouse to world: " + mouseToWorld);

                // Check if this point collides with the first point
                bool collision = false;
                if (m_PolygonData.ListVertices.Count > 0)
                {
                    float distance = Vector3.Distance(mouseToWorld, m_PolygonData.ListVertices[0]);

                    Debug.Log("distance: " + distance);

                    Debug.Log("m_PolygonData.ListVertices[0]: " + m_PolygonData.ListVertices[0] + " mouseToWorld: " + mouseToWorld);


                    // Finish polygon
                    if (distance <= m_CollisionRadius)
                    {
                        collision = true;
                        m_Mode = EPolygonMode.NODE;
                        CreatePolygon();

                        m_ToolUI.ShowPointsButton.gameObject.SetActive(false);
                        m_ToolUI.HidePointsButton.gameObject.SetActive(true);

                        m_ToolUI.StartButton.interactable = false;
                        m_ToolUI.DeleteButton.interactable = true;
                        m_ToolUI.SaveButton.interactable = true;

                        m_ToolUI.Message = "Polygon closed, ready to be saved.";
                    }
                }

                if (!collision)
                {


                    if (m_PolygonData.ListVertices.Count < m_MaxPoints)
                    {
                        m_PolygonData.ListVertices.Add(mouseToWorld);
                        GameObject obj = Instantiate(m_PointObjectPrefab, mouseToWorld, Quaternion.identity);
                        obj.name = "PolygonPoint_" + m_PolygonData.ListVertices.Count;
                        m_ListObjectPoints.Add(obj);
                    }
                    else
                    {
                        // Reach maximun points
                        Debug.unityLogger.Log(m_LogTag, " Reach maximun points: ");

                        m_ToolUI.ShowPointsButton.gameObject.SetActive(false);
                        m_ToolUI.HidePointsButton.gameObject.SetActive(true);

                        m_ToolUI.StartButton.interactable = false;
                        m_ToolUI.DeleteButton.interactable = true;
                        m_ToolUI.SaveButton.interactable = true;

                        m_ToolUI.Message = "Reached maximun points. Polygon closed, ready to be saved.";

                        m_Mode = EPolygonMode.NODE;
                        CreatePolygon();
                    }

                }
            }
        }


        private void CreatePolygon()
        {
            Triangulator tr = new Triangulator(m_PolygonData.ListVertices);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[m_PolygonData.ListVertices.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                //vertices[i] = new Vector3(m_PolygonData.ListVertices[i].x, m_PolygonData.ListVertices[i].y, m_DistanceToCamera);
                vertices[i] = m_PolygonData.ListVertices[i];
            }
            // Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();


            m_CurrentPolygon = new GameObject("Polygon");
            m_CurrentPolygon.AddComponent<MeshRenderer>().material = m_PolygonMaterial;
            m_CurrentPolygon.AddComponent<MeshFilter>().mesh = msh;
            m_CurrentPolygon.layer = gameObject.layer;
        }

        #region Buttons

        private GameObject m_CurrentPolygon = null;

        public enum EPolygonMode { NODE, CREATION };
        private EPolygonMode m_Mode = EPolygonMode.NODE;

        public void StartPolygon()
        {
            ResetPolygon();

            m_PolygonWasSaved = false;

            m_ToolUI.StartButton.interactable = false;
            StartCoroutine(WaitToStartCreate());

        }

        private IEnumerator WaitToStartCreate()
        {
            yield return new WaitForSeconds(0.3f);
            m_Mode = EPolygonMode.CREATION;
        }


        private void ResetPolygon()
        {
            // Clear mesh, vertices, objects
            if (m_CurrentPolygon != null)
            {
                Destroy(m_CurrentPolygon);
            }
            m_PolygonData = new PolygonData();

            if (m_ListObjectPoints.Count > 0)
            {
                for (int i = m_ListObjectPoints.Count - 1; i >= 0; i--)
                {
                    Destroy(m_ListObjectPoints[i]);
                }
            }
            m_ListObjectPoints = new List<GameObject>();

        }

        public void DeletePolygon()
        {
            string path = Path.Combine(Application.dataPath, "ARWindowFiles");
            string filePath = Path.Combine(path, "PolygonMask.dat");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (m_PolygonData.ListVertices != null && m_PolygonData.ListVertices.Count > 0)
            {
                m_ToolUI.Message = "Polygon and file data deleted.";

                m_ToolUI.DeleteButton.interactable = false;

                StopCoroutine(ResetMessage());
                StartCoroutine(ResetMessage());
            }
            else
            {

                m_ToolUI.Message = "No polygon data found.";
                StopCoroutine(ResetMessage());
                StartCoroutine(ResetMessage());
            }

            m_ToolUI.StartButton.interactable = true;
            m_ToolUI.SaveButton.interactable = false;
            ResetPolygon();

        }

        private IEnumerator ResetMessage()
        {
            yield return new WaitForSeconds(3.0f);
            m_ToolUI.Message = "Polygon Tool";
        }


        public void SavePolygon()
        {
            // Check if file exists
            string path = Path.Combine(Application.dataPath, "ARWindowFiles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Check if file exists
            string filePath = Path.Combine(path, "PolygonMask.dat");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Save file in path with all vertices
            string data = JsonUtility.ToJson(m_PolygonData);
            using (var stream = new StreamWriter(filePath))
            {
                stream.Write(data);
                stream.Close();
            }


            Debug.unityLogger.Log(m_LogTag, " Polygon saved in " + filePath);
            m_PolygonWasSaved = true;
            m_ToolUI.SaveButton.interactable = false;
            m_ToolUI.DeleteButton.interactable = true;
            m_ToolUI.StartButton.interactable = false;
        }

        public bool LoadPolygon()
        {
            string path = Path.Combine(Application.dataPath, "ARWindowFiles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }

            // Check if file exists
            string filePath = Path.Combine(path, "PolygonMask.dat");
            if (!File.Exists(filePath))
            {
                Debug.unityLogger.Log(m_LogTag, " There is no polygon: " + filePath);
                return false;
            }

            using (var stream = new StreamReader(filePath))
            {
                string line = stream.ReadLine();
                Debug.unityLogger.Log(m_LogTag, " Line: " + line);

                if (!string.IsNullOrEmpty(line))
                {
                    m_PolygonData = JsonUtility.FromJson<PolygonData>(line);

                }
                stream.Close();
            }

            if (m_PolygonData != null)
            {
                Debug.unityLogger.Log(m_LogTag, " Polygon loaded ");
                CreatePolygon();
            }

            return true;
        }

        private void CreatePoints()
        {
            m_ListObjectPoints = new List<GameObject>();
            if (m_PolygonData != null)
            {
                for (int i = 0; i < m_PolygonData.ListVertices.Count; i++)
                {
                    GameObject obj = Instantiate(m_PointObjectPrefab, m_PolygonData.ListVertices[i], Quaternion.identity);
                    obj.name = "PolygonPoint_" + m_PolygonData.ListVertices.Count;
                    m_ListObjectPoints.Add(obj);
                }
            }

        }

        public void ShowPoints()
        {
            if (m_ListObjectPoints != null)
            {
                for (int i = 0; i < m_ListObjectPoints.Count; i++)
                {
                    m_ListObjectPoints[i].SetActive(true);
                }

                m_ToolUI.ShowPointsButton.gameObject.SetActive(false);
                m_ToolUI.HidePointsButton.gameObject.SetActive(true);
            }
        }

        public void HidePoints()
        {
            if (m_ListObjectPoints != null)
            {
                for (int i = 0; i < m_ListObjectPoints.Count; i++)
                {
                    m_ListObjectPoints[i].SetActive(false);
                }
                m_ToolUI.ShowPointsButton.gameObject.SetActive(true);
                m_ToolUI.HidePointsButton.gameObject.SetActive(false);
            }
        }

        #endregion Buttons


    }
}
