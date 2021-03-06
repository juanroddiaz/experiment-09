﻿using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private HudGameplayController _hudGameplay = null;
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
    private Transform _centerObject = null;
    [SerializeField]
    private Camera _camera = null;
    [SerializeField]
    private Vector2 _targetScreenSize = new Vector2(1920.0f, 1080.0f);
    [Header("Config")]
    [SerializeField]
    private UnitsCharacteristicConfig _unitsCharacteristicConfig = null;
    [SerializeField]
    private bool _isUnitTestScene = false;

    private UnitFactory _factory = new UnitFactory();
    private List<UnitLogic> _team1Units = new List<UnitLogic>();
    private List<UnitLogic> _team2Units = new List<UnitLogic>();

    private void Awake()
    {
        _team1GridLogic.Initialize(this);
        _factory.Initialize(_unitsCharacteristicConfig, _team1UnitObject, _team2UnitObject);
        InitializeScene();
    }

    public void StartLevel()
    {
        for (int i = 0; i < _tilesPerSide; i++)
        {
            _team1Units[i].StartGameplay();
            _team2Units[i].StartGameplay();
        }
    }

    public void TogglePause(bool toggle)
    {
        // cheap but effective
        Time.timeScale = toggle ? 0.0f : 1.0f;
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void InitializeScene()
    {
        float aspectRatio = Screen.width / ((float)Screen.height);
        float percentage = 1 - (aspectRatio / (_targetScreenSize.x / _targetScreenSize.y));
        _camera.rect = new Rect(0f, (percentage / 2), 1f, (1 - percentage));

        foreach (var unit in _team1Units)
        {
            unit.Cleanup(true);
        }

        foreach (var unit in _team2Units)
        {
            unit.Cleanup(true);
        }

        _team1Units.Clear();
        _team2Units.Clear();

        var team1Model = new UnitLogicModel
        {
            Config = null,
            Team = UnitTeam.Team1,
            OnDeathAction = OnUnitDeath,
            AttackCenter = _centerObject,
            Hud = _hudGameplay,
        };

        var team2Model = new UnitLogicModel
        {
            Config = null,
            Team = UnitTeam.Team2,
            OnDeathAction = OnUnitDeath,
            AttackCenter = _centerObject,
            Hud = _hudGameplay,
        };

        if (_isUnitTestScene)
        {
            _team1Units.Add(_factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(6), team1Model));
            _team1Units.Add(_factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(5), team1Model));
            _team2Units.Add(_factory.CreateRandomUnit(_team2GridLogic.GetTileTransform(7), team2Model));
            _team2Units.Add(_factory.CreateRandomUnit(_team2GridLogic.GetTileTransform(14), team2Model));
            return;
        }

        for (int i = 0; i < _tilesPerSide; i++)
        {
            _team1Units.Add(_factory.CreateRandomUnit(_team1GridLogic.GetTileTransform(i), team1Model));
            _team2Units.Add(_factory.CreateRandomUnit(_team2GridLogic.GetTileTransform(i), team2Model));
        }

        _hudGameplay.Initialize(this);
    }

    private void OnUnitDeath(UnitLogic logic, UnitTeam team)
    {
        switch (team)
        {
            case UnitTeam.Team1:
                foreach (var unit in _team2Units)
                {
                    unit.OnTargetKilled(logic);
                }
                
                _team1Units.Remove(logic);
                logic.Cleanup();
                if (_team1Units.Count == 0)
                {
                    _hudGameplay.OnTeamWon(UnitTeam.Team2);
                }
                break;
            case UnitTeam.Team2:
                foreach (var unit in _team1Units)
                {
                    unit.OnTargetKilled(logic);
                }
                _team2Units.Remove(logic);
                logic.Cleanup();
                if (_team2Units.Count == 0)
                {                    
                    _hudGameplay.OnTeamWon(UnitTeam.Team1);
                }
                break;
        }
    }
}