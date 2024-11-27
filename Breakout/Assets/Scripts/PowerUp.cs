using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    

    public GameObject bola;
    private Vidas vidas;
    

    [SerializeField] float incremento = 0.25f;
    public bool powerUp1;
    public bool powerUp2;
    public bool powerUp3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && powerUp1)
        {
            BolaExtra();
            Destroy(gameObject);
        }
        else if (collider.gameObject.CompareTag("Player") && powerUp2)
        {
            AumentarPala(collider.gameObject);
            Destroy(gameObject);
        }
        else if (collider.gameObject.CompareTag("Player") && powerUp3)
        {
            AumentarVida();
            Destroy(gameObject);
        }

        if (collider.gameObject.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }

    void AumentarPala(GameObject pala)
    {
        float tamanoMax = 2f;
        Transform transformPala = pala.transform;
        transformPala.localScale = new Vector3(transformPala.localScale.x + incremento, transformPala.localScale.y, transformPala.localScale.z);

        if (transformPala.localScale.x > tamanoMax)
        {
            transformPala.localScale = new Vector3(tamanoMax, transformPala.localScale.y, transformPala.localScale.z);
        }
    }

    void BolaExtra()
    {
        bola = Instantiate(bola);
        bola.GetComponent<Bola>().LanzarBola();
        GameManager.Instance.bola = bola;
        GameManager.Instance.bolasActivas++;
        Debug.Log("Bolas Activas: " + GameManager.Instance.bolasActivas);
    }

    void AumentarVida()
    {
        if (GameManager.Instance.numeroVidas < 6)
        {
            vidas = FindObjectOfType<Vidas>();
            Debug.Log("Tus vidas antes eran: " + vidas.vidasRestantes);
            vidas.vidasRestantes++;
            Debug.Log("Tus vidas son: " + vidas.vidasRestantes);
            vidas.ActualizarCorazones();
            GameManager.Instance.numeroVidas++;
        }
    }
}
