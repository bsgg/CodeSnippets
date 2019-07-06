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

            if (GUILayout.Button("Play BVH Animation"))
            {
                test.LoadBVH();
            }


            if (GUILayout.Button("Play dat Animation"))
            {
                test.LoadDatAnimation();
            }

        }
    }
}
