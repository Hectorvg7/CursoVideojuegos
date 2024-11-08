using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pala : MonoBehaviour
{
    [SerializeField] float velocidad = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * movimiento * velocidad * Time.deltaTime);
    }
}
