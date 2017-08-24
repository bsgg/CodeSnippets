using System;
using UnityEngine;

/// <summary>
/// But how do those 3D points end up drawn on a 2D display? 
/// This requires a transformation from 3D to 2D space. 
/// </summary>
public class CameraTransformation : Transformation
{
    [SerializeField] private float m_FocalLength = 1f;

    // Transformation matrix from 3D to 2D space. 
    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(m_FocalLength, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, m_FocalLength, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 1f, 0f));
            return matrix;
        }
    }
}
