using System;
using UnityEngine;

public enum UnitTeam
{ 
    Team1 = 0,
    Team2,
}

public class UnitLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _areas = null;
    [SerializeField]
    private TriggerEventLogic _detectionTriggerLogic = null;
    [SerializeField]
    private TriggerEventLogic _attackTriggerLogic = null;

    [Header("Visual Feedback")]
    [SerializeField]
    private GameObject _cubeShape = null;
    [SerializeField]
    private GameObject _sphereShape = null;
    [SerializeField]
    private float _smallSizeScale = 0.8f;
    [SerializeField]
    private float _bigSizeScale = 1.2f;
    [SerializeField]
    private Animator _animator = null;
    [SerializeField]
    private TMPro.TextMeshPro _teamText = null;
    [SerializeField]
    private CombatFloatingNumberLogic _combatNumber = null;

    private UnitConfig _config;
    private Material _shapeMaterial;
    private GameObject _currentShapeObj;
    private CollisionTriggerLogic _bodyColliderLogic;

    private UnitMovementLogic _movementLogic;
    private UnitAttackLogic _attackLogic;
    private int _currentHp = 0;

    public UnitConfig Config => _config;
    private Action<UnitLogic> _onDeath = null;
    public UnitTeam Team;

    public void Initialize(UnitConfig config, UnitTeam team, Action<UnitLogic> onDeath, Transform center, HudGameplayController hud)
    {
        _config = config;
        Team = team;
        _cubeShape.SetActive(false);
        _sphereShape.SetActive(false);
        _currentHp = _config.Hp;
        _combatNumber.Initialize(hud, transform, _config.Hp);

        Debug.Log(config.ToString());
        Configure();
        InitializeComponents(center);
        _onDeath = onDeath;
    }

    private void Configure()
    {
        switch (_config.Shape)
        {
            case UnitShape.Cube:
                _currentShapeObj = _cubeShape;
                break;
            case UnitShape.Sphere:
                _currentShapeObj = _sphereShape;
                break;
        }

        switch (_config.Size)
        {
            case UnitSize.Big:
                _currentShapeObj.transform.localScale *= _bigSizeScale;
                break;
            case UnitSize.Small:
                _currentShapeObj.transform.localScale *= _smallSizeScale;
                break;
        }

        Color targetColor = Color.white;
        switch(_config.Colour)
        {
            case UnitColour.Blue:
                targetColor = Color.blue;
                break;
            case UnitColour.Green:
                targetColor = Color.green;
                break;
            case UnitColour.Yellow:
                targetColor = Color.yellow;
                break;
            case UnitColour.Red:
                targetColor = Color.red;
                break;
        }

        _shapeMaterial = _currentShapeObj.GetComponent<MeshRenderer>().material;
        _shapeMaterial.color = targetColor;
        _teamText.text = Team == UnitTeam.Team1 ? "1" : "2";
    }

    private void InitializeComponents(Transform center)
    {
        _currentShapeObj.SetActive(true);
        _bodyColliderLogic = GetComponent<CollisionTriggerLogic>();
        _bodyColliderLogic.Initialize(new CollisionTriggerData
        {
            ColliderEnterAction = OnBodyColliderEnter,
            ColliderExitAction = OnBodyColliderExit,
        });
        _areas.SetParent(_currentShapeObj.transform);

        _movementLogic = GetComponent<UnitMovementLogic>();
        _movementLogic.Initialize(_config.MovementSpeed, GetComponent<Rigidbody>(), center);

        _attackLogic = GetComponent<UnitAttackLogic>();
        _attackLogic.Initialize(this);

        _detectionTriggerLogic.Initialize(new TriggerEventData
        {
            TriggerEnterAction = OnDetectionEnter,
            TriggerExitAction = OnDetectionExit,
        });

        _attackTriggerLogic.Initialize(new TriggerEventData
        {
            TriggerEnterAction = OnAttackEnter,
            TriggerExitAction = OnAttackExit,
        });
    }

    public void StartGameplay()
    {
        _movementLogic.GameStarted();
    }

    private void OnBodyColliderEnter(Transform t)
    {
        //_movementLogic.OnTogglePauseMovement(false);
    }

    private void OnBodyColliderExit(Transform t)
    {
        //_movementLogic.OnTogglePauseMovement(true);
    }

    private void OnDetectionEnter(Transform t)
    {
        _movementLogic.OnTargetDetected(t);
    }

    private void OnDetectionExit(Transform t)
    {
        _movementLogic.OnTargetLost(t);
    }

    private void OnDetectionExit(UnitLogic unit)
    {
        _movementLogic.OnTargetLost(unit.transform);
    }

    private void OnAttackEnter(Transform t)
    {
        _movementLogic.OnTogglePauseMovement(false);
        _attackLogic.OnTargetInRange(t.GetComponent<UnitLogic>());
    }

    private void OnAttackExit(Transform t)
    {
        OnAttackExit(t.GetComponent<UnitLogic>());
    }

    private void OnAttackExit(UnitLogic unit)
    {
        bool noTargets = _attackLogic.OnTargetOutOfRange(unit);
        if (noTargets)
        {
            _movementLogic.OnTogglePauseMovement(true);
        }
    }

    public void OnTargetKilled(UnitLogic logic)
    {
        OnDetectionExit(logic);
        OnAttackExit(logic);
    }

    public void OnAttackFeedback()
    {
        _animator.SetTrigger("Attack");
    }

    public void ReceiveAttack(int atkPoints)
    {
        if (_currentHp <= 0)
        { 
            Debug.LogError("Already dead!");
            return;
        }
        
        _currentHp -= atkPoints;
        Debug.Log(name + " suffered damage! Remaining HP: " + _currentHp + ", atk received: " + atkPoints);
        CombatMessageType combatMessage = CombatMessageType.Normal;
        if (_currentHp <= 0)
        {
            // DED
            gameObject.SetActive(false);
            combatMessage = CombatMessageType.Lethal;
            _onDeath?.Invoke(this);
        }
        _combatNumber.Trigger(atkPoints.ToString(), combatMessage, _currentHp % 2 == 0);
    }

    public void Cleanup(bool instant = false)
    {
        Destroy(_combatNumber.gameObject, instant ? 0.0f : 2.0f);
        Destroy(gameObject, instant ? 0.0f : 2.0f);
    }
}
