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
    private Vector3 targetOffsetValue;

    //Valores Rotación
    private float rotation;
    private float rotationSpeed = 100;
    private float initialCameraPosition;
    [SerializeField] private float angleRotationLimit = 360;
    private float currentRotation;
    private float minRotation = -180,
        maxRotation = 360;

    //Valores Movimiento
    private Vector2 movement;
    private float movementSpeed = 10;
    [SerializeField] float minX = 0;
    [SerializeField] float maxX = 40;
    [SerializeField] float minZ = 0;
    [SerializeField] float maxZ = 40;

    //Valores Zoom
    private float zoom;
    [SerializeField] float zoomSpeed = 10f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 20f;


    private bool isMove = false,
        isRotate = false,
        isZoom = false;
    [SerializeField] float smoothTime = 0.3f;

    void Awake()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetOffsetValue = transposer.m_FollowOffset;
        CheckRotationLimits();
    }

    void Update()
    {
        Move();
        Rotate();
        Zoom();
    }

    private void CheckRotationLimits()
    {
        initialCameraPosition = transform.eulerAngles.y;
        currentRotation = initialCameraPosition;
        minRotation = initialCameraPosition - angleRotationLimit;
        maxRotation = initialCameraPosition + angleRotationLimit;
    }

    public void OnRotate(InputAction.CallbackContext input)
    {
        rotation = input.ReadValue<float>();
        Debug.Log("Rotation Value: " + rotation);  // Verifica que el valor está cambiando

        if (input.performed)
        {
            isRotate = true;
        } 
        else if (input.canceled)
        {
            isRotate = false;
        }
    }

    public void OnMove(InputAction.CallbackContext input)
    {
        movement = input.ReadValue<Vector2>();

        if (input.performed)
        {
            movement.Normalize();
            isMove = true;
        }
        else if (input.canceled)
        {
            isMove = false;
        }
    }

    public void OnZoom(InputAction.CallbackContext input)
    {
        zoom = input.ReadValue<float>();
        Debug.Log("Zoom Value: " + zoom);  // Verifica el valor del zoom

        if (input.performed)
        {
            isZoom = true;
        }
        else if (input.canceled)
        {
            isZoom = false;
        }
    }

    private void Move()
    {
        if (isMove)
        {
        Vector3 worldMovement = transform.forward * movement.y + transform.right * movement.x;
            transform.position += worldMovement * movementSpeed * Time.deltaTime;
            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);

            // Asignamos la nueva posición limitada (mantenemos la altura constante de la cámara)
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
        }
    }

    private void Rotate()
    {
        if (isRotate)
        {
            currentRotation = Mathf.Clamp(
                currentRotation - rotation * rotationSpeed * Time.deltaTime,
                minRotation,
                maxRotation
            );

            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                currentRotation,
                transform.eulerAngles.z
            );
        }
    }

    private void Zoom()
    {
        if (isZoom)
        {
            // Ajusta la distancia de la cámara modificando el FollowOffset
            float newCameraDistance = transposer.m_FollowOffset.z - zoom * zoomSpeed * Time.deltaTime;
            
            // Limita la distancia de la cámara
            newCameraDistance = Mathf.Clamp(newCameraDistance, minZoom, maxZoom);
            
            // Asigna el nuevo valor de distancia a la cámara
            transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, targetOffsetValue.y, newCameraDistance);
        }
    }
}
