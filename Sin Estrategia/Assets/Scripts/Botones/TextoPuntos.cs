using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextoPuntos : MonoBehaviour
{
    TextMeshProUGUI textoPuntos;
    public UnitsController unitsController;
    private int puntosUnit;

    void Awake()
    {
        textoPuntos = GetComponent<TextMeshProUGUI>();
        textoPuntos.text = "";
    }

    void Update()
    {
      if (unitsController.selectedUnit != null)
      {
        puntosUnit = unitsController.selectedUnit.actionPoints;
        textoPuntos.text = "Action Points: " + puntosUnit;
      }
      else
      {
        textoPuntos.text = "";
      }
    }
}
