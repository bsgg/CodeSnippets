using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeSnippets
{
    public class MiscPhysics : MonoBehaviour
    {
        // Gravity acceleration
       public const float GRAVITY = 9.8f;


        public static Vector2 GetVelocity(float initVelocity, float angle, float time)
        {
            float cosAngle = Mathf.Cos(Mathf.Deg2Rad * angle);
            float senAngle = Mathf.Sin(Mathf.Deg2Rad * angle);
            Vector2 velocity = new Vector2(initVelocity * cosAngle, (initVelocity * senAngle) - (GRAVITY * time));
            return velocity;
        }

        public static Vector2 GetPosition(float initVelocity, float angle, float time)
        {
            float cosAngle = Mathf.Cos(Mathf.Deg2Rad * angle);
            float senAngle = Mathf.Sin(Mathf.Deg2Rad * angle);

            float x = initVelocity * cosAngle * time;
            float y = ((initVelocity * senAngle * time) - ((GRAVITY * time * time)/2.0f));

            return new Vector2(x , y);
        }

    }
}
