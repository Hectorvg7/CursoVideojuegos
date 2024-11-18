using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vidas : MonoBehaviour
{
    public Sprite corazonRelleno;
    public Sprite corazonMedio;
    public Sprite corazonVacio;

    public GameObject corazon1;
    public GameObject corazon2;
    public GameObject corazon3;

    public int vidasRestantes = 6;
    public AudioClip audioVida;

    // Start is called before the first frame update
    void Start()
    {
        ActualizarCorazones();
    }


    public void PerderVida()
    {
        if (vidasRestantes > 0)
        {
            vidasRestantes--;
            ActualizarCorazones();

            if (vidasRestantes == 0)
            {
                GameManager.Instance.MostrarGameOver();
            }
        }
    }

    void ActualizarCorazones(){
        
        // Actualizamos cada corazón según el número de vidas restantes
        if (vidasRestantes == 6)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
        }
        else if (vidasRestantes == 5)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonMedio;
        }
        else if (vidasRestantes == 4)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonVacio;
        }
        else if (vidasRestantes == 3)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonMedio;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonVacio;
        }
        else if (vidasRestantes == 2)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonVacio;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonVacio;
        }
        else if (vidasRestantes == 1)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonMedio;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonVacio;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonVacio;
        }
        else if (vidasRestantes == 0)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonVacio;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonVacio;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonVacio;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bola"))
        {
            PerderVida();
            Destroy(collision.gameObject);
            AudioManager.Instance.PlaySound(audioVida);
            GameManager.Instance.ReiniciarNivel();
        }
    }
}
