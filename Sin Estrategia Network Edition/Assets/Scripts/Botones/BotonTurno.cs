using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotonTurno : MonoBehaviour
{
    public TextMeshProUGUI textoTurno;
    private int turnoActual;
    [SerializeField] UnitsController unitsController;

    void Awake()
    {
        turnoActual = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (unitsController.selectedUnit != null && unitsController.selectedUnit.actionPoints < 1)
        {
            AcabarTurno();
        }
    }

    public void AcabarTurno()
    {
        turnoActual++;
        textoTurno.text = "TURN: " + turnoActual;
        unitsController.DevolverPuntos(unitsController.selectedUnit);
        unitsController.DeselectUnit();
        unitsController.BorrarQuads();

    }
}
