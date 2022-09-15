using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //the object the camera is focusing on
    [SerializeField] private GameObject focusedObject;
    //speed of camera
    [SerializeField] private float cameraSpeed;
    // camera offset
    [SerializeField] private Vector3 offset;

    //camera z position
    private const float cameraZ = -10;
    // player mover component on focus object
    private PlayerMover pm;
    //delegate for camera follow method
    private delegate void Follow();
    private Follow follow;
    private void Start()
    {
        //if camera follow player - follow with offset , else if camera follow object - follow w/out offset
        if (focusedObject.GetComponent<PlayerMover>() != null)
        {
            pm = focusedObject.GetComponent<PlayerMover>();
            follow = FollowPlayer;
        }
        else
            follow = FollowObject;
    }
    private void FixedUpdate()
    {
        follow();
    }
    //follow w/out offset
    private void FollowObject()
    {
        Camera.main.transform.position = Vector3.MoveTowards(transform.position,
                    focusedObject.transform.position, cameraSpeed);
    }
    //follow with side offset
    private void FollowPlayer()
    {
        int side;
        if (pm.isFacingRight)
            side = 1;
        else
            side = -1;
        Vector3 pos = new Vector3(focusedObject.transform.position.x + offset.x * side,
            focusedObject.transform.position.y + offset.y * side, cameraZ);
        Camera.main.transform.position = Vector3.MoveTowards(transform.position,
            pos, cameraSpeed);
    }



}
