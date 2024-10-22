using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject ball;

    int golesPlayer1 = 0;
    int golesPlayer2 = 0;
    private bool isPlaying;

    private Vector3 posicionPlayer1;
    private Vector3 posicionPlayer2;
    private Vector3 escalaPlayer1;
    private Vector3 escalaPlayer2;
    private Vector3 posicionBall;
    private float velocidadBall;

    public TextMeshProUGUI player1Score;
    public TextMeshProUGUI player2Score;
    public TextMeshProUGUI mensajeGanador;

    // Start is called before the first frame update
    void Start()
    {
        posicionPlayer1 = player1.transform.position;
        posicionPlayer2 = player2.transform.position;
        escalaPlayer1 = player1.transform.localScale;
        escalaPlayer2 = player2.transform.localScale;
        posicionBall = ball.transform.position;
        velocidadBall = ball.GetComponent<Ball>().speed;
    }

    public void RestartGame()
    {
        player1.transform.position = posicionPlayer1;
        player2.transform.position = posicionPlayer2;
        ball.transform.position =  posicionBall;
        isPlaying = false;
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
            mensajeGanador.text = "¡El jugador 1 ha ganado!\nR para reiniciar";
        }else if (golesPlayer2 == 5)
        {
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
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying && golesPlayer1 < 5 && golesPlayer2 < 5)
            {
                ball.GetComponent<Ball>().Launch();
                isPlaying = true;
            }else if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
                ReiniciarMarcador();
            }
    }
}
