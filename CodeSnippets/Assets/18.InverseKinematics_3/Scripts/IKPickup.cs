using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IKController
{
    public class IKPickup : MonoBehaviour
    {
        private Animator anim;

        [SerializeField] private Transform target;
        [SerializeField] private Transform hand;
        [SerializeField] private float weight = 1.0f;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("pickup");
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            weight = anim.GetFloat("IKPickup");
            if (weight > 0.95f)
            {
                target.parent = hand;
                target.localPosition = new Vector3(0.0f, 0.09f, 0.09f);
                target.localRotation = Quaternion.Euler(-18.0f, 45.0f, -42.0f);
            }
            anim.SetIKPosition(AvatarIKGoal.RightHand, target.position);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        }

    }
}
