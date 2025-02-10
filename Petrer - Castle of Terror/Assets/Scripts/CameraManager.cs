using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera cam;
    float currentPosX;
    float currentPosY;
    float smoothTime = 0.25f;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = Vector3.SmoothDamp(
            cam.transform.position,
            new Vector3(currentPosX, currentPosY, cam.transform.position.z),
            ref velocity,
            smoothTime
        );
    }

    public void MoveToNewRoom(Transform newRoom)
    {
        currentPosX = newRoom.position.x;
        currentPosY = newRoom.position.y;
    }
}
