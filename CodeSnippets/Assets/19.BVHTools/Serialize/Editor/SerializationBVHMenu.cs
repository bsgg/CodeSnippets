using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace BVHTools
{
    public class EditorCoroutine
    {
        public static EditorCoroutine Start(IEnumerator a_routine)
        {
            EditorCoroutine coroutine = new EditorCoroutine(a_routine);
            coroutine.Start();
            return coroutine;
        }

        private readonly IEnumerator m_routine;

        EditorCoroutine(IEnumerator a_routine)
        {
            m_routine = a_routine;
        }

        private void Start()
        {
            EditorApplication.update += Update;
        }
        public void Stop()
        {
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            if (!m_routine.MoveNext())
            {
                Stop();
            }
        }
    }

    public class SerializationBVHMenu : EditorWindow
    {
        private static bool initialize = false;

        private static bool isSkeletonInitialized = false;

       // private Animator animator = null;

       // private SkeletonMapper skeletonMapper = null;

        [MenuItem("Aero Content/Serialize BVH Files")]
        private static void InitEditorWindow()
        {
            initialize = false;

            isSkeletonInitialized = false;

            GetWindow(typeof(SerializationBVHMenu));
        }


        private void OnGUI()
        {
            OnGUIMenu();
        }

        private void OnGUIMenu()
        {
            Color origColor = GUI.backgroundColor;

            /*if (!initialize)
            {
                if (animator == null)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("SELECT THE GAMEOBJECT WITH A HUMANOID ANIMATOR AVATAR", EditorStyles.boldLabel, GUILayout.Width(800));

                    EditorGUILayout.Space();

                    GUI.backgroundColor = Color.cyan;
                    Rect objectFieldRect = new Rect(5, 40, position.width - 6, 20);

                    animator = (Animator)EditorGUI.ObjectField(objectFieldRect, "DEPENDENCY:", animator, typeof(Animator), true);

                    GUI.backgroundColor = origColor;
                }
                else
                {
                    if (!isSkeletonInitialized)
                    {
                        skeletonMapper = animator.GetComponent<SkeletonMapper>();
                        if (skeletonMapper == null)
                        {
                            skeletonMapper = animator.gameObject.AddComponent<SkeletonMapper>();
                        }

                        if (skeletonMapper != null)
                        {
                            skeletonMapper.GenerateBoneMap(animator);

                            if (!skeletonMapper.Initialized)
                            {
                                isSkeletonInitialized = false;
                                skeletonMapper = null;
                            }
                            else
                            {
                                isSkeletonInitialized = true;
                            }
                        }
                    }
                    else
                    {

                        initialize = true;
                    }
                }
            }
            else*/
            {
                EditorGUILayout.Space();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("READY TO SERIALIZE BVH FILES", EditorStyles.boldLabel, GUILayout.Width(800));

                if (GUILayout.Button("SERIALIZE BVH FILES", GUILayout.ExpandWidth(true)))
                {                    
                    string filePath = EditorUtility.OpenFolderPanel("BVH Directory", "D:\\RecordSessions", "");

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        Debug.Log("FILE SELECTED " + filePath);

                        EditorCoroutine.Start(SerializeBVHFiles(filePath));
                    }
                    
                }

                EditorGUILayout.Space();
            }
        }

        private IEnumerator SerializeBVHFiles(string bvhDirectory)
        {
           /* if ((skeletonMapper == null) || (!skeletonMapper.Initialized))
            {
                bool answer = EditorUtility.DisplayDialog("Aero Tool", "Error: The Animator selected doesn't contain a SkeletonMapper", "OK");

                if (answer)
                {
                    yield break;
                }
            }*/

            DirectoryInfo dir = new DirectoryInfo(bvhDirectory);
            FileInfo[] infoFiles = dir.GetFiles("*.bvh*");

            if ((infoFiles == null) || (infoFiles.Length == 0))
            {
                bool answer = EditorUtility.DisplayDialog("Aero Tool", "Error: The Directory doesn't contain bvh files", "OK");

                if (answer)
                {
                    yield break;
                }
            }

            int index = 0;
            foreach (FileInfo f in infoFiles)
            {
                float progress = (float)index / infoFiles.Length;

                string outSerializeName = Path.Combine(bvhDirectory,Path.GetFileNameWithoutExtension(f.Name) + ".dat");

                 bool serialize = MotionCaptureSerializator.Serialize(f.FullName, outSerializeName);
                if (!serialize)
                {
                    EditorUtility.DisplayProgressBar("BVH Serializer", "Error Serializing (" + (index + 1) + "/" + infoFiles.Length +")  " + f.Name ,progress);
                }else
                {
                    EditorUtility.DisplayProgressBar("BVH Serializer", "Completed Serialized (" + (index + 1) + "/" + infoFiles.Length +")  " + f.Name, progress);
                }
                yield return new WaitForSeconds(0.2f);

                index++;
            }

            EditorUtility.ClearProgressBar();
            bool answerDialog = EditorUtility.DisplayDialog("Serialization Completed", "Production DAT files at " + bvhDirectory, "OK");

            if (answerDialog)
            {
                initialize = false;

                isSkeletonInitialized = false;
            }

        }
    }
}
