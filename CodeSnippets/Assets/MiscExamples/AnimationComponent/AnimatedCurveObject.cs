using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCurveObject : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] private float speed = 100.0f;

    // Declare the float to use with the animation curve
    private float curveDeltaTime = 0.0f;

    void Start()
    {
        // Auto destroy object after 5 seconds
        Destroy(gameObject, 15.0f);
    }

    void Update()
    {
        // Get the current position of the sphere
        Vector3 currentPosition = transform.position;
        currentPosition.z += speed * Time.deltaTime;

        // Call evaluate on that time   
        curveDeltaTime += Time.deltaTime;
        currentPosition.y = animationCurve.Evaluate(curveDeltaTime);
        // Update the current position of the sphere
        transform.position = currentPosition;

    }
}
