using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] GameManager gm;
    [SerializeField] LayerMask mapLayer;
    [SerializeField] AudioSource audioSource;
    float irHorizontal;
    bool irDerecha = true;
    bool saltar;
    float velocidadMov = 5f;
    float fuerzaSalto = 10f;
    float fuerzaDobleSalto = 6f;
    public PlayerState estado = PlayerState.iddle;

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
        audioSource = GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ReadInputs();
        ControlOrientation();
        SetAnimations();
    }


    private void ReadInputs()
    {
        //Recoger inputs del usuario.
        irHorizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
            saltar = true;
    }

    private void ControlOrientation()
    {
        if (rb.velocity.x > 0 && !irDerecha)
        {
            irDerecha = true;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < 0 && irDerecha)
        {
            irDerecha = false;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        } else if (rb.velocity.x == 0 && irDerecha)
        {
            irDerecha = true;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


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
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        animator.SetTrigger("Jumps");
        //audioSource.PlayOneShot(jumpClip);
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
        //audioSource.PlayOneShot(doubleJumpClip);
    }


}
