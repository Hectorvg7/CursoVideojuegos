using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarNiveles : MonoBehaviour
{
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(0);
    }
}
