using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerarBoton : MonoBehaviour
{
    TextMeshProUGUI textoBoton;
    Button boton;
    
    void Awake()
    {
        boton = GetComponent<Button>();
        textoBoton = boton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        textoBoton.text = baseAction.GetActionName().ToUpper();
        boton.onClick.AddListener(() => 
        {
            UnitsController.Instance.SetSelectedAction(baseAction);
        });
    }

}
