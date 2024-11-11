using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{
    [SerializeField] float velocidadVertical;
    [SerializeField] float velocidadHorizontal;
    private float velocidadMinima = 3f;
    private float direccionX;
    private bool enJuego = false;
    public Transform pala;
    public GameObject panel;
    private Vector3 posicionInicial;



    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;
        direccionX = 0f;
        panel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !enJuego)
        {
            LanzarBola();
            panel.SetActive(false);
        }

    }

    void LanzarBola()
    {
        //Calcular velocidades y dirección del saque.
        velocidadVertical = Random.Range(3, 6);
        velocidadHorizontal = Random.Range(3, 6);
        direccionX = Random.Range(0, 2) == 0 ? velocidadHorizontal : -velocidadHorizontal;

        //Aplicar velocidades en el RigidBody2D.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direccionX, velocidadVertical);
        enJuego = true;
    }


    public void ResetearBola()
    {
        enJuego = false;
        transform.position = posicionInicial;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        if (rb.velocity.magnitude < velocidadMinima)
        {
            rb.velocity = new Vector2(rb.velocity.x * velocidadMinima, rb.velocity.y * velocidadMinima);
        }
    }
}
