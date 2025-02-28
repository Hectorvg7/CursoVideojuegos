using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineTransposer transposer;
    public CinemachineVirtualCamera virtualCamera;
    private float targetOffsetValue;
    private float rotation;
    private float rotationSpeed = 30;
    private Vector2 movement;
    private float movementSpeed = 5;

    void Awake()
    {
      transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
      targetOffsetValue = transposer.m_FollowOffset.y;
    }

    public void OnRotate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            rotation = 90;
            Debug.Log("Rotando hacia derecha");
            ApplyRotation();
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotation = -90;
            Debug.Log("Rotando hacia izquierda");
            ApplyRotation();
        }
    }

    public void OnMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movement.x = 10;
            Debug.Log("Moviendo hacia alante");
            ApplyMovement();
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement.x = -10;
            Debug.Log("Moviendo hacia atras");
            ApplyMovement();
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement.y = 10;
            Debug.Log("Moviendo hacia izq");
            ApplyMovement();
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement.y = -10;
            Debug.Log("Moviendo hacia derecha");
            ApplyMovement();
        }
    }

    private void ApplyRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        rotationVector.y = rotation;

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    void ApplyMovement()
    {
        Vector3 worldMovement = transform.forward * movement.y + transform.right * movement.x;
        transform.position += worldMovement * movementSpeed * Time.deltaTime;
    }
}
