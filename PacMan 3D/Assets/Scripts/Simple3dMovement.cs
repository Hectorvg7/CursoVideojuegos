using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Simple3DMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField]
    [Range(0.0f, 0.3f)]
    float RotationSmoothTime = 0.12f;

    float xMovement;
    float zMovement;
    float targetRotation;
    CharacterController characterController;
    float rotationVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        zMovement = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(xMovement, zMovement);

        Vector3 inputDirection = new Vector3(xMovement, 0.0f, zMovement).normalized;

        if (move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                      Camera.main.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            //characterController.Move(targetDirection.normalized * (speed * Time.deltaTime));
            characterController.SimpleMove(targetDirection * speed);

        }

    }
}
