using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UtilityCurves
{
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineCurveInspector : Editor
    {
        private BezierSpline m_Spline;
        private Transform m_HandleTransform;
        private Quaternion m_HandleRotation;

        private const int m_LineSteps = 10;
        private const float m_DirectionScale = 0.5f;
        private const int m_StepsPerCurve = 10;

        private const float m_HandleSize = 0.04f;
        private const float m_PickSize = 0.06f;

        private int m_SelectedIndex = -1;


        private static Color[] m_ModeColors = 
        {
            Color.white,
            Color.yellow,
            Color.cyan
        };


        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            m_Spline = target as BezierSpline;

            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", m_Spline.Loop);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Spline, "Toggle Loop");
                EditorUtility.SetDirty(m_Spline);
                m_Spline.Loop = loop;
            }

            if (m_SelectedIndex >= 0 && m_SelectedIndex < m_Spline.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }

            if (GUILayout.Button("Add Curve"))
            {
                Undo.RecordObject(m_Spline, "Add Curve");
                m_Spline.AddCurve();
                EditorUtility.SetDirty(m_Spline);
            }


            if (GUILayout.Button("Add reference points"))
            {
                Undo.RecordObject(m_Spline, "Add Reference Points");
                m_Spline.AddReferencePoints();
                EditorUtility.SetDirty(m_Spline);
            }

            if (GUILayout.Button("Remove reference points"))
            {
                Undo.RecordObject(m_Spline, "Remove Reference Points");
                m_Spline.RemoveReferecePoints();
                EditorUtility.SetDirty(m_Spline);
            }
        }

        private void OnSceneGUI()
        {
            m_Spline = target as BezierSpline;
            m_HandleTransform = m_Spline.transform;
            m_HandleRotation = Tools.pivotRotation == PivotRotation.Local ?
                m_HandleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            for (int i = 1; i < m_Spline.ControlPointCount; i += 3)
            {
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i+1);
                Vector3 p3 = ShowPoint(i+2);

                Handles.color = Color.grey;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p1, p2);
                Handles.DrawLine(p2, p3);

                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2.0f);
                p0 = p3;
            } 
            ShowDirections();
            
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = m_HandleTransform.TransformPoint(m_Spline.GetControlPoint(index));

            // Hnadles for each point
            float size = HandleUtility.GetHandleSize(point);
            if (index == 0)
            {
                size *= 2.0f;
            }

            Handles.color = m_ModeColors[(int) m_Spline.GetControlPointMode(index)];

            if (Handles.Button(point, m_HandleRotation, size * m_HandleSize, size * m_PickSize, Handles.DotHandleCap))
            {
                m_SelectedIndex = index;
                Repaint();
            }

            if (m_SelectedIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, m_HandleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_Spline, "Move Point");
                    EditorUtility.SetDirty(m_Spline);
                    m_Spline.SetControlPoint(index,  m_HandleTransform.InverseTransformPoint(point));
                }
            }
            return point;
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            Vector3 point = m_Spline.GetPoint(0f);
            Handles.DrawLine(point, point + m_Spline.GetDirection(0f) * m_DirectionScale);

            int steps = m_StepsPerCurve * m_Spline.CurveCount;

            for (int i = 1; i <= steps; i++)
            {
                point = m_Spline.GetPoint(i / (float) steps);
                Handles.DrawLine(point, point + m_Spline.GetDirection(i / (float) steps) * m_DirectionScale);
            }
        }

        private void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", m_Spline.GetControlPoint(m_SelectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Spline, "Move Point");
                EditorUtility.SetDirty(m_Spline);
                m_Spline.SetControlPoint(m_SelectedIndex, point);
            }


            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode)
            EditorGUILayout.EnumPopup("Mode", m_Spline.GetControlPointMode(m_SelectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Spline, "Change Point Mode");
                m_Spline.SetControlPointMode(m_SelectedIndex, mode);
                EditorUtility.SetDirty(m_Spline);
            }

        }
    }
}
