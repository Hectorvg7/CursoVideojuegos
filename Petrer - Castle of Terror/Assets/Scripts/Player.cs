using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector3 direction;
    float speed = 5f;
    Rigidbody2D rb;
    bool attackPending = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        if (direction != Vector3.zero)
        {
            direction.Normalize();
        }
    }

    void Move()
    {
        rb.velocity = direction * speed;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackPending = true;
        }
    }
}
