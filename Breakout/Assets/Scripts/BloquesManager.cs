using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloques : MonoBehaviour
{
    [SerializeField] int vidaMaxima = 4;
    private int vidasRestantes;

    public Sprite vida1;
    public Sprite vida2;
    public Sprite vida3;
    public Sprite vida4;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        vidasRestantes = vidaMaxima;
        sr = GetComponent<SpriteRenderer>();
        AsignarVidaInicial();
    }

    public void ReducirVida()
    {
        vidasRestantes--;
        ActualizarSprite();
        if (vidasRestantes <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ActualizarSprite()
    {
        switch (vidasRestantes)
        {
            case 4:
                sr.sprite = vida4;
                break;
            case 3:
                sr.sprite = vida3;
                break;
            case 2:
                sr.sprite = vida2;
                break;
            case 1:
                sr.sprite = vida1;
                break;         
            default:
                sr.sprite = vida1;
                break;
        }
    }

    void AsignarVidaInicial(){
        if (sr.sprite == vida1)
        {
            vidasRestantes = 1;
        }
        else if (sr.sprite == vida2)
        {
            vidasRestantes = 2;
        }
        else if (sr.sprite == vida3)
        {
            vidasRestantes = 3;
        }
        else if (sr.sprite == vida4)
        {
            vidasRestantes = 4;
        }
        else
        {
            vidasRestantes = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bola"))
        {
            ReducirVida();
        }
    }
}
