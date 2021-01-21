using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private UnitsGridLogic _team1GridLogic = null;
    [SerializeField]
    private UnitsGridLogic _team2GridLogic = null;
    [SerializeField]
    private GameObject _team1UnitObject = null;
    [SerializeField]
    private GameObject _team2UnitObject = null;
    [SerializeField]
    private int _tilesPerSide = 20;
    [Header("Config")]
    [SerializeField]
    private UnitsCharacteristicConfig _unitsCharacteristicConfig = null;
    [SerializeField]
    private bool _isUnitTestScene = false;

    private UnitFactory _factory = new UnitFactory();

    private void Start()
    {
        _team1GridLogic.Initialize(this);
        _factory.Initialize(_unitsCharacteristicConfig, _team1UnitObject, _team2UnitObject);

        if (_isUnitTestScene)
        {
            _factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(6), UnitTeam.Team1);
            _factory.CreateRandomUnit(_team2GridLogic.GetTileTransform(7), UnitTeam.Team2);
            return;
        }

        for (int i = 0; i < _tilesPerSide; i++)
        {
            _factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(i), UnitTeam.Team1);
        }
    }
}