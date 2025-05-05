using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotonTurno : MonoBehaviour
{
    public TextMeshProUGUI textoTurno;
    private int turnoActual;
    [SerializeField] UnitsController unitsController;

    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
    }


    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateTurnText();

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            // Reiniciar puntos de acción a todas las unidades del jugador
            foreach (Unit unit in UnitsController.Instance.GetUnitsList())
            {
                unit.ResetActionPoints(); // Suponiendo que este método ya existe en Unit
            }

            // Limpiar selección y UI del turno anterior
            unitsController.DeselectUnit();
            unitsController.BorrarQuads();
        }
    }

    private void UpdateTurnText()
    {
        string turnoJugador = TurnSystem.Instance.IsPlayerTurn() ? "Player" : "Enemy";
        textoTurno.text = "TURN: " + TurnSystem.Instance.GetTurnNumber() + " - " + turnoJugador;
    }

    public void AcabarTurno()
    {
        TurnSystem.Instance.NextTurn(); // Ahora delegamos al TurnSystem
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
    }
}
