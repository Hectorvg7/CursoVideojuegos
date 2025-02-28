using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsController : MonoBehaviour
{
    public static UnitsController Instance { get; private set; }
    
    private BaseAction selectedAction;
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }
}
