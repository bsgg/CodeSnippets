using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class PolygonToolMaker : MonoBehaviour
    {
        [SerializeField]
        private Material m_PolygonMaterial;
        [SerializeField]
        private GameObject m_PointObjectPrefab; 
               
        List<Vector2> m_ListVertices = new List<Vector2>();




        private void Update()
        {
            if (m_ListVertices.Count < 10)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    // Instance object in mouse 3d position
                    Vector3 pointToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pointToWorld.z = 0.0f;

                    m_ListVertices.Add(pointToWorld);
                    GameObject point = Instantiate(m_PointObjectPrefab, pointToWorld, Quaternion.identity);

                    if (m_ListVertices.Count >= 10)
                    {
                        CreatePolygon();
                    }

                }
            }
        }

        private void CreateMask()
        {

        }

        private void SaveMask()
        {

        }

        private void CreatePolygon()
        {
            Triangulator tr = new Triangulator(m_ListVertices);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[m_ListVertices.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(m_ListVertices[i].x, m_ListVertices[i].y, 0.0f);
            }

            // Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

           

            // Set up game object with mesh;
            MeshRenderer mRenderer = gameObject.AddComponent<MeshRenderer>();
            mRenderer.material = m_PolygonMaterial;
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();

            filter.mesh = msh;



        }


        void Start()
        {
            // Create Vector2 vertices
            Vector2[] vertices2D = new Vector2[] {
            new Vector2(0,0),
            new Vector2(0,50),
            new Vector2(50,50),
            new Vector2(50,100),
            new Vector2(0,100),
            new Vector2(0,150),
            new Vector2(150,150),
            new Vector2(150,100),
            new Vector2(100,100),
            new Vector2(100,50),
            new Vector2(150,50),
            new Vector2(150,0),
            };

            // Use the triangulator to get indices for creating triangles
            /*Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            // Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();
            

            // Set up game object with mesh;
            MeshRenderer mRenderer = gameObject.AddComponent<MeshRenderer>();
            mRenderer.material = m_PolygonMaterial;
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            
            filter.mesh = msh;*/
        }
    }
}
