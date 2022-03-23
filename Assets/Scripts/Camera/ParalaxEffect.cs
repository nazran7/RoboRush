using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float parallaxStrength = 0.1f;
    [SerializeField] private bool disableVertParallax;

    Vector3 targetPrevPos;
    private void Start()
    {
        if (!followingTarget)
        {
            followingTarget = Camera.main.transform;
        }
        targetPrevPos = followingTarget.position;
    }
    private void Update()
    {
        var delta = followingTarget.position - targetPrevPos;

        if (disableVertParallax)
            delta.y = 0;

        targetPrevPos = followingTarget.position;

        transform.position += delta * parallaxStrength; 
    }
}
