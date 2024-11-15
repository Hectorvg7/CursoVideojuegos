using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private GameObject pala;
    private GameObject bola;
    public GameObject vidas;
    private GameObject panel;
    public GameObject bolaNueva;
    public GameObject panelNuevo;
    public GameObject palaNueva;
    private bool enJuego = false;
    private bool isGameOver = false;
    private Vector3 posicionBola;
    private Vector3 posicionPala;
    private Vector3 posicionPanel;


    // Start is called before the first frame update
    void Start()
    {
        bola = Instantiate(bolaNueva);
        panel = Instantiate(panelNuevo);
        pala = Instantiate(palaNueva);
        panel.SetActive(true);
        posicionBola = bola.transform.position;
        posicionPala = pala.transform.position;
        posicionPanel = panel.transform.position;
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
        {
            SceneManager.LoadScene(0);
            isGameOver = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !enJuego && !isGameOver)
        {
            IniciarJuego();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Se ha cargado la escena");
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            IniciarNivel();
        }
    }

    public void ReiniciarNivel()
    {
        bola = Instantiate(bolaNueva, posicionBola, Quaternion.identity);
        pala.transform.position = posicionPala;
        panel.SetActive(true);
        enJuego = false;
        Time.timeScale = 0f;
    }

    public void IniciarNivel()
    {
        bola = Instantiate(bolaNueva, posicionBola, Quaternion.identity);
        pala = Instantiate(palaNueva, posicionPala, Quaternion.identity);
        panel = Instantiate(panelNuevo, posicionPanel, Quaternion.identity);
        panel.SetActive(true);
        enJuego = false;
        Time.timeScale = 0f;
    }

    public void IniciarJuego()
    {
        Time.timeScale = 1f;
        panel.SetActive(false);
        enJuego = true;
        bola.GetComponent<Bola>().LanzarBola();
    }

    public void MostrarGameOver()
    {
        SceneManager.LoadScene(3);
        isGameOver = true;
    }
}
