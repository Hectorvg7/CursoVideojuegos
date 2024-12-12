using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject player2CPU;
    public GameObject ball;

    private Paddle paddlePlayer2;
    private AutoPaddle autoPaddlePlayer2;
    public GameObject panelElegirModo;

    int golesPlayer1 = 0;
    int golesPlayer2 = 0;
    private EstadosJuego estadoActual;

    private Vector3 posicionPlayer1;
    private Vector3 posicionPlayer2;
    private Vector3 posicionPlayer2CPU;
    private Vector3 escalaPlayer1;
    private Vector3 escalaPlayer2;
    private Vector3 posicionBall;
    private float velocidadBall;

    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public TextMeshProUGUI mensajeGanador;
    public 

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameOver();
        posicionPlayer1 = player1.transform.position;
        posicionPlayer2 = player2.transform.position;
        posicionPlayer2CPU = player2CPU.transform.position;
        escalaPlayer1 = player1.transform.localScale;
        escalaPlayer2 = player2.transform.localScale;
        posicionBall = ball.transform.position;
        velocidadBall = ball.GetComponent<Ball>().speed;
    }

    enum EstadosJuego
    {
        Ready,
        Playing,
        GameOver
    }

    void UpdateReady()
    {
        estadoActual = EstadosJuego.Ready;
    }

    void UpdatePlaying()
    {
        estadoActual = EstadosJuego.Playing;
    }

    void UpdateGameOver()
    {
        estadoActual = EstadosJuego.GameOver;
    }

    public void RestartGame()
    {
        player1.transform.position = posicionPlayer1;
        player2.transform.position = posicionPlayer2;
        player2CPU.transform.position = posicionPlayer2CPU;
        ball.transform.position =  posicionBall;
        UpdateReady();
        Rigidbody2D rb = GameObject.Find("Ball").GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0); 
        ball.GetComponent<Ball>().speed = velocidadBall;
    }

    public void IncrementarMarcador(bool isPlayer1)
    {
        if (isPlayer1)
        {
            golesPlayer1++;
            player1Score.text = golesPlayer1.ToString();
            ModificarPalas(player2, player1);
        }else
        {
            golesPlayer2++;
            player2Score.text = golesPlayer2.ToString();
            ModificarPalas(player1, player2);
        }
        Debug.Log("Marcador: Player1 [" + golesPlayer1 + " - " + golesPlayer2 + "] Player2");
    }

    public void ReiniciarMarcador()
    {
        golesPlayer1 = 0;
        player1Score.text = golesPlayer1.ToString();
        golesPlayer2 = 0;
        player2Score.text = golesPlayer2.ToString();
        mensajeGanador.text = "";
        player1.transform.localScale = escalaPlayer1;
        player2.transform.localScale = escalaPlayer2;
    }

    public void ComprobarGanador(){
        if (golesPlayer1 == 5)
        {
            UpdateGameOver();
            mensajeGanador.text = "¡El jugador 1 ha ganado!\nR para reiniciar";
        }else if (golesPlayer2 == 5)
        {
            UpdateGameOver();
            mensajeGanador.text = $"¡El jugador 2 ha ganado!\nR para reiniciar";
        }
    }

    public void ModificarPalas(GameObject playerMas, GameObject playerMenos)
    {
        playerMas.transform.localScale += new Vector3(0, 0.75f, 0);
        playerMenos.transform.localScale -= new Vector3(0, 0.75f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estadoActual == EstadosJuego.Ready)
            {
                ball.GetComponent<Ball>().Launch();
                UpdatePlaying();
            }else if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
                ReiniciarMarcador();
                panelElegirModo.SetActive(true);
            }
    }

    public void Modo1vs1()
    {
        player2CPU.SetActive(false);
        player2.SetActive(true);
        panelElegirModo.SetActive(false);
        UpdateReady();
    }

    public void Modo1vsCPU()
    {
        player2.SetActive(false);
        player2CPU.SetActive(true);
        panelElegirModo.SetActive(false);
        UpdateReady();
    }
}
