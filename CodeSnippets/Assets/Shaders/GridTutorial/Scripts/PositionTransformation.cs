using System;
using UnityEngine;

public class PositionTransformation : Transformation
{
    [SerializeField] private Vector3 m_Position;

    /*public override Vector3 Apply(Vector3 point)
    {        
        return point + m_Position;
    }*/


    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(1f, 0f, 0f, m_Position.x));
            matrix.SetRow(1, new Vector4(0f, 1f, 0f, m_Position.y));
            matrix.SetRow(2, new Vector4(0f, 0f, 1f, m_Position.z));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}
