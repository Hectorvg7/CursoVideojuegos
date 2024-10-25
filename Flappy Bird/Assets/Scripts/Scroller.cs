using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float velocidadFondo = 2f;
    public float velocidadSuelo = 4f;
    public Transform fondo1;
    public Transform fondo2;
    public Transform suelo1;
    public Transform suelo2;
    private float anchoFondo;
    private float anchoSuelo;
    private List<Transform> tuberias = new List<Transform>();
 

    // Start is called before the first frame update
    void Start()
    {
        anchoFondo = fondo1.GetComponent<SpriteRenderer>().bounds.size.x;
        anchoSuelo = suelo1.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        MoverFondo();
        MoverSuelo();
        MoverTuberias();
    }

    private void MoverFondo()
    {
        fondo1.position += Vector3.left * velocidadFondo * Time.deltaTime;
        fondo2.position += Vector3.left * velocidadFondo * Time.deltaTime;

        if (fondo1.position.x <= -anchoFondo)
        {
            fondo1.position = new Vector3(fondo2.position.x + anchoFondo, 0, 0);
        }

        if (fondo2.position.x <= -anchoFondo)
        {
            fondo2.position = new Vector3(fondo1.position.x + anchoFondo, 0, 0);
        }
    }

    private void MoverSuelo()
    {
        suelo1.position += Vector3.left * velocidadSuelo * Time.deltaTime;
        suelo2.position += Vector3.left * velocidadSuelo * Time.deltaTime;

        if (suelo1.position.x <= -anchoSuelo)
        {
            suelo1.position = new Vector3(suelo2.position.x + anchoSuelo, suelo1.position.y, 0);
        }

        if (suelo2.position.x <= -anchoSuelo)
        {
            suelo2.position = new Vector3(suelo1.position.x + anchoSuelo, suelo2.position.y, 0);
        }
    }

    public void AñadirTuberia(Transform tuberia)
    {
        tuberias.Add(tuberia);
    }

    private void MoverTuberias()
    {
        List<Transform> tuberiasEliminar = new List<Transform>(); 
        
        foreach (Transform tuberia in tuberias)
        {
            tuberia.position += Vector3.left * velocidadSuelo * Time.deltaTime;

            if (tuberia.position.x < -10)
            {
                tuberiasEliminar.Add(tuberia);
            }
        }

        foreach (Transform tuberia in tuberiasEliminar)
        {
            tuberias.Remove(tuberia);
            Destroy(tuberia.gameObject);
        }
    }
}
