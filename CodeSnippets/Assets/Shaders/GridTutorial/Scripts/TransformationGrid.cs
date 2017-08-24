using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationGrid : MonoBehaviour {

    [SerializeField]
    private Transform m_Prefab;

    [SerializeField]
    private int m_GridResolution = 10;

    private Transform[] m_Grid;

    private List<Transformation> m_Transformations;

    private Matrix4x4 m_Transformation;

    private void Awake()
    {
        m_Transformations = new List<Transformation>();

        // Initialize the transform grid in the 3 axis with a 3D point
        m_Grid = new Transform[m_GridResolution * m_GridResolution * m_GridResolution];
        for (int i= 0, z = 0; z< m_GridResolution; z++)
        {
            for (int y = 0; y < m_GridResolution; y++)
            {
                for (int x = 0; x < m_GridResolution; x++, i++)
                {
                    m_Grid[i] = CreateGridPoint(x, y, z);
                }
            }
        }
    }

    /// <summary>
    /// Gets an instance of a point in the given coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private Transform CreateGridPoint(int x, int y, int z)
    {
        Transform point = Instantiate(m_Prefab);

        point.localPosition = GetCoordinates(x, y, z);

        Color pointColor = new Color((float)x / m_GridResolution, (float)y / m_GridResolution, (float)z / m_GridResolution);

        point.GetComponent<MeshRenderer>().material.color = pointColor;

        point.name = "Point (" + x.ToString() + "x" + y.ToString() + "x" + z.ToString() + ")";

        return point;

    }

    private Vector3 GetCoordinates(int x, int y, int z)
    {
        // The position will be the midpoing of the grid cube
        return new Vector3(
            x - (m_GridResolution - 1) * 0.5f, 
            y - (m_GridResolution - 1) * 0.5f, 
            z - (m_GridResolution - 1) * 0.5f
            );
    }

    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateTransformation();

       // GetComponents<Transformation>(m_Transformations);
        for (int i = 0, z = 0; z < m_GridResolution; z++)
        {
            for (int y = 0; y < m_GridResolution; y++)
            {
                for (int x = 0; x < m_GridResolution; x++, i++)
                {
                    m_Grid[i].localPosition = TransformPoint(x, y, z);
                }
            }
        }

    }

    void UpdateTransformation()
    {
        GetComponents<Transformation>(m_Transformations);
        if (m_Transformations.Count > 0)
        {
            m_Transformation = m_Transformations[0].Matrix;
            for (int i = 1; i < m_Transformations.Count; i++)
            {
                m_Transformation = m_Transformations[i].Matrix * m_Transformation;
            }
        }
    }

    Vector3 TransformPoint(int x, int y, int z)
    {
        // Get coordinates
        Vector3 coords = GetCoordinates(x, y, z);

        // Transform the coordinates with all of the transformations
        /*for (int i=0; i< m_Transformations.Count; i++)
        {
            coords = m_Transformations[i].Apply(coords);
        }*/

        // returns new coords
        return m_Transformation.MultiplyPoint(coords); ;
    }
}
