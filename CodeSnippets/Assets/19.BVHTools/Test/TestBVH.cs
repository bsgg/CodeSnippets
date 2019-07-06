using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BVHTools
{
    public class TestBVH : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string bvhFileToLoad;

        private BVHAnimationLoader bvhLoader;

        public void LoadBVH()
        {
            bvhLoader = GetComponent<BVHAnimationLoader>();

            if (bvhLoader == null)
            {
               bvhLoader = gameObject.AddComponent(typeof(BVHAnimationLoader)) as BVHAnimationLoader;
            }

            bvhLoader.targetAvatar = animator;
            bvhLoader.anim = animator.GetComponent<Animation>();

            bvhLoader.blender = false;
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
    }
}
