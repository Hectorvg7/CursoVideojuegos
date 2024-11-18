using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    /*
    public GameObject pala;
    public GameObject bola;
    public GameObject vidas;
    */

    [SerializeField] float incremento = 0.25f;
    public bool powerUp1;
    public bool powerUp2;
    public bool powerUp3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && powerUp2)
        {
            Debug.Log("Ha tocado la pala");
            Debug.Log("Crecio la pala");
            AumentarPala(collider.gameObject);
            Destroy(gameObject);
        }

        if (collider.gameObject.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }

    void AumentarPala(GameObject pala)
    {
        float tamanoMax = 2f;
        Transform transformPala = pala.transform;
        transformPala.localScale = new Vector3(transformPala.localScale.x + incremento, transformPala.localScale.y, transformPala.localScale.z);

        if (transformPala.localScale.x > tamanoMax)
        {
            transformPala.localScale = new Vector3(tamanoMax, transformPala.localScale.y, transformPala.localScale.z);
        }
    }
}
