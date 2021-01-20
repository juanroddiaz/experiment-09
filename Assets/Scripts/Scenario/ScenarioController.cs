using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private UnitsGridLogic _gridLogic;
    [Header("Config")]
    private UnitsCharacteristicConfig _unitsCharacteristicConfig;

    private UnitFactory _factory = new UnitFactory();

    private void Start()
    {
        
    }
}

