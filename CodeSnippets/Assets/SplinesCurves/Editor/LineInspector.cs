using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UtilityCurves
{
    [CustomEditor(typeof(Line))]
    public class LineInspector : Editor
    {
        private void OnSceneGUI()
        {
            Line line = target as Line;

            // transformation
            Transform handleTransform = line.transform;
            Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;
            // Convert the points into world space points           
            Vector3 p0 = handleTransform.TransformPoint(line.P0);
            Vector3 p1 = handleTransform.TransformPoint(line.P1);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);

            // Transform points
            EditorGUI.BeginChangeCheck();
            p0 = Handles.DoPositionHandle(p0, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.P0 = handleTransform.InverseTransformPoint(p0);
            }
            EditorGUI.BeginChangeCheck();
            p1 = Handles.DoPositionHandle(p1, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.P1 = handleTransform.InverseTransformPoint(p1);
            }
        }

    }
}
