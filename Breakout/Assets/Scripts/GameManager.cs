using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // Instancia estática de GameManager
    private GameObject pala;
    private GameObject bola;
    private Vidas vidas;
    private GameObject panel;
    public GameObject bolaNueva;
    public GameObject panelNuevo;
    public GameObject palaNueva;
    public GameObject nivelGanadoText;
    public GameObject nivelGanadoActual;
    private bool enJuego = false;
    private bool isGameOver = false;
    private Vector3 posicionBola;
    private Vector3 posicionPala;
    private Vector3 posicionPanel;
    private Vector3 tamanoPala;

    private int totalBloques;
    private int bloquesRestantes;
    public int numeroVidas;
    public int bolasActivas;




    // Start is called before the first frame update
    void Start()
    {
        bola = Instantiate(bolaNueva);
        bolasActivas = 1;
        panel = Instantiate(panelNuevo);
        pala = Instantiate(palaNueva);
        panel.SetActive(true);
        posicionBola = bola.transform.position;
        tamanoPala = pala.transform.localScale;
        posicionPala = pala.transform.position;
        posicionPanel = panel.transform.position;

        vidas = FindObjectOfType<Vidas>();
        numeroVidas = vidas.vidasRestantes;


        GameObject[] bloques = GameObject.FindGameObjectsWithTag("Bloque");
        totalBloques = bloques.Length;
        bloquesRestantes = totalBloques;
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene(0);
            isGameOver = false;
            numeroVidas = 6;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !enJuego && !isGameOver && SceneManager.GetActiveScene().buildIndex != 0)
        {
            IniciarJuego();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Se ha cargado la escena");
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            IniciarNivel();
        }
    }

    public void ReiniciarNivel()
    {
        bola = Instantiate(bolaNueva, posicionBola, Quaternion.identity);
        bolasActivas = 1;
        pala.transform.position = posicionPala;
        pala.transform.localScale = tamanoPala;
        panel.SetActive(true);
        enJuego = false;
        Time.timeScale = 0f;
    }

    public void IniciarNivel()
    {
        bola = Instantiate(bolaNueva, posicionBola, Quaternion.identity);
        pala = Instantiate(palaNueva, posicionPala, Quaternion.identity);
        panel = Instantiate(panelNuevo, posicionPanel, Quaternion.identity);
        
        bloquesRestantes = totalBloques;
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

    public void BloqueDestruido()
    {
        bloquesRestantes--;
        Debug.Log(bloquesRestantes);
        if (bloquesRestantes <= 0)
        {
            StartCoroutine(MostrarNivelGanado());
        }
    }

    public void ContarVidas(int vidasRestantes)
    {
        numeroVidas = vidasRestantes;
    }

    private IEnumerator MostrarNivelGanado(){
        nivelGanadoActual = Instantiate(nivelGanadoText);
        nivelGanadoActual.SetActive(true);
        Rigidbody2D rbBola = bola.GetComponent<Rigidbody2D>();
        rbBola.isKinematic = true;

        yield return new WaitForSeconds(5);
        nivelGanadoActual.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
