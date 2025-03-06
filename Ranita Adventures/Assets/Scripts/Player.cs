using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] GameManager gm;
    [SerializeField] LayerMask mapLayer;
    public AudioClip audioSalto;

    bool irDerecha = true;
    float velocidadMov = 5f;
    float fuerzaSalto = 8f;
    float fuerzaDobleSalto = 5f;
    public PlayerState estado = PlayerState.iddle;

    // Guardamos las referencias a las acciones del Input System
    private float irHorizontal;
    private bool saltar;

    public enum PlayerState
    {
        iddle,
        running,
        jump,
        doubleJump,
        sliding
    }

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /* Leemos los inputs del usuario, 
    comprobamos la orientación a la que debe mirar el personaje 
    y establecemos las condiciones para las animaciones. */
    void Update()
    {
        ControlOrientation();
        SetAnimations();
    }


    public void OnMove(InputAction.CallbackContext input)
    {
        Vector2 moveInput = input.ReadValue<Vector2>();
        irHorizontal = moveInput.x;
    }

    public void OnJump(InputAction.CallbackContext input)
    {
        if (input.ReadValue<float>() > 0.1f)
        {
            saltar = true;
        }
    }

    private void ControlOrientation()
    {
        if (rb.velocity.x > 0.1f && !irDerecha)
        {
            irDerecha = true;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < -0.1f && irDerecha)
        {
            irDerecha = false;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void SetAnimations()
    {
        animator.SetBool("Running", irHorizontal != 0);
        animator.SetBool("Grounded", isGrounded());
        animator.SetBool("OnTheWall", isNextToTheWall());
    }

//Cambios en las físicas.
    void FixedUpdate()
    {
        switch (estado)
        {
            case PlayerState.iddle:
                FixedUpdateIddle();
                break;
            case PlayerState.running:
                FixedUpdateRun();
                break;
            case PlayerState.jump:
                FixedUpdateJump();
                break;
            case PlayerState.doubleJump:
                FixedUpdateDoubleJump();
                break;
            case PlayerState.sliding:
                FixedUpdateSliding();
                break;
        }

    }


// Estado Iddle
    void FixedUpdateIddle()
    {
        CheckIddleTransitions();
        MoverX();
        IntentarSalto();
    }

    void CheckIddleTransitions()
    {
        if (!isGrounded() && isNextToTheWall())
            {
                estado = PlayerState.sliding;

            }
        if (isGrounded() && saltar)
            {
                animator.SetTrigger("Jumps");
                estado = PlayerState.jump;
            }
        else if (irHorizontal != 0)
        {
            estado = PlayerState.running;
        }
    }
    
    void MoverX()
    {
        rb.velocity = new Vector2(irHorizontal * velocidadMov, rb.velocity.y);
    }

    void IntentarSalto()
    {
        if (saltar && isGrounded())
        {
            Jump();
        }
        saltar = false;
    }


//Estado Run
    void FixedUpdateRun()
    {
        CheckRunTransitions();
        MoverX();
        IntentarSalto();
    }

    void CheckRunTransitions()
    {
        if (!isGrounded())
        {
            if (isNextToTheWall())
            {
                estado = PlayerState.sliding;
            }
            else
            {
                animator.SetTrigger("Jumps");
                estado = PlayerState.jump;
            }
        }
        else if (irHorizontal == 0)
        {
            estado = PlayerState.iddle;
        }
    }


//Estado Jump
    void FixedUpdateJump()
    {
        CheckJumpTransitions();
        MoverX();
        IntentarDobleSalto();
    }

    void CheckJumpTransitions()
    {
        if (isGrounded())
        {
            estado = PlayerState.iddle;
        }
        else if(isNextToTheWall())
        {
            estado = PlayerState.sliding;
        }
    }

    void IntentarDobleSalto()
    {
        if (saltar)
        {
            DoubleJump();
            saltar = false;
            estado = PlayerState.doubleJump;
        }
    }


//Estado DoubleJump
    void FixedUpdateDoubleJump()
    {
        CheckDoubleJumpTransitions();
        MoverX();
        saltar = false;
    }

    void CheckDoubleJumpTransitions()
    {
        if (isGrounded())
        {
            estado = PlayerState.iddle;
        }
        else if (isNextToTheWall())
        {
            estado = PlayerState.sliding;
        }
    }
 

//Estado Slide
    void FixedUpdateSliding()
    {
        CheckSlidingTransitions();
        MoverX();
        IntentarDobleSalto();
    }

    void CheckSlidingTransitions()
    {
        if (isGrounded())
        {
            estado = PlayerState.iddle;
        }
        else if (!isNextToTheWall())
        {
            estado = PlayerState.jump;
        }
    }



//Comprobar si está tocando el suelo o la pared.
    public bool isGrounded()
    {
        var boxCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, mapLayer);
        return boxCastHit.collider != null;
    }

    public bool isNextToTheWall()
    {
        Vector2 direccion = irDerecha ? Vector2.right : Vector2.left;
        var boxCastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, direccion, 0.01f, mapLayer);
        return boxCastHit.collider != null;
    }


//Método para saltar y hacer doble salto.
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        animator.SetTrigger("Jumps");
        AudioManager.Instance.PlaySound(audioSalto);
    }

    void DoubleJump()
    {
        if (isNextToTheWall())
        {
            rb.velocity = new Vector2(irDerecha ? -1f : 1f, fuerzaDobleSalto);
        }
        else 
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaDobleSalto);
        }
        animator.SetTrigger("DoubleJumps");
        AudioManager.Instance.PlaySound(audioSalto);
    }


}
