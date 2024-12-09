using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bloques : MonoBehaviour
{
    [SerializeField] float powerChance = 0.2f;
    public int vidasRestantes;
    public Sprite vida1;
    public Sprite vida2;
    public Sprite vida3;
    public Sprite vida4;
    public GameObject[] powerUps;
    public AudioClip audioBloque;
    public AudioClip audioDestroy;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            AsignarVidaInicial();
        }
    }

    void Update()
    {
        ActualizarSprite();
    }

    public void ReducirVida()
    {
        vidasRestantes--;
        ActualizarSprite();
        if (vidasRestantes > 0)
        {
            AudioManager.Instance.PlaySound(audioBloque);
        }
        else if (vidasRestantes <= 0)
        {
            SoltarPowerUp();
            AudioManager.Instance.PlaySound(audioDestroy);
            Destroy(gameObject);
            GameManager.Instance.BloqueDestruido();
        }
    }

    private void SoltarPowerUp()
    {
        if (Random.value <= powerChance)
        {
            int randomIndex = Random.Range(0, powerUps.Length);

            Instantiate(powerUps[randomIndex], transform.position, Quaternion.identity);
        }
        
        
    }

    public void ActualizarSprite()
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
