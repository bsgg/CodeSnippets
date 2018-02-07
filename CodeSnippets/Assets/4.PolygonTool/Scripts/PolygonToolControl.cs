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
        [Header("File Data")]
        [SerializeField] private string m_DataFolder = "10.PolygonTool";
        [SerializeField] private string m_FileName = "Polygon.dat";
        [SerializeField] private PolygonToolUI m_UI;        

        [Header("Points Settings")]
        [SerializeField] private Camera m_SceneCamera;
        [SerializeField] private float m_MinDistanceToFirstPoint = 1.0f;
        [SerializeField] private int m_MaxPoints = 50;
        [SerializeField] private float m_DistanceToCamera = 0.0f;

        [SerializeField] private Material m_PolygonMaterial;
        [SerializeField] private GameObject m_PointObjectPrefab;

        public enum EPolygonMode { NONE, SAVING, NEW, DELETE };
        private EPolygonMode m_Mode = EPolygonMode.NONE;             
        private PolygonData m_PolygonData;
        private GameObject m_CurrentPolygon = null;
        private List<GameObject> m_ListObjectPoints;

        private void Start()
        {
            m_PolygonData = new PolygonData();
            m_ListObjectPoints = new List<GameObject>();

            // Loads polygon from file
            LoadPolygon(); 
        }

        /// <summary>
        /// Loads polygon from folder
        /// </summary>
        /// <returns></returns>
        public bool LoadPolygon()
        {
            // Check directory
            string path = Path.Combine(Application.dataPath, m_DataFolder);
            if (!Directory.Exists(path))
            {
                m_UI.Message = "Unable to load polygon. " + m_FileName + " not found";
                Debug.Log("<color=purple>" + "Unable to load polygon. Folder doesn't exists " + "</color>");

                Directory.CreateDirectory(path);

                return false;
            }

            // Check if file exists
            string filePath = Path.Combine(path, m_FileName);
            if (!File.Exists(filePath))
            {
                Debug.Log("<color=purple>" + "Unable to load polygon. " + filePath + " not found" +  "</color>");
                m_UI.Message = "Unable to load polygon. " + m_FileName + " not found";
                return false;
            }

            // Read file
            using (var stream = new StreamReader(filePath))
            {
                string line = stream.ReadLine();

                if (!string.IsNullOrEmpty(line))
                {
                    m_PolygonData = JsonUtility.FromJson<PolygonData>(line);

                }
                stream.Close();
            }

            if (m_PolygonData != null)
            {
                m_UI.Message = "Loading polygon";
                Debug.Log("<color=purple>" + "Loading polygon"  + "</color>");

                InstancePolygon();
                InstancePoints();
            }

            return true;
        }

        private void Update()
        {
            if (m_Mode == EPolygonMode.NEW)
            {
                UpdatePointsOnScreen();
            }
        }

        private void UpdatePointsOnScreen()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 pointMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_DistanceToCamera);
                Vector3 mouseToWorld = m_SceneCamera.ScreenToWorldPoint(pointMouse);

                //Debug.Log("Input.mousePosition: " + pointMouse + " mouseToWorld: " + mouseToWorld);

                // Check if this point collides with the first point
                bool collision = false;
                if (m_PolygonData.ListVertices.Count > 1)
                {
                    float distance = Vector3.Distance(mouseToWorld, m_PolygonData.ListVertices[0]);
                    Debug.Log("<color=purple>" + "Distance to first point: " + distance + " MinDistance: " + m_MinDistanceToFirstPoint  +"</color>");

                    // Finish polygon
                    if (distance <= m_MinDistanceToFirstPoint)
                    {
                        collision = true;
                        m_Mode = EPolygonMode.SAVING;

                        m_UI.Message = "Reach first point. Saving polygon.";
                        // Create polygon
                        InstancePolygon();
                        SaveDataPolygon();

                        m_Mode = EPolygonMode.NONE;
                    }
                }

                if (!collision)
                {
                    if (m_PolygonData.ListVertices.Count < m_MaxPoints)
                    {
                        m_PolygonData.ListVertices.Add(mouseToWorld);

                        // Instance an object
                        // Instantiating objects
                        GameObject obj = Instantiate(m_PointObjectPrefab, mouseToWorld, Quaternion.identity);
                        obj.name = "PolygonPoint_" + (m_PolygonData.ListVertices.Count-1);
                        m_ListObjectPoints.Add(obj);
                    }
                    else
                    {
                        // Reach maximun points
                        m_UI.Message = "Reach maximun points. Saving polygon.";

                        // Create polygon
                        m_Mode = EPolygonMode.SAVING;

                        InstancePolygon();
                        SaveDataPolygon();

                        m_Mode = EPolygonMode.NONE;
                    }
                }
            }
        }

        /// <summary>
        /// Creates polygon
        /// </summary>
        private void InstancePolygon()
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

        private void InstancePoints()
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

        #region Buttons

        public void NewPolygon()
        {
            ResetPolygon();
            m_UI.NewButton.interactable = false;
            m_Mode = EPolygonMode.NONE;

            m_UI.Message = "Click anywhere to place a point";

            StartCoroutine(NewPolygonDelayed());
        }

        private IEnumerator NewPolygonDelayed()
        {
            yield return new WaitForSeconds(0.3f);
            m_Mode = EPolygonMode.NEW;
        }

        /// <summary>
        /// Reset current data
        /// </summary>
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
            m_Mode = EPolygonMode.DELETE;
            string path = Path.Combine(Application.dataPath, m_DataFolder);
            string filePath = Path.Combine(path, m_FileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            ResetPolygon();
            m_UI.Message = "Polygon data deleted.";

            m_Mode = EPolygonMode.NONE;
        }

        private void SaveDataPolygon()
        {
            // Check if file exists
            string path = Path.Combine(Application.dataPath, m_DataFolder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Check if file exists
            string filePath = Path.Combine(path, m_FileName);
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
        }
        

        #endregion Buttons

    }
}
