using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vidas : MonoBehaviour
{
    public Sprite corazonRelleno;
    public Sprite corazonVacio;

    public GameObject corazon1;
    public GameObject corazon2;
    public GameObject corazon3;

    public int vidasRestantes = 3;
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
                Debug.Log("Perdiste");
            }
        }
    }

    void ActualizarCorazones(){
        
        // Actualizamos cada corazón según el número de vidas restantes
        if (vidasRestantes == 3)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
        }
        else if (vidasRestantes == 2)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon2.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
            corazon3.GetComponent<SpriteRenderer>().sprite = corazonVacio;
        }
        else if (vidasRestantes == 1)
        {
            corazon1.GetComponent<SpriteRenderer>().sprite = corazonRelleno;
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
            Debug.Log("Menos 1 vida");
            AudioManager.Instance.PlaySound(audioVida);
            GameManager.Instance.ReiniciarNivel();
        }
    }
}
