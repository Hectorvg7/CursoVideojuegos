using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CinemachineTransposer transposer;
    public CinemachineVirtualCamera virtualCamera;
    private float targetOffsetValue;
    private float rotation;
    private float rotationSpeed = 30;
    private Vector2 movement;
    private float movementSpeed = 10;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 20f;
    [SerializeField] float smoothTime = 0.3f;

    void Awake()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset.y = 7f;
        targetOffsetValue = transposer.m_FollowOffset.y;
    }

    public void OnRotate()
    {
        //Rotar hacia la izquierda.
        if (Input.GetKey(KeyCode.Q))
        {
            rotation = 90;
            ApplyRotation();
        }

        //Rotar hacia la derecha.
        if (Input.GetKey(KeyCode.E))
        {
            rotation = -90;
            ApplyRotation();
        }
    }

    public void OnMove()
    {
        //Mover hacia delante.
        if (Input.GetKey(KeyCode.W))
        {
            movement.y = 10;
            ApplyMovement();
            ReiniciarMovement();
        }

        //Mover hacia detrás.
        if (Input.GetKey(KeyCode.S))
        {
            movement.y = -10;
            ApplyMovement();
            ReiniciarMovement();
        }

        //Mover hacia la izquierda.
        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -10;
            ApplyMovement();
            ReiniciarMovement();
        }


        //Mover hacia la derecha.
        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 10;
            ApplyMovement();
            ReiniciarMovement();
        }
    }

    public void OnZoom()
    {
        float scrollInput = Mouse.current.scroll.ReadValue().y;

        if (scrollInput != 0)
        {
            // Calculamos el nuevo valor del zoom (modificamos el FollowOffset de la cámara)
            targetOffsetValue -= scrollInput * zoomSpeed;

            // Limitamos el zoom para que no se aleje demasiado ni se acerque demasiado
            targetOffsetValue = Mathf.Clamp(targetOffsetValue, maxZoom, minZoom);
            
            // Actualizamos la distancia de la cámara con el Smooth Damp para suavizar el movimiento.
            Vector3 currentOffset = transposer.m_FollowOffset;
            targetOffsetValue = Mathf.SmoothDamp(currentOffset.y, targetOffsetValue, ref scrollInput, smoothTime);
            currentOffset.y = targetOffsetValue;
            transposer.m_FollowOffset = currentOffset;
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

    void ReiniciarMovement()
    {
        movement.x = 0;
        movement.y = 0;
    }
}
