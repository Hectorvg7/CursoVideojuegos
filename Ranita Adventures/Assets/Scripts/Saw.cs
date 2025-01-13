using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{

    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float velocidadMov = 2f;
    [SerializeField] float movimiento = 5f;
    private Vector3 posInicial;
    private bool subiendo = true;


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
        MoverY();
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
}
