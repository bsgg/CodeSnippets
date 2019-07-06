using UnityEngine;
using UnityEditor;
using System;

namespace BVHTools
{
    [CustomEditor(typeof(TestBVH))]
    public class TestBVHEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TestBVH test = (TestBVH)target;

            if (GUILayout.Button("LoadBVH"))
            {
                test.LoadBVH();
            }


            if (GUILayout.Button("Start Record BVH"))
            {

            }

            if (GUILayout.Button("Stop Record BVH"))
            {

            }
        }
    }
}
