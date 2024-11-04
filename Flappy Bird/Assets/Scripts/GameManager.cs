using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused = true;
    public bool isGameOver = false;

    public GameObject pausa;
    public GameObject inicio;
    public GameObject puntuacion;
    public GameObject bird;
    private GameObject birdActual;
    public GameObject medalla;
    public GameObject panelGameOver;
    public GameObject botonSalir;
    public TextMeshProUGUI textoGameOver;
    public Transform medallasContainer;
    public Scroller scroller;
    public int puntos = 0;



    // Start is called before the first frame update
    void Start()
    {
        panelGameOver.SetActive(false);
        pausa.SetActive(false);
        puntuacion.SetActive(false);
        inicio.SetActive(true);
        botonSalir.SetActive(false);
        birdActual = Instantiate(bird);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PausarJuego();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IniciarJuego();
        }
    }

    void PausarJuego()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausa.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausa.SetActive(false);
        }
    }


    void IniciarJuego()
    {
        if (isPaused)
        {
            botonSalir.SetActive(true);
            inicio.SetActive(false);
            bird.SetActive(true);
            puntuacion.SetActive(true);
            Time.timeScale = 1f;
            isPaused = false;
        }
        if (isGameOver)
        {
            ReiniciarJuego();
        }
    }

    void ReiniciarJuego()
    {
        if (birdActual != null)
        {
            Destroy(birdActual);
        }

            birdActual = Instantiate(bird);
            scroller.ReiniciarTuberias();
            panelGameOver.SetActive(false);
            puntos = 0;
            puntuacion.GetComponent<TextMeshProUGUI>().text = "0";

            foreach (Transform child in medallasContainer)
            {Destroy(child.gameObject);}

            inicio.SetActive(true);
            isPaused = true;
            isGameOver = false;
    }

    public void IncrementarPuntuacion()
    {
        puntos++;
        puntuacion.GetComponent<TextMeshProUGUI>().text = puntos.ToString();
        GetComponent<AudioSource>().Play();

        if (puntos % 5 == 0)
        {
            // Calcular la posición para la nueva medalla
        Vector3 nuevaPosicion = medallasContainer.position + new Vector3(0, (-medallasContainer.childCount), 0);
        
        // Instanciar la medalla en la nueva posición
        GameObject medallaInstanciada = Instantiate(medalla, nuevaPosicion, Quaternion.identity, medallasContainer);
        
        // Ajustar la posición de la medalla para que esté en la lista de hijos
        medallaInstanciada.transform.SetParent(medallasContainer);
    
        }
    }

    public void MostrarGameOver(int puntos)
    {
        panelGameOver.SetActive(true);
        textoGameOver.text = $"\n\nHas conseguido " + puntos.ToString() + " puntos\n\nPresiona espacio para continuar";
        Time.timeScale = 0;
        isGameOver = true;
    }


}
