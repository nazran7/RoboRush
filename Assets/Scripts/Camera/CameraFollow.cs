using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject focusedObject;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 offset;

    private const float cameraZ = -10;
    private PlayerMover pm;
    private delegate void Follow();
    private Follow follow;
    private void Start()
    {
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

    private void FollowObject()
    {
        Camera.main.transform.position = Vector3.MoveTowards(transform.position,
                    focusedObject.transform.position, cameraSpeed);
    }
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
