using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    [SerializeField]
    private HudGameplayController _hudGameplay;
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
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private Vector2Int _targetScreenSize = new Vector2Int(1920, 1080);
    [Header("Config")]
    [SerializeField]
    private UnitsCharacteristicConfig _unitsCharacteristicConfig = null;
    [SerializeField]
    private bool _isUnitTestScene = false;

    private UnitFactory _factory = new UnitFactory();
    private List<UnitLogic> _team1Units = new List<UnitLogic>();
    private List<UnitLogic> _team2Units = new List<UnitLogic>();
    private float _cameraHeight = 0.0f;

    private void Awake()
    {
        _team1GridLogic.Initialize(this);
        _factory.Initialize(_unitsCharacteristicConfig, _team1UnitObject, _team2UnitObject);
        _cameraHeight = _cameraTransform.position.y;
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
        foreach (var unit in _team1Units)
        {
            Destroy(unit.gameObject);
        }

        foreach (var unit in _team2Units)
        {
            Destroy(unit.gameObject);
        }

        _team1Units.Clear();
        _team2Units.Clear();

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

        _hudGameplay.Initialize(this);
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
                    _hudGameplay.OnTeamWon(UnitTeam.Team2);
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
                    _hudGameplay.OnTeamWon(UnitTeam.Team1);
                }
                break;
        }
    }
}