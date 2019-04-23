#define DEBUGMODE

using UnityEngine;

namespace IKController
{
    public class FootIKSmooth : MonoBehaviour
    {
        public bool IkActive = true;
        [Range(0f, 1f)]
        public float WeightPositionRight = 1f;
        [Range(0f, 1f)]
        public float WeightRotationRight = 0f;
        [Range(0f, 1f)]
        public float WeightPositionLeft = 1f;
        [Range(0f, 1f)]
        public float WeightRotationLeft = 0f;
#if DEBUGMODE
    //public Transform FootRight = null;
    //public Transform FootLeft = null;
#endif
        Animator anim;
        [Tooltip("Feet Offset Position")]
        public Vector3 offset;
        [Tooltip("Layer for raycast")]
        public LayerMask RayMask;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        RaycastHit hit;

        void OnAnimatorIK()
        {
            if (IkActive)
            {
                Vector3 FootPos = anim.GetIKPosition(AvatarIKGoal.RightFoot); //Get Right FeetPosition


                // Raycast from foot
                if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, RayMask))
                {
                    anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, WeightPositionRight);
                    anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, WeightRotationRight);
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + offset); //Set  IK Position for rightFoot where the hit is

#if DEBUGMODE
				//Debug.DrawLine(hit.point, Vector3.ProjectOnPlane(hit.normal, FootLeft.right), Color.blue);
				//Debug.DrawLine(FootLeft.position, FootLeft.position + FootLeft.right, Color.yellow);
#endif

                    if (WeightRotationRight > 0f) //Set Weight rotation
                    {
                        // Adjust rotation of feet
                        Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                        anim.SetIKRotation(AvatarIKGoal.RightFoot, footRotation);
                    }
                }
                else // Set to o right foot
                {
                    anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                    anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
                }

                // Left foot
                FootPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
                if (Physics.Raycast(FootPos + Vector3.up, Vector3.down, out hit, 1.2f, RayMask))
                {
                    anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, WeightPositionLeft);
                    anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, WeightRotationLeft);
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + offset);

#if DEBUGMODE
                //Debug.DrawLine(hit.point, Vector3.ProjectOnPlane(hit.normal, FootLeft.right), Color.blue);
				//Debug.DrawLine(FootLeft.position, FootLeft.position + FootLeft.right, Color.yellow);
#endif

                    if (WeightRotationLeft > 0f)
                    {

                        Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                        anim.SetIKRotation(AvatarIKGoal.LeftFoot, footRotation);
                    }
                }
                else
                {
                    anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                    anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
                }


            }
            else // No Ik
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
            }
        }
    }
}
