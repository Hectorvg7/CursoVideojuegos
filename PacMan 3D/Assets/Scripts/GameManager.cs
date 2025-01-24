using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

private GameObject[] dots;
private int dotsRestantes;



    // Update is called once per frame
    void Update()
    {
        dots = GameObject.FindGameObjectsWithTag("Dot");
        dotsRestantes = dots.Length;


        if (dotsRestantes <= 0)
        {
            //Código para WIN
            Debug.Log("¡Has ganado!");
        }
    }
}
