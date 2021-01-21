﻿using System.Collections;
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
    [SerializeField]
    private Transform _centerObject;
    [Header("Config")]
    [SerializeField]
    private UnitsCharacteristicConfig _unitsCharacteristicConfig = null;
    [SerializeField]
    private bool _isUnitTestScene = false;

    private UnitFactory _factory = new UnitFactory();
    private List<UnitLogic> _team1Units = new List<UnitLogic>();
    private List<UnitLogic> _team2Units = new List<UnitLogic>();

    private void Start()
    {
        _team1GridLogic.Initialize(this);
        _factory.Initialize(_unitsCharacteristicConfig, _team1UnitObject, _team2UnitObject);

        if (_isUnitTestScene)
        {
            _team1Units.Add(_factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(6), UnitTeam.Team1, OnUnitDeath, _centerObject));
            _team1Units.Add(_factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(5), UnitTeam.Team1, OnUnitDeath, _centerObject));
            _team2Units.Add(_factory.CreateRandomUnit(_team2GridLogic.GetTileTransform(7), UnitTeam.Team2, OnUnitDeath, _centerObject));
            _team2Units.Add(_factory.CreateRandomUnit(_team2GridLogic.GetTileTransform(14), UnitTeam.Team2, OnUnitDeath, _centerObject));
            return;
        }

        for (int i = 0; i < _tilesPerSide; i++)
        {
            _team1Units.Add(_factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(i), UnitTeam.Team1, OnUnitDeath, _centerObject));
            _team2Units.Add(_factory.CreateRandomUnit(_team2GridLogic.GetTileTransform(i), UnitTeam.Team2, OnUnitDeath, _centerObject));
        }
    }

    private void OnUnitDeath(UnitLogic logic)
    {
        switch (logic.Team)
        {
            case UnitTeam.Team1:
                foreach (var unit in _team2Units)
                {
                    unit.OnTargetKilled(logic);
                }
                
                _team1Units.Remove(logic);
                if (_team1Units.Count == 0)
                {
                    Debug.Log("Team 2 won!");
                    return;
                }
                break;
            case UnitTeam.Team2:
                foreach (var unit in _team1Units)
                {
                    unit.OnTargetKilled(logic);
                }
                _team2Units.Remove(logic);
                if (_team2Units.Count == 0)
                {
                    Debug.Log("Team 1 won!");
                    return;
                }
                break;
        }
    }
}