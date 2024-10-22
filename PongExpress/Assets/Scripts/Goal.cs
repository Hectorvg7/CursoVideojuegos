using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{

    public bool isPlayer1Goal;


    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if (collision.CompareTag("Ball"))
        {
            GetComponent<AudioSource>().Play();
            gm.IncrementarMarcador(!isPlayer1Goal);
            gm.RestartGame();
            gm.ComprobarGanador();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
