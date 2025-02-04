using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private GameObject playerActual;
    private Vector2 posPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        playerActual = GameObject.FindGameObjectWithTag("Player");
        posPlayer = playerActual.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerActual == null)
        {
            playerActual = Instantiate(player);
            playerActual.transform.position = posPlayer;
        }
    }
}
