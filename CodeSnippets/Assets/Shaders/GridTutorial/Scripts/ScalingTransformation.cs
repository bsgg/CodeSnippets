using UnityEngine;

public class ScalingTransformation : Transformation
{
    [SerializeField]
    private Vector3 m_Scale = new Vector3(1.0f,1.0f,1.0f);

    /*public override Vector3 Apply(Vector3 point)
    {
        // The scale affects the position of the point not the scale of the object itself

        point.x *= m_Scale.x;
        point.y *= m_Scale.y;
        point.z *= m_Scale.z;

        return point;
    }*/


    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(m_Scale.x, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, m_Scale.y, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, m_Scale.z, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}
