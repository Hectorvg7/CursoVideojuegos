using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject tuberias;
    public float tiempoSpawn;
    private float tiempoUltimoSpawn;
    public float rangoYMin = 0;
    public float rangoYMax = -5;
    public float huecoMinimo =  0.5f;
    public float huecoMaximo = 3f;

    // Start is called before the first frame update
    void Start()
    {
        tiempoUltimoSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        tiempoSpawn = Random.Range(1.5f, 5f);
        tiempoUltimoSpawn += Time.deltaTime;

        if (tiempoUltimoSpawn >= tiempoSpawn)
        {
            SpawnTuberia();
            tiempoUltimoSpawn = 0f;
        }
    }

    public void SpawnTuberia()
    {
        float alturaSuperior = Random.Range(rangoYMin, rangoYMax);
        float alturaInferior = alturaSuperior - Random.Range(huecoMinimo, huecoMaximo);
        
        Vector2 posicion = new Vector2(10, alturaSuperior);
        Transform nuevaTuberia = Instantiate(tuberias, posicion, Quaternion.identity).transform;
        FindObjectOfType<Scroller>().AñadirTuberia(nuevaTuberia);

        Transform parteInferior = nuevaTuberia.transform.Find("PipeDown");
        parteInferior.position = new Vector2(nuevaTuberia.transform.position.x, alturaInferior);
    }
}
