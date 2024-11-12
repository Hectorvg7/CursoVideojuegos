using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pala;
    public GameObject bola;
    public GameObject vidas;
    public GameObject panel;
    private bool enJuego = false;
    private Vector3 posicionBola;
    private Vector3 posicionPala;


    // Start is called before the first frame update
    void Start()
    {
        posicionBola = bola.transform.position;
        posicionPala = pala.transform.position;
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarNivel();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !enJuego)
        {
            IniciarJuego();
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 0f;

        bola.GetComponent<Bola>().ResetearBola();
        pala.transform.position = posicionPala;

        panel.SetActive(true);
        enJuego = false;
    }

    public void IniciarJuego()
    {
        Time.timeScale = 1f;
        panel.SetActive(false);
        enJuego = true;
        bola.GetComponent<Bola>().LanzarBola();
    }
}
