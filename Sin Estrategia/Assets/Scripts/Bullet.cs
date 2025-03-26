using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform transformBala;
    private bool derecha = true;
    private Vector3 posInicial;
    [SerializeField] float velocidadMov = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        transformBala = GetComponent<Transform>();
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoverX();
    }

    void MoverX()
    {
        Vector3 destino = derecha ? posInicial + Vector3.right * 10 : posInicial;
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMov * Time.deltaTime);

        if (transform.position == destino)
        {
            derecha = !derecha;
        }
    }
}
