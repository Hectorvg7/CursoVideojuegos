using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    private TextMeshProUGUI texto;
    private Button boton;

    void Awake()
    {
        boton = GetComponent<Button>();
        texto = boton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        texto.text = baseAction.GetActionName().ToUpper();
        boton.onClick.AddListener(() => 
        {
            UnitsController.Instance.SetSelectedAction(baseAction);
            /*REVISAR*/ GridVisualizer.Instance.OnDrawGizmos();
        });
    }

}
