using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IKController
{
    public class IKControl : MonoBehaviour
    {

        [SerializeField] private Transform m_headFollower;

        [SerializeField] private Transform m_hipFollower;

        private float m_offsetHeight;

        [SerializeField] private Transform m_rightHandFollower;

        [SerializeField] private Transform m_leftHandFollower;


        [SerializeField] private Transform m_rightFootFollower;

        [SerializeField] private Transform m_leftFootFollower;

        private Vector3 m_pelvisPosition;

        [SerializeField] private Animator m_avatar;

        [SerializeField]
        private bool m_processIK = false;

        [SerializeField] private float angleDegToFront;

        [Header("Look At")]
        [SerializeField] private Transform m_lookAtTarget;
        [SerializeField] private float m_lookAtWeight = 1.0f;

        //[Header("Feet Grounder")]
        //https://www.youtube.com/watch?v=MonxKdgxi2w
        /*[SerializeField]
        private bool m_enableFeetIK = false;
        [Range(0,2)] [SerializeField] private float heightFromGroundRaycast = 1.14f;
        [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1.5f;

        private Vector3 rightFootPosition, leftFootPosition, leftFootIKPosition, rightFootIKPosition;
        private Quaternion leftFootIKRotation, rightFootIKRotation;
        private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

        [SerializeField] private LayerMask enviromentLayer;
        [SerializeField] private float pelvisOffset;
        [Range(0, 2)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
        [Range(0, 2)] [SerializeField] private float feetToIkPositionSpeed = 0.5f;

        public string leftFootAnimVariableName = "LeftFootCurve";
        public string rightFootAnimVariableName = "RightFootCurve";
        public bool showSolverDebug = true;
        */
        private void Awake()
        {
           // m_animator = GetComponent<Animator>();
        }

        Transform headTransform;
        Transform hips;
        Transform m_spine;

        private void Start()
        {
            headTransform = m_avatar.GetBoneTransform(HumanBodyBones.Head);
            hips = m_avatar.GetBoneTransform(HumanBodyBones.Hips);
            m_spine = m_avatar.GetBoneTransform(HumanBodyBones.Spine);

            m_offsetHeight = m_headFollower.transform.transform.position.y - m_hipFollower.transform.position.y;
        }



        
        private void LateUpdate()
        {
            //if (m_headFollower != null)
            //{
                //head.position = headObj.position;
            //}

            //if (m_hipFollower != null)
            //{
                // Maximun 90 degrees
            angleDegToFront = Vector3.Angle(m_headFollower.transform.forward, transform.forward);



            m_hipFollower.transform.position = new Vector3(m_headFollower.transform.transform.position.x, m_headFollower.transform.transform.position.y - m_offsetHeight, m_headFollower.transform.transform.position.z);

            if (angleDegToFront < 90.0f)
            {
                //Transform headTransform = m_animator.GetBoneTransform(HumanBodyBones.Head);
                // Rotate head with the follower
                headTransform.rotation = m_headFollower.rotation;

                // Hip rotation
                //Transform hips = m_animator.GetBoneTransform(HumanBodyBones.Hips);
                // Hip look at head
                //m_hipFollower.forward = m_headFollower.forward;

                // hips.localRotation = m_hipFollower.localRotation;

                // Move the object whente the hip is
                // Vector3 oldPosition = gameObject.transform.localPosition;
                //gameObject.transform.localPosition = new Vector3(m_hipFollower.localPosition.x, oldPosition.y, m_hipFollower.localPosition.z);


                // Rotate head with the hips

                //Quaternion oldRotation = gameObject.transform.localRotation;
                //gameObject.transform.localRotation = Quaternion.Euler(oldRotation.eulerAngles.x, m_hipFollower.rotation.eulerAngles.y, oldRotation.eulerAngles.z);

                // offset to avoid showing mouth parts in view
                //gameObject.transform.position += gameObject.transform.forward * -0.1f;

                // Move the avatar where the headFollower is located, leaving the height where it is
                Vector3 currentPosition = m_avatar.transform.position;
                m_avatar.transform.position = new Vector3(m_headFollower.position.x, currentPosition.y, m_headFollower.position.z);

                // Spine rotate with the head
                m_spine.transform.rotation = Quaternion.LookRotation(m_headFollower.forward, transform.up);
                //m_avatar.SetLookAtWeight(m_lookAtWeight);             
                    

                   

            }

           
        }
                      

        private void OnAnimatorIK()
        {
            if (m_avatar == null) return;

            if (!m_processIK)
            {
                m_avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
                m_avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.0f);

                m_avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
                m_avatar.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);

                m_avatar.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.0f);
                m_avatar.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.0f);

                m_avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.0f);
                m_avatar.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.0f);

                return;

            }



            // Right Hand
            // 1.0f weight, it will ignore what happens on the rest and aim for this position
            m_avatar.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            m_avatar.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            m_avatar.SetIKPosition(AvatarIKGoal.RightHand, m_rightHandFollower.position);
            m_avatar.SetIKRotation(AvatarIKGoal.RightHand, m_rightHandFollower.rotation);

            // Left Hand
            m_avatar.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            m_avatar.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            m_avatar.SetIKPosition(AvatarIKGoal.LeftHand, m_leftHandFollower.position);
            m_avatar.SetIKRotation(AvatarIKGoal.LeftHand, m_leftHandFollower.rotation);

            // Right Foot
            m_avatar.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            m_avatar.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
            

            // Left Foot
            m_avatar.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            m_avatar.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

            // Look at
            m_avatar.SetLookAtPosition(m_lookAtTarget.position);
            m_avatar.SetLookAtWeight(m_lookAtWeight);


            // Move the pelvis with the hip follower
            //m_avatar.bodyPosition = m_hipFollower.position;

            // Move the pelvis with the head follower
            //if (!m_enableFeetIK)
            {
               // m_avatar.SetIKPosition(AvatarIKGoal.RightFoot, m_rightFootFollower.position);
                //m_avatar.SetIKRotation(AvatarIKGoal.RightFoot, m_rightFootFollower.rotation);

               //m_avatar.SetIKPosition(AvatarIKGoal.LeftFoot, m_leftFootFollower.position);
                //m_avatar.SetIKRotation(AvatarIKGoal.LeftFoot, m_leftFootFollower.rotation);

               
    
            }

            //if (!m_enableFeetIK)
            {
                m_avatar.bodyPosition = new Vector3(m_headFollower.position.x, m_hipFollower.transform.position.y, m_headFollower.position.z);

                Vector3 rightFootT = m_avatar.GetIKPosition(AvatarIKGoal.RightFoot);
                Quaternion rightFootQ = m_avatar.GetIKRotation(AvatarIKGoal.RightFoot);

                Vector3 rightFootH = new Vector3(0, -m_avatar.rightFeetBottomHeight, 0);

                Vector3 pos = rightFootT + rightFootQ * rightFootH;

                //Transform leftLowerLeg = m_avatar.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
                //Transform leftUpperLeg = m_avatar.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
                //leftLowerLeg.rotation = m_leftFootFollower.rotation;
                //leftUpperLeg.rotation = m_leftFootFollower.rotation;


                //Transform rigthLowerLeg = m_avatar.GetBoneTransform(HumanBodyBones.RightLowerLeg);
                //Transform rigthUpperLeg = m_avatar.GetBoneTransform(HumanBodyBones.RightUpperLeg);
                //rigthLowerLeg.rotation = m_rightFootFollower.rotation;
                //rigthUpperLeg.rotation = m_rightFootFollower.rotation;

               // m_avatar.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1.0f);
               // m_avatar.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1.0f);
                //Debug.Log(pos);
            }


            /*if (m_enableFeetIK)
            {
                // MOVE PELVIS
                MovePelvisHeight();

                // right foot ik positions and rotations
                MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);
                MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);
            }*/

        }


        #region FeetGrounding

        /*
        /// <summary>
        /// We are updating the AdjustFeetTarget method and also find the position of each foot inside our Solver Position
        /// </summary>
        private void FixedUpste()
        {
            if (!m_enableFeetIK) return;

            if (m_avatar == null) return;

            AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);

            AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

            // Find and raycast to the ground
            FeetPositionSolver(rightFootPosition, ref rightFootIKPosition, ref rightFootIKRotation); // handle the solver for the right foot
            FeetPositionSolver(leftFootPosition, ref leftFootIKPosition, ref leftFootIKRotation);

        }

        private void MoveFeetToIkPoint (AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFoodPositionY)
        {
            Vector3 targetIKPosition = m_avatar.GetIKPosition(foot);

            if (positionIKHolder != Vector3.zero)
            {
                targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
                positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

                float yVariable = Mathf.Lerp(lastFoodPositionY, positionIKHolder.y, feetToIkPositionSpeed);
                targetIKPosition.y += yVariable;

                lastFoodPositionY = yVariable;

                targetIKPosition = transform.TransformPoint(targetIKPosition);
                m_avatar.SetIKRotation(foot, rotationIKHolder);

            }

            m_avatar.SetIKPosition(foot, targetIKPosition);
        }

        /// <summary>
        /// Calculate the pelvis position
        /// </summary>
        private void MovePelvisHeight()
        {
            if ((rightFootIKPosition == Vector3.zero) || (leftFootIKPosition == Vector3.zero) || (lastPelvisPositionY == 0))
            {
                // The position of the body center of mass.
                lastPelvisPositionY = m_avatar.bodyPosition.y;
            }

            float lOffsetPosition = leftFootIKPosition.y - transform.position.y;
            float rOffsetPosition = rightFootIKPosition.y - transform.position.y;

            // Select the lowes position
            float totalOffset = (lOffsetPosition < rOffsetPosition) ? lOffsetPosition : rOffsetPosition;

            Vector3 newPelvisPosition = m_avatar.bodyPosition + Vector3.up * totalOffset;

            newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

            m_avatar.bodyPosition = newPelvisPosition;

            lastPelvisPositionY = m_avatar.bodyPosition.y;

        }

        /// <summary>
        /// Locating the Feet position via a Raycast and then Solving
        /// </summary>
        /// <param name="fromSkyPosition"></param>
        /// <param name="feetIKPositions"></param>
        /// <param name="feetIKRotations"></param>
        private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPositions, ref Quaternion feetIKRotations)
        {
            RaycastHit feetOutHit;

            if (showSolverDebug)
            {
                Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);
            }

            if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, enviromentLayer ))
            {
                // Finding our feet ik positions from the sky position
                feetIKPositions = fromSkyPosition;
                feetIKPositions.y = feetOutHit.point.y + pelvisOffset;
                feetIKRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

                return;
            }

            feetIKPositions = Vector3.zero; // Didn't work

        }

        /// <summary>
        /// Adjust the feet target
        /// </summary>
        /// <param name="feetPositions"></param>
        /// <param name="foot"></param>
        private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
        {
            feetPositions = m_avatar.GetBoneTransform(foot).position;
            feetPositions.y = transform.position.y + heightFromGroundRaycast;
        }
         */
        #endregion  FeetGrounding
    }

}
