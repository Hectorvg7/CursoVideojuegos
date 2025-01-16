using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{

    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float velocidadMov = 2f;
    [SerializeField] float movimiento = 5f;
    [SerializeField] bool horizontal = false;
    [SerializeField] bool vertical = true;
    private Vector3 posInicial;
    private bool subiendo = true;
    private bool derecha = true;


    // Start is called before the first frame update
    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (vertical)
        {
        MoverY();
        }

        if (horizontal)
        {
            MoverX();
        }
    }

    void MoverY()
    {
        Vector3 destino = subiendo ? posInicial + Vector3.up * movimiento : posInicial;
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMov * Time.deltaTime);

        if (transform.position == destino)
        {
            subiendo = !subiendo;
        }
    }

    void MoverX()
    {
        Vector3 destino = derecha ? posInicial + Vector3.right * movimiento : posInicial;
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMov * Time.deltaTime);

        if (transform.position == destino)
        {
            derecha = !derecha;
        }
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("Player"))
        {
            Destroy(colision.gameObject);
        }
    }
}
