using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bird : MonoBehaviour
{
    private AudioSource audioColision;
    private AudioSource audioSalto;
    public AudioClip colision;
    public AudioClip salto;
    public float fuerzaSalto = 7f;
    public float alturaMaxima = 5f;
    private float velocidadRotacion = 10f;
    private Rigidbody2D rb;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        audioSalto = gameObject.AddComponent<AudioSource>();
        audioSalto.clip = salto;
        
        audioColision = gameObject.AddComponent<AudioSource>();
        audioColision.clip = colision;
    }

    // Update is called once per frame
    void Update()
    {
        Saltar();
        LimitarAltura();
        RotarPajaro();
    }

    private void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            audioSalto.Play();
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

    private void RotarPajaro()
    {
         // Obtener la velocidad en el eje Y
        float verticalVelocity = rb.velocity.y;

        // Calcular la rotación basada en la velocidad vertical
        float targetRotationZ = Mathf.Clamp(verticalVelocity * velocidadRotacion, -45f, 45f);
        
        // Aplicar la rotación al pájaro
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetRotationZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * velocidadRotacion);
    
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Tuberia") || collider.gameObject.CompareTag("Suelo"))
        {
            gm.MostrarGameOver(gm.puntos);
            audioColision.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (gameObject != null && gameObject.activeInHierarchy)
        {
            if (collider.gameObject.CompareTag("Tuberias"))
            {
                gm.IncrementarPuntuacion();
            }
        }
    }
}
