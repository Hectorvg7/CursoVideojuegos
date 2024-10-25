using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pausa;

    // Start is called before the first frame update
    void Start()
    {
        pausa.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PausarJuego();
        }
    }

    void PausarJuego()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausa.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausa.SetActive(false);
        }
    }


}
