using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshSplitter : MonoBehaviour
{
    [Header("Mesh Splitter Settings")]

    [SerializeField]
    private GameObject m_MeshToSplit;

    [SerializeField]
    private float m_Mass = 1.0f;
    [SerializeField]
    private float m_MinExplosionForce = 50.0f;
    [SerializeField]
    private float m_MaxExplosionForce = 100.0f;

    [SerializeField]
    private float m_MinRadius = 50.0f;
    [SerializeField]
    private float m_MaxRadius = 100.0f;


    [Header("Fall off Settings")]
    [SerializeField]
    private bool m_EnableEffect;
    [SerializeField] private float m_StartDelay= 1.0f;


    private List<GameObject> m_MeshList;
    private MeshRenderer m_MeshRenderer;
    private Mesh m_Mesh;

    private void Start()
    {

        

        if (m_MeshToSplit == null)
        {
            Debug.Log("Mesh to split not found");
        }
        else
        {
            m_MeshList = new List<GameObject>();

            // Get mesh filter, mesh render
            MeshFilter meshFilter = m_MeshToSplit.GetComponent<MeshFilter>();
            m_MeshRenderer = m_MeshToSplit.GetComponent<MeshRenderer>();
            m_Mesh = meshFilter.mesh;

            SplitMesh();

            if (m_EnableEffect)
            {
                StartCoroutine(FallOffEffect());
            }
        }
    }


    /// <summary>
    /// Creates triangles withing the object
    /// </summary>
    private void SplitMesh()
    {
        // Get vertices, nomrlas and uvs from the mesh
        Vector3[] verts = m_Mesh.vertices;
        Vector3[] normals = m_Mesh.normals;
        Vector2[] uvs = m_Mesh.uv;

        for (int submesh = 0; submesh < m_Mesh.subMeshCount; submesh++)
        {
            // Get the indices in each submesh
            int[] indices = m_Mesh.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 3)
            {

                // Triangulate object
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];

                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                // Add Rigidbody to each triangle
                GameObject triangle = new GameObject("Triangle " + (i / 3));
                triangle.transform.parent = transform;
                triangle.transform.position = m_MeshToSplit.transform.position;
                triangle.transform.rotation = m_MeshToSplit.transform.rotation;
                triangle.transform.localScale = m_MeshToSplit.transform.localScale;
                triangle.AddComponent<MeshRenderer>().material = m_MeshRenderer.materials[submesh];
                triangle.AddComponent<MeshFilter>().mesh = mesh;
                triangle.AddComponent<BoxCollider>();
                Rigidbody rig = triangle.AddComponent<Rigidbody>();
                rig.useGravity = false;
                m_MeshList.Add(triangle);
            }
        }

        // Disable the main mesh
        m_MeshRenderer.enabled = false;
    }


    private IEnumerator FallOffEffect()
    {
        yield return new WaitForSeconds(m_StartDelay);

        for (int i= 0; i< m_MeshList.Count; i++)
        {
            Rigidbody rig = m_MeshList[i].GetComponent<Rigidbody>();
            rig.mass = m_Mass;
            rig.AddExplosionForce( Random.Range(m_MinExplosionForce, m_MaxExplosionForce), transform.position, Random.Range(m_MinRadius, m_MaxRadius));
            rig.useGravity = true;

            Destroy(m_MeshList[i],5);
        }
    }
    
}
