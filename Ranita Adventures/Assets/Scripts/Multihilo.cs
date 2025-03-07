using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multihilo : MonoBehaviour
{
    [SerializeField] GameObject sierra;
    [SerializeField] GameObject plataforma;

    void Awake()
    {
        Invoke("SpawnSierra", 5f);
        InvokeRepeating("PlataformaIntermitente", 3f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //La sierra aparecerá y desaparecerá cada X segundos.
    void SpawnSierra()
    {
        if (sierra.activeSelf)
        {
            sierra.SetActive(false);
        } else
        {
            sierra.SetActive(true);
        }
    }

    void PlataformaIntermitente()
    {
        if (plataforma.activeSelf)
        {
            plataforma.SetActive(false);
        } else
        {
            plataforma.SetActive(true);
        }
    }
}
