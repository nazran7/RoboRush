using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{
    //following target for paralax
    [SerializeField] Transform followingTarget;
    //paralax effect strength
    [SerializeField, Range(0f, 1f)] float parallaxStrength = 0.1f;
    //vertical paralax on/off
    [SerializeField] private bool disableVertParallax;

    Vector3 targetPrevPos;
    private void Start()
    {
        //start follow object
        if (!followingTarget)
        {
            followingTarget = Camera.main.transform;
        }
        targetPrevPos = followingTarget.position;
    }
    private void Update()
    {
        //paralax effect flow
        var delta = followingTarget.position - targetPrevPos;

        if (disableVertParallax)
            delta.y = 0;

        targetPrevPos = followingTarget.position;

        transform.position += delta * parallaxStrength; 
    }
}
