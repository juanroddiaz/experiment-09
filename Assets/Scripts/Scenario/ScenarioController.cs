using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private UnitsGridLogic _gridLogic = null;
    [SerializeField]
    private GameObject _baseUnitObject = null;
    [Header("Config")]
    [SerializeField]
    private UnitsCharacteristicConfig _unitsCharacteristicConfig = null;

    private UnitFactory _factory = new UnitFactory();

    private void Start()
    {
        _gridLogic.Initialize(this);
        _factory.Initialize(_unitsCharacteristicConfig, _baseUnitObject);
        _factory.CreateRandomUnit();
        _factory.CreateRandomUnit();
        _factory.CreateRandomUnit();
    }
}