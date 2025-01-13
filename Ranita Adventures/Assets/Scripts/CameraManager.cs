using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float distanciaDelante = 0;
    [SerializeField] float distanciaArriba = 0;
    [SerializeField] float minXPos = 0;
    [SerializeField] float maxXPos = 100;
    [SerializeField] float minYPos = 0;
    [SerializeField] float smoothTime = 0.25f;
    [SerializeField] GameObject player;
    Vector2  posInicial;
    [SerializeField] Camera cam;
    private Vector3 velocity;

    void Awake()
    {
        player = GameObject.Find("Player");
        posInicial = player.transform.position;
        velocity = player.GetComponent<Rigidbody2D>().velocity;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        movimientoCamara();

        if (Input.GetKey(KeyCode.R))
        {
            player.transform.position = posInicial;
        }
    }

    void movimientoCamara()
    {
        var cameraPos = cam.transform.position;
        cameraPos.x = Mathf.Clamp(player.transform.position.x + distanciaDelante, minXPos, maxXPos);
        cameraPos.y = Mathf.Max(minYPos, player.transform.position.y - distanciaArriba);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, cameraPos, ref velocity, smoothTime);
    }
}
