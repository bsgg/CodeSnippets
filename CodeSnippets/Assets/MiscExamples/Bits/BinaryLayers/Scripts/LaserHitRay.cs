using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitwiseExamples
{
    public class LaserHitRay : MonoBehaviour
    {
        private int layerMask = 1 << 14; // Layer 14, to get it we need to shift 1 to the left 12 positions

        private void Start()
        {
            // Prevent to use the layer 13 , it will hit anything but anything on the layer 14
            //layerMask = ~layerMask;

            // It will hit on layer 13 and 14 only (combine layers)
            layerMask = (1 << 13) | (1 << 14);
        }
        void Update()
        {           

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                //Debug.Log("<color=cyan>" + "Hit on " + hit.collider.name + "</color>");

                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.cyan);
            }else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);

                Debug.Log("<color=cyan>" + "Miss hit "  + "</color>");
            }
        }
    }
}
