using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BVHTools
{
    public class TestBVH : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private new Animation animation;

       [SerializeField] private string bvhFileToLoad;

        [Header("Dat Animation Files")]
        [SerializeField] private string datFileToLoad;

        [SerializeField] private bool importFromBlender = false;

        private BVHAnimationLoader bvhLoader;


        private SkeletonMapper skeletonMapper;

        [SerializeField] private GameObject[] avatarMeshes;


        [Header("Final IK")]
        [SerializeField] private VRIK ik;
        [SerializeField] private VRIKCalibrator.Settings settings;
        [SerializeField] private Transform headTracker;
        [SerializeField] private Transform bodyTracker;
        [SerializeField] private Transform leftHandTracker;
        [SerializeField] private Transform rightHandTracker;
        [SerializeField] private Transform leftFootTracker;
        [SerializeField] private Transform rightFootTracker;

        private VRIKCalibrator.CalibrationData calibrationData = new VRIKCalibrator.CalibrationData();

        public void LoadBVH()
        {
            bvhLoader = GetComponent<BVHAnimationLoader>();

            if (bvhLoader == null)
            {
               bvhLoader = gameObject.AddComponent(typeof(BVHAnimationLoader)) as BVHAnimationLoader;
            }

            bvhLoader.targetAvatar = animator;
            bvhLoader.anim = animator.GetComponent<Animation>();

            bvhLoader.blender = importFromBlender;
            bvhLoader.standardBoneNames = true;
            bvhLoader.flexibleBoneNames = false;
            bvhLoader.respectBVHTime = true;

            bvhLoader.autoPlay = false;
            bvhLoader.autoStart = false;

            bvhLoader.RetargetBonesMap();

            bvhLoader.filename = bvhFileToLoad;

            bvhLoader.parseFile();

            bvhLoader.loadAnimation();

            bvhLoader.playAnimation();
        }

        public void LoadDatAnimation()
        {
            skeletonMapper = GetComponent<SkeletonMapper>();

            if (skeletonMapper == null)
            {
                skeletonMapper = gameObject.AddComponent(typeof(SkeletonMapper)) as SkeletonMapper;
            }

            skeletonMapper.GenerateBoneMap(animator);

            MotionData data = MotionCaptureDeserializator.Deserialize(datFileToLoad);

            if (data != null)
            {
                AnimationClip clip = MotionCaptureDeserializator.CreateAnimationClip(skeletonMapper, data, importFromBlender);

                if (clip == null)
                {
                    Debug.Log("<color=yellow>" + "Error - Clip is null" + "</color>");

                    return;
                }


                clip.name = Path.GetFileNameWithoutExtension(datFileToLoad);
                animation.AddClip(clip, clip.name);
                animation.clip = clip;

                animation.playAutomatically = true;
                animation.Play(clip.name);

            }
            else
            {
                Debug.Log("<color=yellow>" + "Unable to Create Animation Clip from " + datFileToLoad + "</color>");
            }
        }


        public void CalibrateAvatarFinalIK()
        {
            for (int i=0;i< avatarMeshes.Length; i++)
            {
                avatarMeshes[i].SetActive(false);
            }

            calibrationData = VRIKCalibrator.Calibrate(ik, settings, headTracker, bodyTracker, leftHandTracker, rightHandTracker, leftFootTracker, rightFootTracker);
            
        }
    }
}
