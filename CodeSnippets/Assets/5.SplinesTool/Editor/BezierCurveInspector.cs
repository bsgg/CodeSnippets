using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UtilityCurves
{
    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveInspector : Editor
    {
        private BezierCurve m_Curve;
        private Transform m_HandleTransform;
        private Quaternion m_HandleRotation;

        private const int m_LineSteps = 10;
        private const float m_DirectionScale = 0.5f;


        private void OnSceneGUI()
        {
            m_Curve = target as BezierCurve;
            m_HandleTransform = m_Curve.transform;
            m_HandleRotation = Tools.pivotRotation == PivotRotation.Local ?
                m_HandleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            Vector3 p1 = ShowPoint(1);
            Vector3 p2 = ShowPoint(2);
            Vector3 p3 = ShowPoint(3);

            Handles.color = Color.grey;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);


            ShowDirections();
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);            
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = m_HandleTransform.TransformPoint(m_Curve.Points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, m_HandleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Curve, "Move Point");
                EditorUtility.SetDirty(m_Curve);
                m_Curve.Points[index] = m_HandleTransform.InverseTransformPoint(point);
            }
            return point;
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            Vector3 point = m_Curve.GetPoint(0f);
            Handles.DrawLine(point, point + m_Curve.GetDirection(0f) * m_DirectionScale);
            for (int i = 1; i <= m_LineSteps; i++)
            {
                point = m_Curve.GetPoint(i / (float) m_LineSteps);
                Handles.DrawLine(point, point + m_Curve.GetDirection(i / (float) m_LineSteps) * m_DirectionScale);
            }
        }

    }
}
