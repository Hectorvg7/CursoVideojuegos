using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CinemachineFollow transposer;
    public CinemachineCamera virtualCamera;

    //Valores Rotación
    private Vector3 initialRotation;
    private float rotation;
    private float rotationSpeed = 100;
    private float currentRotationY;
    private float minRotation,
        maxRotation;

    //Valores Movimiento
    private Vector2 movement;
    private float movementSpeed = 10,
    minX = -20,
    maxX = 40,
    minZ = -20,
    maxZ = 40;

    //Valores Zoom
    private float zoom;
    [SerializeField] float zoomSpeed = 100f;
    [SerializeField] float minZoom = 0f;
    [SerializeField] float maxZoom = 20f;
    float minZoomY = 2,
    maxZoomY = 15;


    private bool isMove = false,
        isRotate = false,
        isZoom = false;
    [SerializeField] float smoothTime = 0.3f;

    void Awake()
    {
        transposer = virtualCamera.GetComponent<CinemachineFollow>();
        initialRotation = virtualCamera.transform.eulerAngles;
        currentRotationY = initialRotation.y;
    }

    void Update()
    {
        Move();
        Rotate();
        Zoom();
    }



    public void OnRotate(InputAction.CallbackContext input)
    {
        rotation = input.ReadValue<float>();

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
            // Obtenemos la rotación de la cámara solo en Y
            Vector3 cameraForward = virtualCamera.transform.forward;
            Vector3 cameraRight = virtualCamera.transform.right;

            // Eliminamos el componente vertical (para que no suba o baje)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Creamos el movimiento relativo a la cámara
            Vector3 direction = cameraForward * movement.y + cameraRight * movement.x;

            Vector3 newPosition = transform.position + direction * movementSpeed * Time.deltaTime;

            // Clamp para mantener dentro de los límites
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }
    }

    private void Rotate()
    {
        if (isRotate)
        {
            currentRotationY = currentRotationY - rotation * rotationSpeed * Time.deltaTime;


            virtualCamera.transform.eulerAngles = new Vector3(
                virtualCamera.transform.eulerAngles.x,
                currentRotationY,
                virtualCamera.transform.eulerAngles.z
            );
        }
    }

    private void Zoom()
    {
        if (isZoom)
        {
            // Obtenemos la dirección de la cámara (solo X y Z)
            Vector3 cameraForward = virtualCamera.transform.forward;
            cameraForward.y = 0f; // Aseguramos que la dirección de la cámara no afecte la componente Y

            // Normalizamos la dirección para asegurar que tiene longitud 1
            cameraForward.Normalize();

            // Calculamos la cantidad de zoom que queremos aplicar
            float zoomAmount = zoom * zoomSpeed * Time.deltaTime;

            // Obtenemos el FollowOffset actual
            Vector3 currentFollowOffset = transposer.FollowOffset;

            // Calculamos el nuevo FollowOffset ajustado a la dirección de la cámara
            Vector3 newFollowOffset = currentFollowOffset + cameraForward * zoomAmount;

            // Limitamos la distancia para evitar que la cámara se acerque demasiado
            newFollowOffset.z = Mathf.Clamp(newFollowOffset.z, minZoom, maxZoom);
            newFollowOffset.x = Mathf.Clamp(newFollowOffset.x, minZoom, maxZoom);
            newFollowOffset.y = Mathf.Clamp(newFollowOffset.y, minZoomY, maxZoomY);

            // Asignamos el nuevo FollowOffset
            transposer.FollowOffset = new Vector3(newFollowOffset.x, newFollowOffset.y, newFollowOffset.z);
        }
    }
}
