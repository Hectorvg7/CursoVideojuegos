using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public static ActionButton Instance { get; private set; }
    private TextMeshProUGUI texto;
    private Button boton;
    public event EventHandler OnActionSelected;

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
            ActionSelected();
        });
    }

    private void ActionSelected()
    {
        OnActionSelected?.Invoke(this, EventArgs.Empty);

    }

}
