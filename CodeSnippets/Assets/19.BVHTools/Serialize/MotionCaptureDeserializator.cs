using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BVHTools
{
    [Serializable]
    public class BoneNode
    {
        // Bone Name: This name is generic name based on humanoid standard bone
        public string boneName;

        // Offset position respect parent transform bone
        public float offsetX, offsetY, offsetZ;

        // 3 or 6 channels of info
        // 6 Channels: Only root bone should have 6 channels (x,y,z Position, x,y,z Rotation Euler in BVH format)
        // BVH Saves the rotation info in this order Y Z X
        public int channelNumber;

        // Map of keyframes. 
        // [iChannel, iFrame]
        public float[,] keyframes;
    }

    [Serializable]
    public class MotionData
    {
        public int frames = 0;

        public float frameTime;

        public BoneNode[] bones;
    }

}
namespace BVHTools
{
    public class MotionCaptureDeserializator
    {
        public static MotionData Deserialize(string fileNameToDeserialize)
        {
            if (!File.Exists(fileNameToDeserialize))
            {
                Debug.Log("<color=yellow>" + "Error: Unable to find: " + fileNameToDeserialize + "</color>");

                return null;
            }

            MotionData data = new MotionData();

            using (FileStream fileStream = File.Open(fileNameToDeserialize, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                data = (MotionData)binaryFormatter.Deserialize(fileStream);
            }

            if (data == null)
            {
                Debug.Log("<color=yellow>" + "Error tyring to deserialize" + "</color>");
            }
            else
            {
                Debug.Log("<color=cyan>" + "Deserialization completed - Frames: " + data.frames + " - Nodes: " + data.bones.Length + "</color>");
            }

            return data;
        }

        /// <summary>
        /// Create Animation clip from motion data input
        /// </summary>
        /// <param name="skeletonMapper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static AnimationClip CreateAnimationClip(SkeletonMapper skeletonMapper, MotionData inMotionData, bool blender)
        {            
            if ((inMotionData == null) || (skeletonMapper == null)) return null;

            Vector3 targetAvatarPosition = skeletonMapper.animator.transform.position;
            Quaternion targetAvatarRotation = skeletonMapper.animator.transform.rotation;

            // Reset position before creating the clip
            skeletonMapper.animator.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            skeletonMapper.animator.transform.rotation = Quaternion.identity;

            // Get root bone from skeleton mapper
            HumanoidBone rootBone = skeletonMapper.GetBoneByIndex(HumanBodyBones.Hips);
            if (rootBone == null)
            {
                Debug.Log("<color=cyan>" + "[MotionCaptureDeserializator] The avatar doesn't have root bone " + "</color>");
                return null;
            }

            AnimationClip newClip = new AnimationClip();
            newClip.name = "Clip";
            newClip.legacy = true;

            // 60 fps
            float frameRate = (1.0f / inMotionData.frameTime);

            for (int iBone = 0; iBone < inMotionData.bones.Length; iBone++)
            {
                // HumanBodyBones boneID = (HumanBodyBones)inMotionData.bones[iBone].boneIndex;

                HumanBodyBones boneID = (HumanBodyBones)skeletonMapper.GetBoneIndexByName(inMotionData.bones[iBone].boneName);

                HumanoidBone hBone = null;
                Vector3 offset = Vector3.zero;
                bool isRoot = false;
                if (boneID == HumanBodyBones.Hips)
                {
                    isRoot = true;
                    hBone = rootBone;

                    offset = new Vector3(
                        -inMotionData.bones[iBone].offsetX,
                        inMotionData.bones[iBone].offsetY,
                        inMotionData.bones[iBone].offsetZ);

                    // Correct Rotation for blender
                    if (blender)
                    {
                        offset = new Vector3(-inMotionData.bones[iBone].offsetX, inMotionData.bones[iBone].offsetZ, -inMotionData.bones[iBone].offsetY);
                    }
                }
                else
                {
                    // Get bone data from the mapper
                    //hBone = skeletonMapper.GetBoneByIndex(inMotionData.bones[iBone].boneIndex);
                    hBone = skeletonMapper.GetBoneByIndex((int)boneID);
                }

                if (hBone == null)
                {
                    Debug.Log("<color=yellow>" + "Error: There is no bone with index: " + (boneID) + "</color>");
                    continue;
                }

                if (hBone.BoneTransform == null)
                {
                    Debug.Log("<color=yellow>" + "Error: The bone: " + (boneID) + " doesn't have transform bone" + "</color>");
                    continue;
                }

                // Check how many channels this bone
                // ONLY ROOT BONE SHOULD HAVE 6 CHANNELS
                // 6 Channels = Position + Rotation
                // 3 Channels = Rotation
                int nChannels = inMotionData.bones[iBone].channelNumber;

                // Generate Frames
                Keyframe[][] keyFrames;

                if (isRoot && (nChannels == 6))
                {
                    keyFrames = new Keyframe[7][];
                    keyFrames[0] = new Keyframe[inMotionData.frames];
                    keyFrames[1] = new Keyframe[inMotionData.frames];
                    keyFrames[2] = new Keyframe[inMotionData.frames];
                    keyFrames[3] = new Keyframe[inMotionData.frames];
                    keyFrames[4] = new Keyframe[inMotionData.frames];
                    keyFrames[5] = new Keyframe[inMotionData.frames];
                    keyFrames[6] = new Keyframe[inMotionData.frames];

                }
                else
                {
                    keyFrames = new Keyframe[4][];
                    keyFrames[0] = new Keyframe[inMotionData.frames];
                    keyFrames[1] = new Keyframe[inMotionData.frames];
                    keyFrames[2] = new Keyframe[inMotionData.frames];
                    keyFrames[3] = new Keyframe[inMotionData.frames];
                }

                Quaternion oldBoneRotation = hBone.BoneTransform.rotation;

                float frameTime = 0.0f;

                for (int iFrame = 0; iFrame < inMotionData.frames; iFrame++)
                {
                    frameTime += 1f / frameRate;

                    float valRot1, valRot2,valRot3;

                    if (isRoot && (nChannels == 6))
                    {
                        // Position information (General bvh)
                        float xPosValue = -inMotionData.bones[iBone].keyframes[0, iFrame];
                        float yPosValue = inMotionData.bones[iBone].keyframes[1, iFrame];
                        float zPosValue = inMotionData.bones[iBone].keyframes[2, iFrame];

                        // Corrected if imported from blender
                        if (blender)
                        {
                            xPosValue = -inMotionData.bones[iBone].keyframes[0, iFrame];
                            yPosValue = inMotionData.bones[iBone].keyframes[2, iFrame];
                            zPosValue = -inMotionData.bones[iBone].keyframes[1, iFrame];
                        }

                        Vector3 position = new Vector3(xPosValue, yPosValue, zPosValue);
                        Vector3 transformedPosition = hBone.BoneTransform.parent.InverseTransformPoint(position + skeletonMapper.animator.transform.position + offset);

                        keyFrames[0][iFrame].time = frameTime;
                        keyFrames[1][iFrame].time = frameTime;
                        keyFrames[2][iFrame].time = frameTime;

                        keyFrames[0][iFrame].value = transformedPosition.x * skeletonMapper.animator.transform.localScale.x; 
                        keyFrames[1][iFrame].value = transformedPosition.y * skeletonMapper.animator.transform.localScale.y;
                        keyFrames[2][iFrame].value = transformedPosition.z * skeletonMapper.animator.transform.localScale.z;



                        // Get Rotation Data
                        valRot1 = inMotionData.bones[iBone].keyframes[3, iFrame];
                        valRot2 = inMotionData.bones[iBone].keyframes[4, iFrame];
                        valRot3 = inMotionData.bones[iBone].keyframes[5, iFrame];
                    }
                    else // Only rotation
                    {
                        valRot1 = inMotionData.bones[iBone].keyframes[0, iFrame];
                        valRot2 = inMotionData.bones[iBone].keyframes[1, iFrame];
                        valRot3 = inMotionData.bones[iBone].keyframes[2, iFrame];
                    }

                    // Get Rotation
                    Vector3 eulerBVH = new Vector3(WrapAngle(valRot1), WrapAngle(valRot2), WrapAngle(valRot3));
                    Quaternion rotation = FromEulerZXY(eulerBVH);

                    // Convert from General BVH to Unity (NOT BLENDER)
                    float xValue = rotation.x;
                    float yValue = -rotation.y;
                    float zValue = -rotation.z;
                    float wValue = rotation.w;

                    // Corrected from blender
                    if (blender)
                    {
                        xValue = rotation.x;
                        yValue = -rotation.z;
                        zValue = rotation.y;
                        wValue = rotation.w;
                    }

                    if (isRoot && (nChannels == 6)) //only root has position and Rotation, 
                    {
                        keyFrames[3][iFrame].time = frameTime;
                        keyFrames[4][iFrame].time = frameTime;
                        keyFrames[5][iFrame].time = frameTime;
                        keyFrames[6][iFrame].time = frameTime;

                        hBone.BoneTransform.rotation = new Quaternion(xValue, yValue, zValue, wValue);

                        keyFrames[3][iFrame].value = hBone.BoneTransform.localRotation.x;
                        keyFrames[4][iFrame].value = hBone.BoneTransform.localRotation.y;
                        keyFrames[5][iFrame].value = hBone.BoneTransform.localRotation.z;
                        keyFrames[6][iFrame].value = hBone.BoneTransform.localRotation.w;

                    }
                    else
                    {
                        keyFrames[0][iFrame].time = frameTime;
                        keyFrames[1][iFrame].time = frameTime;
                        keyFrames[2][iFrame].time = frameTime;
                        keyFrames[3][iFrame].time = frameTime;

                        keyFrames[0][iFrame].value = xValue;
                        keyFrames[1][iFrame].value = yValue;
                        keyFrames[2][iFrame].value = zValue;
                        keyFrames[3][iFrame].value = wValue;
                    }
                    
                }

                hBone.BoneTransform.rotation = oldBoneRotation;

                if (isRoot && (nChannels == 6))
                {
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localPosition.x", new AnimationCurve(keyFrames[0]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localPosition.y", new AnimationCurve(keyFrames[1]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localPosition.z", new AnimationCurve(keyFrames[2]));

                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.x", new AnimationCurve(keyFrames[3]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.y", new AnimationCurve(keyFrames[4]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.z", new AnimationCurve(keyFrames[5]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.w", new AnimationCurve(keyFrames[6]));

                }
                else
                {
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.x", new AnimationCurve(keyFrames[0]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.y", new AnimationCurve(keyFrames[1]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.z", new AnimationCurve(keyFrames[2]));
                    newClip.SetCurve(hBone.Path, typeof(Transform), "localRotation.w", new AnimationCurve(keyFrames[3]));
                }
            }

            newClip.EnsureQuaternionContinuity();

            // Reset position
            skeletonMapper.animator.transform.position = targetAvatarPosition;
            skeletonMapper.animator.transform.rotation = targetAvatarRotation;

            return newClip;
        }


        private static float WrapAngle(float a)
        {
            if (a > 180f)
            {
                return a - 360f;
            }
            if (a < -180f)
            {
                return 360f + a;
            }
            return a;
        }

        private static Quaternion FromEulerZXY(Vector3 euler)
        {
            return Quaternion.AngleAxis(euler.z, Vector3.forward) * Quaternion.AngleAxis(euler.x, Vector3.right) * Quaternion.AngleAxis(euler.y, Vector3.up);
        }

    }
}
