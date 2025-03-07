using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private GameObject playerActual;
    private Vector2 posPlayer;
    private SpriteRenderer playerSprite;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Awake()
    {
        playerActual = GameObject.FindGameObjectWithTag("Player");
        playerSprite = playerActual.GetComponent<SpriteRenderer>();
        posPlayer = playerActual.transform.position;
        coroutine = Transparentar(playerSprite);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerActual == null)
        {
            playerActual = Instantiate(player);
            playerSprite = playerActual.GetComponent<SpriteRenderer>();
            playerActual.transform.position = posPlayer;
            coroutine = Transparentar(playerSprite);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator Transparentar(SpriteRenderer sprite)
    {
        Color colorActual = sprite.color;
        colorActual.a = 0.4f;
        sprite.color = colorActual;
        
        yield return new WaitForSeconds(2.5f);

        colorActual.a = 1f;
        sprite.color = colorActual;
    }
}
