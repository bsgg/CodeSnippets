using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplitMeshIntoTriangles : MonoBehaviour
{
    private MeshFilter m_MesFilter;
    private MeshRenderer m_MeshRenderer;
    private Mesh m_Mesh;


    [SerializeField]  private List<GameObject> m_MeshList;
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

    private void Start()
    {
        m_MeshList = new List<GameObject>();

        // Get mesh filter, mesh render
        m_MesFilter = GetComponent<MeshFilter>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_Mesh = m_MesFilter.mesh;

        Vector3[] verts = m_Mesh.vertices;
        Vector3[] normals = m_Mesh.normals;
        Vector2[] uvs = m_Mesh.uv;

        Debug.Log("SpliteMeshIntoTriangles: m_Mesh.subMeshCount " + m_Mesh.subMeshCount);
        for (int submesh = 0; submesh < m_Mesh.subMeshCount; submesh++)
        {
            int[] indices = m_Mesh.GetTriangles(submesh);

            Debug.Log("SpliteMeshIntoTriangles: indices " + indices.Length);
            for (int i = 0; i < indices.Length; i += 3)
            {
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

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.transform.localScale = transform.localScale;
                GO.AddComponent<MeshRenderer>().material = m_MeshRenderer.materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                Rigidbody rig = GO.AddComponent<Rigidbody>();
                rig.useGravity = false;

                    //.AddExplosionForce(100, transform.position, 30);

                m_MeshList.Add(GO);
            }
        }

        m_MeshRenderer.enabled = false;

        StartCoroutine(DestroyMesh());



        //StartCoroutine(SplitMesh());
    }

    IEnumerator DestroyMesh()
    {
        yield return new WaitForSeconds(3.0f);

        for (int i= 0; i< m_MeshList.Count; i++)
        {
            Rigidbody rig = m_MeshList[i].GetComponent<Rigidbody>();
            rig.mass = m_Mass;
            rig.AddExplosionForce( Random.Range(m_MinExplosionForce, m_MaxExplosionForce), transform.position, Random.Range(m_MinRadius, m_MaxRadius));
            rig.useGravity = true;

            Destroy(m_MeshList[i],5);
        }

    }

    IEnumerator SplitMesh()
    {
        MeshFilter MF = GetComponent<MeshFilter>();
        MeshRenderer MR = GetComponent<MeshRenderer>();
        Mesh M = MF.mesh;
        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {
            int[] indices = M.GetTriangles(submesh);
            for (int i = 0; i < indices.Length; i += 3)
            {
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

                GameObject GO = new GameObject("Triangle " + (i / 3));
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.AddComponent<MeshRenderer>().material = MR.materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                GO.AddComponent<Rigidbody>().AddExplosionForce(100, transform.position, 30);

                Destroy(GO, 5 + Random.Range(0.0f, 5.0f));
            }
        }
        MR.enabled = false;

        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.8f);
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
    
}
