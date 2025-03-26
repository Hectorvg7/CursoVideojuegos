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
    private float rotationSpeed = 1000;
    private Vector2 movement;
    private float movementSpeed = 10;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 20f;
    [SerializeField] float smoothTime = 0.3f;

    void Awake()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetOffsetValue = transposer.m_FollowOffset.y;
    }

    public void OnRotate(InputAction.CallbackContext input)
    {
        rotation = input.ReadValue<float>();

        transform.Rotate(Vector3.up, rotation * rotationSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext input)
    {
        Vector3 moveDirection = Vector3.zero;

        // Obtenemos los valores de las teclas W, A, S, D para el movimiento.
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.z = 1f; // Movimiento hacia delante
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.z = -1f; // Movimiento hacia atrás
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.x = -1f; // Movimiento hacia la izquierda
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x = 1f; // Movimiento hacia la derecha
        }

        // Movemos la cámara en la dirección calculada
        float movementSpeed = 10f; // Ajusta la velocidad de movimiento
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);
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
