using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BVHTools
{
    public class TestBVH : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Animation animation;

       [SerializeField] private string bvhFileToLoad;

        [Header("Dat Animation Files")]
        [SerializeField] private string datFileToLoad;

        private BVHAnimationLoader bvhLoader;


        private SkeletonMapper skeletonMapper;

        public void LoadBVH()
        {
            bvhLoader = GetComponent<BVHAnimationLoader>();

            if (bvhLoader == null)
            {
               bvhLoader = gameObject.AddComponent(typeof(BVHAnimationLoader)) as BVHAnimationLoader;
            }

            bvhLoader.targetAvatar = animator;
            bvhLoader.anim = animator.GetComponent<Animation>();

            bvhLoader.blender = true;
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
                AnimationClip clip = MotionCaptureDeserializator.CreateAnimationClip(skeletonMapper, data, true);

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
    }
}
