using UnityEngine;
using System.Collections;
using System;

namespace MiscCode.Utility
{
    public class MathUtility
    {
        /// <summary>
        /// Method to shuffle arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void Shuffle<T>(ref T[] array)
        {
            UnityEngine.Random random = new UnityEngine.Random();
            for (int i = 0; i < array.Length; i++)
            {
                int idx = UnityEngine.Random.Range(i, array.Length);
                //swap elements
                T tmp = array[i];
                array[i] = array[idx];
                array[idx] = tmp;
            }
        }

        /// <summary>
        /// Bezier formula 1 point of control
        /// </summary>
        public static Vector3 Bezier(Vector3 Start, Vector3 Control, Vector3 End, float time)
        {
            return (((1 - time) * (1 - time)) * Start) + (2 * time * (1 - time) * Control) + ((time * time) * End);
        }

        /// <summary>
        /// Bezier formula 2 points of control
        /// </summary>
        public static Vector3 Bezier(Vector3 Start, Vector3 StartControl, Vector3 EndControl, Vector3 End, float time)
        {
            return (((-Start + 3 * (StartControl - EndControl) + End) * time + (3 * (Start + EndControl) - 6 * StartControl)) * time + 3 * (StartControl - Start)) * time + Start;
        }

        /// <summary>
        /// Parable movement between two points, a predefined height and during an specific time
        /// </summary>
        /// <param name="start">Start Point</param>
        /// <param name="end">End Point</param>
        /// <param name="height">Max height</param>
        /// <param name="time">Time to travel</param>
        /// <returns></returns>
        public static Vector3 Parable(Vector3 start, Vector3 end, float height, float time)
        {
            float parabolicT = time * 2 - 1;
            if (Mathf.Abs(start.y - end.y) < 0.1f)
            {
                //start and end are roughly level, pretend they are - simpler solution with less steps
                Vector3 travelDirection = end - start;
                Vector3 result = start + time * travelDirection;
                result.y += (-parabolicT * parabolicT + 1) * height;
                return result;
            }
            else
            {
                //start and end are not level, gets more complicated
                Vector3 travelDirection = end - start;
                Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
                Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
                Vector3 up = Vector3.Cross(right, travelDirection);
                if (end.y > start.y) up = -up;
                Vector3 result = start + time * travelDirection;
                result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
                return result;
            }
        }

        /// <summary>
        /// Convert a value inside a range into another range
        /// (i.e. converting value from 0 - 60 value to 0 - 360 value)
        /// </summary>
        /// <param name="oldValue">Old value</param>
        /// <param name="oldMin">Min old range</param>
        /// <param name="oldMax">Max old range</param>
        /// <param name="newMin">Min new range</param>
        /// <param name="newMax">Max new range</param>
        /// <returns></returns>
        public static float GetConvertedRange(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
        }

        public static void CalclulateTriangleAngles(Vector3 pointA, Vector3 pointB, Vector3 pointC, out float angleA, out float angleB, out float angleC)
        {
            angleA = 0.0f;
            angleB = 0.0f;
            angleC = 0.0f;

            Vector2 AB = new Vector2(pointB.x - pointA.x, pointB.y - pointA.y);
            Vector2 BC = new Vector2(pointC.x - pointB.x, pointC.y - pointB.y);
            Vector2 AC = new Vector2(pointC.x - pointA.x, pointC.y - pointA.y);
            float distA = BC.magnitude;
            // Oposite side of angle B
            float distB = AC.magnitude;
            // Oposite side of angle C
            float distC = AB.magnitude;

            string resultAngle = "A";
            if ((distB > distA) && (distB > distC)) resultAngle = "B";
            else if ((distC > distA) && (distC > distB)) resultAngle = "C";

            if (resultAngle == "A")
            {
                angleA = GetAngle3(distA, distB, distC);

                Debug.Log("distA: " + distA);
                Debug.Log("distB: " + distB);
                Debug.Log("distC: " + distC);
                Debug.Log("angleA: " + angleA);

                // Get the other two angles
                angleB = GetAngleSinRule(distA, distB, angleA);
                angleC = GetAngleSumRule(angleA, angleB);
            }

            else if (resultAngle == "B")
            {
                angleB = GetAngle3(distA, distB, distC); 

                // Get the other two angles
                angleA = GetAngleSinRule(distB, distA, angleB);
                angleC = GetAngleSumRule(angleA, angleB);
            }
            else if (resultAngle == "C")
            {
                angleC = GetAngle3(distA, distB, distC);
                angleA = GetAngleSinRule(distC, distA, angleC);
                angleB = GetAngleSumRule(angleC, angleA);
            }
        }


        /// <summary>
        /// Get the angle from 3 sides of a triangle
        /// </summary>
        /// <param name="distA">Distance from BC</param>
        /// <param name="distB">Distance from AC</param>
        /// <param name="distC">Distance from AB</param>
        /// <returns>The angle from 3 distances</returns>
        public static float GetAngle3(float a, float b, float c)
        {
            float acos = ((b * b) + (c * c) - (a * a)) / (2 * (b * c));
            float angleRadA = Mathf.Acos(acos);
            float angleA = angleRadA * Mathf.Rad2Deg;
            return Mathf.Round(angleA);
        }

        public static float GetAngleSinRule(float distA, float distB, float AngleA)
        {
            // Sine rule   (distA / sinA) = (distB / sinB)
            float auxSinAngleA = Mathf.Sin(Mathf.Deg2Rad * AngleA);
            float sinB = (distB * auxSinAngleA) / distA;
            float angleB = Mathf.Asin(sinB) * Mathf.Rad2Deg;
            return Mathf.Round(angleB);
        }

        public static float GetAngleSumRule(float angleA, float angleB)
        {
            return (180 - (angleA + angleB));
        }

        public static float GetTimeLeftPercent(DateTime startDate, DateTime endDate, DateTime currentDate)
        {
            TimeSpan totalTime = endDate.Subtract(startDate);
            TimeSpan timeLeft = endDate.Subtract(currentDate);

            return (timeLeft.Ticks / totalTime.Ticks);
        }

        /// <summary>
        /// Polar coordinates based on an angle, a raidus and a center of a circle.
        /// </summary>
        /// <param name="angle">Angle for the point</param>
        /// <param name="radius">Radius of the circle</param>
        /// <param name="center">Center of the circle</param>
        /// <returns></returns>
        public static Vector2 GetPolarCoordinates(float angle, float radius, Vector3 center)
        {
            // Gets the final position 
            float angleRad = Mathf.Deg2Rad * angle;
            float xPostion = center.x + (radius * Mathf.Cos(angleRad));
            float yPostion = center.y + (radius * Mathf.Sin(angleRad));

            return new Vector2(xPostion, yPostion);
        }
        
    }
}

