using System;
using UnityEngine;

public class RotationTransformation : Transformation
{
    [SerializeField]
    private Vector3 m_Rotation;

    /* public override Vector3 Apply(Vector3 point)
     {
         // Get polar coordinates
         // Check http://catlikecoding.com/unity/tutorials/rendering/part-1/ for info
         //  1 radian is equal to π/180 degrees
         //  π: It is the ratio between a circle's circumference and its diameter.
         // Cosine function matches x coordinate
         // Sine function matches y coordinate      

         float radZ = m_Rotation.z * Mathf.Deg2Rad;
         float sinZ = Mathf.Sin(radZ);
         float cosZ = Mathf.Cos(radZ);

         float radX = m_Rotation.x * Mathf.Deg2Rad;
         float sinX = Mathf.Sin(radX);
         float cosX = Mathf.Cos(radX);

         float radY = m_Rotation.y * Mathf.Deg2Rad;
         float sinY = Mathf.Sin(radY);
         float cosY = Mathf.Cos(radY);

         // rotating arbitrary 2Dpoints over Z axis
         // 2D point (x,y)  into xX +yY without rotation this is equal to x(1,0) + y(0,1) which is just (x,y)
         // When rotation we can use x(cosZ, sinZ) + y(-sinZ, cosZ) == 
         // single coordinate point (xcosZ - ysinZ, xsinZ + ycosZ)   
         Vector3 rotVectorZ = new Vector3(
             point.x * cosZ - point.y * sinZ,
             point.x * sinZ + point.y * cosZ,
             point.z);


         // To get the final rotation we need a rotation matrix and we need to  rotate around Z first, then around Y, and finally around X
         Vector3 xAxis = new Vector3(
             cosY * cosZ,
             cosX * sinZ + sinX * sinY * cosZ,
             sinX * sinZ - cosX * sinY * cosZ
         );
         Vector3 yAxis = new Vector3(
             -cosY * sinZ,
             cosX * cosZ - sinX * sinY * sinZ,
             sinX * cosZ + cosX * sinY * sinZ
         );
         Vector3 zAxis = new Vector3(
             sinY,
             -sinX * cosY,
             cosX * cosY
         );

         return xAxis * point.x + yAxis * point.y + zAxis * point.z;

     }*/

    public override Matrix4x4 Matrix
    {
        get
        {
            float radX = m_Rotation.x * Mathf.Deg2Rad;
            float radY = m_Rotation.y * Mathf.Deg2Rad;
            float radZ = m_Rotation.z * Mathf.Deg2Rad;
            float sinX = Mathf.Sin(radX);
            float cosX = Mathf.Cos(radX);
            float sinY = Mathf.Sin(radY);
            float cosY = Mathf.Cos(radY);
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);

            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetColumn(0, new Vector4(
                cosY * cosZ,
                cosX * sinZ + sinX * sinY * cosZ,
                sinX * sinZ - cosX * sinY * cosZ,
                0f
            ));
            matrix.SetColumn(1, new Vector4(
                -cosY * sinZ,
                cosX * cosZ - sinX * sinY * sinZ,
                sinX * cosZ + cosX * sinY * sinZ,
                0f
            ));
            matrix.SetColumn(2, new Vector4(
                sinY,
                -sinX * cosY,
                cosX * cosY,
                0f
            ));
            matrix.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}
