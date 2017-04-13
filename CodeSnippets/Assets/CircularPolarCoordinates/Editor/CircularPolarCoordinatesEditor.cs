using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MisCode
{
    [CustomEditor(typeof(CircularPolarCoordinates))]
    public class CircularPolarCoordinatesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CircularPolarCoordinates objScript = (CircularPolarCoordinates)target;
            if (GUILayout.Button("Generate Object Polar Coordinates"))
            {
                objScript.GeneratePolarCoords();
            }
            
        }
    }
}
