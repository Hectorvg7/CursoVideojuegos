using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonLuces : MonoBehaviour
{
    public GameObject luz1;
    public GameObject luz2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EncenderLuz()
    {
        luz1.SetActive(true);
        luz2.SetActive(true);
    }
}
