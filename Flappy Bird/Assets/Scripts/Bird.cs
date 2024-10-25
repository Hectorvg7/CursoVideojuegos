using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bird : MonoBehaviour
{
    public float fuerzaSalto = 10f;
    public float alturaMaxima = 5f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Saltar();
        LimitarAltura();
    }

    private void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            GetComponent<AudioSource>().Play();
        }
    }

    private void LimitarAltura()
    {
        if (transform.position.y > alturaMaxima)
        {
            transform.position = new Vector2(transform.position.x, alturaMaxima);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Tuberia") || collider.gameObject.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}
