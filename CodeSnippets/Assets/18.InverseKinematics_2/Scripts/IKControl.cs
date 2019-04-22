using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IKController
{
    public class IKControl : MonoBehaviour
    {

        [SerializeField] private Transform m_headFollower;

        [SerializeField] private Transform m_hipFollower;

        [SerializeField] private Transform m_rightHandFollower;

        [SerializeField] private Transform m_leftHandFollower;


        [SerializeField] private Transform m_rightFootFollower;

        [SerializeField] private Transform m_leftFootFollower;

        private Animator m_animator;

        [SerializeField]
        private bool m_processIK = false;

       private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        Transform headTransform;
        Transform hips;

        private void Start()
        {
            headTransform = m_animator.GetBoneTransform(HumanBodyBones.Head);
            hips = m_animator.GetBoneTransform(HumanBodyBones.Hips);
        }

        private void Update()
        {
            
        }


        [SerializeField] private float angleDegToFront;
        private void LateUpdate()
        {
            //if (m_headFollower != null)
            //{
                //head.position = headObj.position;
            //}

            //if (m_hipFollower != null)
            {
                angleDegToFront = Vector3.Angle(m_headFollower.transform.forward, transform.forward);

                if (angleDegToFront < 90.0f)
                {
                    //Transform headTransform = m_animator.GetBoneTransform(HumanBodyBones.Head);
                    headTransform.localRotation = m_headFollower.localRotation;

                    // Hip rotation
                    //Transform hips = m_animator.GetBoneTransform(HumanBodyBones.Hips);
                    // Hip look at head
                    m_hipFollower.forward = m_headFollower.forward;

                    hips.rotation = m_hipFollower.localRotation;

                    Vector3 oldPosition = gameObject.transform.localPosition;
                    gameObject.transform.position = new Vector3(m_hipFollower.localPosition.x, oldPosition.y, m_hipFollower.localPosition.z);


                    // Rotate head with the hips

                    Quaternion oldRotation = gameObject.transform.localRotation;
                    gameObject.transform.localRotation = Quaternion.Euler(oldRotation.eulerAngles.x, m_hipFollower.rotation.eulerAngles.y, oldRotation.eulerAngles.z);

                    // offset to avoid showing mouth parts in view
                    //gameObject.transform.position += gameObject.transform.forward * -0.1f;
                }


            }
        }
                      

        private void OnAnimatorIK()
        {
            if (m_animator == null) return;

            if (!m_processIK)
            {

                m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
                m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);

                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);

                m_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.0f);
                m_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.0f);

                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.0f);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.0f);

                return;

            }

            // Right Hand
            // 1.0f weight, it will ignore what happens on the rest and aim for this position
            m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            m_animator.SetIKPosition(AvatarIKGoal.RightHand, m_rightHandFollower.localPosition);
            m_animator.SetIKRotation(AvatarIKGoal.RightHand, m_rightHandFollower.localRotation);
            
            // Left Hand
            m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            m_animator.SetIKPosition(AvatarIKGoal.LeftHand, m_leftHandFollower.localPosition);
            m_animator.SetIKRotation(AvatarIKGoal.LeftHand, m_leftHandFollower.localRotation);

            // Right Foot
            m_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            m_animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            m_animator.SetIKPosition(AvatarIKGoal.RightFoot, m_rightFootFollower.localPosition);
            m_animator.SetIKRotation(AvatarIKGoal.RightFoot, m_rightFootFollower.localRotation);

            // Left Foot
            m_animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            m_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            m_animator.SetIKPosition(AvatarIKGoal.LeftFoot, m_leftFootFollower.localPosition);
            m_animator.SetIKRotation(AvatarIKGoal.LeftFoot, m_leftFootFollower.localRotation);

        }
    }
}
