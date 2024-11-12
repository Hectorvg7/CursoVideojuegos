using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Bola : MonoBehaviour
{
    [SerializeField] float velocidadVertical;
    [SerializeField] float velocidadHorizontal;
    private float velocidadMinima = 4f;
    private float direccionX;
    public Transform pala;
    private Vector3 posicionInicial;



    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;
        direccionX = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LanzarBola()
    {
        //Calcular velocidades y dirección del saque.
        velocidadVertical = UnityEngine.Random.Range(4, 7);
        velocidadHorizontal = UnityEngine.Random.Range(4, 7);
        direccionX = UnityEngine.Random.Range(0, 2) == 0 ? velocidadHorizontal : -velocidadHorizontal;

        //Aplicar velocidades en el RigidBody2D.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direccionX, velocidadVertical);
    }


    public void ResetearBola()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        transform.position = posicionInicial;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        if (Math.Abs(rb.velocity.x) < velocidadMinima)
        {
            rb.velocity = new Vector2(Math.Sign(rb.velocity.x) * velocidadMinima, rb.velocity.y);
        }

        if (Math.Abs(rb.velocity.y) < velocidadMinima)
        {
            rb.velocity = new Vector2(rb.velocity.x, Math.Sign(rb.velocity.y) * velocidadMinima);
        }
    }
}
