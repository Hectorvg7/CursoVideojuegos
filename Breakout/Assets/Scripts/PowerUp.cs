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
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ha tocado la pala");
            Destroy(gameObject);
        }
        if (collider.gameObject.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}
