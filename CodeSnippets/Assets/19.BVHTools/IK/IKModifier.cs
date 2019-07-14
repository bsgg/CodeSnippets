using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IKModifier
{
    public class IKModifier : MonoBehaviour
    {
        private Animator animator;

        [SerializeField] private bool applyHipRotation = false;
        [SerializeField] private Vector3 hipRotation = new Vector3(0.0f, 0.0f, 0.0f);
        private Transform hipBone;

        [SerializeField] private bool applyOffsetPosition = false;
        [SerializeField] private float YOffset = 0.0f;

        private Transform leftFoot;

        public Vector3 leftFootPosition;

        void Awake()
        {
            animator = GetComponent<Animator>();

            if (animator == null) return;

            hipBone = animator.GetBoneTransform(HumanBodyBones.Hips);

            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);

        }

        private void LateUpdate()
        {
            if (animator == null) return;
            if (applyHipRotation)
            {
                hipBone.localRotation = Quaternion.Euler(new Vector3(hipRotation.x, hipRotation.y, hipRotation.z));
            }

            if (applyOffsetPosition)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, YOffset, transform.localPosition.z);
            }

            leftFootPosition = leftFoot.transform.localPosition;
        }
    }
}
