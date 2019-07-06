using BVHTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace BVHTools
{
    public class MotionCaptureSerializator 
    {

        /// <summary>
        /// Serialize bvh a single file
        /// </summary>
        /// <param name="inBvhFileToDeserialize">inputBVHFile</param>
        /// <param name="outSerializeFilename">Name of the file to export the final file (dat exptension)</param>
        /// <returns>Whether the serialiation was successfully</returns>
        public static bool Serialize( string inBvhFileToDeserialize, string outSerializeFilename)
        {
            if (!File.Exists(inBvhFileToDeserialize))
            {
                Debug.Log("<color=yellow>" + "Error BVH File Serialize: Unable to find BVH File " + inBvhFileToDeserialize + "</color>");

                return false;
            }

            string parseInfo = File.ReadAllText(inBvhFileToDeserialize);

            if (string.IsNullOrEmpty(parseInfo))
            {
                Debug.Log("<color=yellow>" + "Error - Serialize: Incorrect data to serialize" + "</color>");

                return false;
            }

            // Parse bvh file
            BVHParser bvhParse = new BVHParser(parseInfo);

            if (string.IsNullOrEmpty(parseInfo))
            {
                Debug.Log("<color=yellow>" + "Error - Serialize: Incorrect data to serialize" + "</color>");

                return false;
            }
            

            // Create motion data
            MotionData data = new MotionData();
            data.frames = bvhParse.frames;
            data.frameTime = bvhParse.frameTime;           

            data.bones = new BoneNode[bvhParse.boneList.Count];

            for (int iBone = 0; iBone < bvhParse.boneList.Count; iBone++)
            {
                data.bones[iBone] = new BoneNode();

                data.bones[iBone].boneName = bvhParse.boneList[iBone].name;

                data.bones[iBone].offsetX = bvhParse.boneList[iBone].offsetX;
                data.bones[iBone].offsetY = bvhParse.boneList[iBone].offsetY;
                data.bones[iBone].offsetZ = bvhParse.boneList[iBone].offsetZ;

                // Channels
                data.bones[iBone].channelNumber = bvhParse.boneList[iBone].channelNumber;
                data.bones[iBone].keyframes = new float[data.bones[iBone].channelNumber, data.frames];

                int channelIndex = 0;
                for (int iChannel = 0; iChannel < bvhParse.boneList[iBone].channels.Length; iChannel++)
                {
                    if (bvhParse.boneList[iBone].channels[iChannel].enabled)
                    {
                        // Get each keyframe float value for each channel
                        // If it's rotation, the order is different
                        // 4 = Y, 5 = Z, 3 = X
                        float[] keyframeValues = bvhParse.boneList[iBone].channels[iChannel].values;

                        // Include keyframes for each channel
                        for (int iKF = 0; iKF < keyframeValues.Length; iKF++)
                        {
                            data.bones[iBone].keyframes[channelIndex, iKF] = keyframeValues[iKF];
                        }

                        channelIndex++;
                    }

                }
            }

            // Create binary format
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            if (File.Exists(outSerializeFilename))
            {
                File.Delete(outSerializeFilename);
            }

            using (FileStream fileStream = File.Open(outSerializeFilename, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, data);
            }

            Debug.Log("<color=cyan>" + "Serialization Completed:  " + outSerializeFilename + "</color>");

            return true;
        }
    }
}
