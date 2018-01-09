using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SnippetsCode
{
    [CustomEditor(typeof(PolarCoordinates))]
    public class PolarCoordinatesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PolarCoordinates objScript = (PolarCoordinates)target;
            if (GUILayout.Button("Generate Polar Coordinates"))
            {
                objScript.Generate();
            }

            if (GUILayout.Button("Clean"))
            {
                objScript.ResetObject();
            }

        }
    }
}
