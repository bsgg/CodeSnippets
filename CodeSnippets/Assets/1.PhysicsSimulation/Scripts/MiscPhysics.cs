using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeSnippets
{
    public class MiscPhysics : MonoBehaviour
    {
        // Gravity acceleration
        public const float GRAVITY = 9.8f;

        public enum ETYPEPHYSICS { PARABOLIC, FREEFALL, HORIZONTAL };


        public static Vector2 GetVelocity(ETYPEPHYSICS tPhysics, float initVelocity, float angle, float time)
        {
            float cosAngle = Mathf.Cos(Mathf.Deg2Rad * angle);
            float senAngle = Mathf.Sin(Mathf.Deg2Rad * angle);

            Vector2 velocity = Vector2.zero;           

            if (tPhysics == ETYPEPHYSICS.FREEFALL)
            {
                velocity = new Vector2(0.0f, initVelocity - (GRAVITY * time));
            }
            else if (tPhysics == ETYPEPHYSICS.HORIZONTAL)
            {
                velocity = new Vector2(0.0f, GRAVITY * time);
            }
            else if (tPhysics == ETYPEPHYSICS.PARABOLIC)
            {
                velocity = new Vector2(initVelocity * cosAngle, (initVelocity * senAngle) - (GRAVITY * time));
            }

            return velocity;
        }

        public static Vector2 GetPosition(ETYPEPHYSICS tPhysics,float initVelocity, float angle, float time)
        {
            float cosAngle = Mathf.Cos(Mathf.Deg2Rad * angle);
            float senAngle = Mathf.Sin(Mathf.Deg2Rad * angle);

            float x = 0.0f;
            float y = 0.0f;

           
            if (tPhysics == ETYPEPHYSICS.FREEFALL)
            {
                x = 0.0f;
                y = initVelocity - (GRAVITY * time);
            }
            else if (tPhysics == ETYPEPHYSICS.HORIZONTAL)
            {
               // velocity = new Vector2(0.0f, GRAVITY * time);
            }
            else if (tPhysics == ETYPEPHYSICS.PARABOLIC)
            {
                x = initVelocity * cosAngle * time;
                y = ((initVelocity * senAngle * time) - ((GRAVITY * time * time) / 2.0f));
            }


            return new Vector2(x , y);
        }

    }
}
