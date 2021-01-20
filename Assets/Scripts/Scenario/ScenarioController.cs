using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private UnitsGridLogic _gridLogic = null;
    [SerializeField]
    private GameObject _baseUnitObject = null;
    [SerializeField]
    private int _tilesPerSide = 20;
    [Header("Config")]
    [SerializeField]
    private UnitsCharacteristicConfig _unitsCharacteristicConfig = null;

    private UnitFactory _factory = new UnitFactory();

    private void Start()
    {
        _gridLogic.Initialize(this);
        _factory.Initialize(_unitsCharacteristicConfig, _baseUnitObject);
        for (int i = 0; i < _tilesPerSide; i++)
        {
            _factory.CreateRandomUnit(_gridLogic.GetTileTransform(i));
        }
    }
}