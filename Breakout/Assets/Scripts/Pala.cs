using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pala : MonoBehaviour
{
    [SerializeField] float velocidad = 10f;
    private Rigidbody2D rb;
    private float minX, maxX;

    // Start is called before the first frame update
    void Start()
    {
         // Obtener el Rigidbody de la pala
        rb = GetComponent<Rigidbody2D>();

        // Hacer que la pala no se vea afectada por las físicas (pero aún puede detectar colisiones)
        rb.isKinematic = true;

        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));  // Esquina inferior izquierda
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));  // Esquina superior derecha

        // Establecer los límites de la pala
        minX = bottomLeft.x + GetComponent<Collider2D>().bounds.size.x / 2;  // Ajustar según el tamaño de la pala
        maxX = topRight.x - GetComponent<Collider2D>().bounds.size.x / 2;  // Ajustar según el tamaño de la pala
    }

    // Update is called once per frame
    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * movimiento * velocidad * Time.deltaTime);

        // Limitar la posición de la pala para que no se pase de los límites de la pantalla
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);

        // Asignar la posición limitada de la pala
        transform.position = new Vector2(clampedX, transform.position.y);
    }
}
