using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private float speed = 20f;

    private float fixedY; // altura fija
    public GameObject explosionParticle;

    public void SetTarget(Vector3 target)
    {
        // Guardamos la altura actual de la bala y usamos esa para el target
        fixedY = transform.position.y;
        targetPosition = new Vector3(target.x, fixedY, target.z);
    }

    void Update()
    {
        // Calculamos dirección solo en XZ y aplicamos la altura fija
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f; // aseguramos que la dirección sea solo en XZ

        // Movimiento
        transform.position += direction * speed * Time.deltaTime;

        // Corregimos altura por si se desvía
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);

        // Si está cerca del destino, destruirla
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                             new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.2f)
        {
            Destroy(gameObject);
            GameObject.Instantiate(explosionParticle, transform.position, Quaternion.identity);
        }
    }
}
