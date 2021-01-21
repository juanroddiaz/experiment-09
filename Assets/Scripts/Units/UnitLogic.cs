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
    private Transform _areas;
    [SerializeField]
    private TriggerEventLogic _detectionTriggerLogic;
    [SerializeField]
    private TriggerEventLogic _attackTriggerLogic;

    [Header("Visual Feedback")]
    [SerializeField]
    private GameObject _cubeShape;
    [SerializeField]
    private GameObject _sphereShape;
    [SerializeField]
    private float _smallSizeScale = 0.8f;
    [SerializeField]
    private float _bigSizeScale = 1.2f;
    [SerializeField]
    private Animator _animator;

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

    public void Initialize(UnitConfig config, UnitTeam team, Action<UnitLogic> onDeath)
    {
        _config = config;
        Team = team;
        _cubeShape.SetActive(false);
        _sphereShape.SetActive(false);
        _currentHp = _config.Hp;

        Debug.Log(config.ToString());
        Configure();

        _movementLogic = GetComponent<UnitMovementLogic>();
        _movementLogic.Initialize(_config.MovementSpeed);

        _attackLogic = GetComponent<UnitAttackLogic>();
        _attackLogic.Initialize(this);

        _detectionTriggerLogic.gameObject.tag = team.ToString();
        _detectionTriggerLogic.Initialize(new TriggerEventData
        {
            TriggerEnterAction = OnDetectionEnter,
            TriggerExitAction = OnDetectionExit,
        });

        _attackTriggerLogic.gameObject.tag = team.ToString();
        _attackTriggerLogic.Initialize(new TriggerEventData
        {
            TriggerEnterAction = OnAttackEnter,
            TriggerExitAction = OnAttackExit,
        });

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

        _currentShapeObj.SetActive(true);
        _shapeMaterial = _currentShapeObj.GetComponent<MeshRenderer>().material;
        _bodyColliderLogic = _currentShapeObj.GetComponent<CollisionTriggerLogic>();
        _bodyColliderLogic.Initialize(new CollisionTriggerData
        {
            ColliderEnterAction = OnBodyColliderEnter,
            ColliderExitAction = OnBodyColliderExit,
        });
        _areas.SetParent(_currentShapeObj.transform);

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
        _shapeMaterial.color = targetColor;
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
        _movementLogic.OnTargetDetected(t.parent);
    }

    private void OnDetectionExit(Transform t)
    {
        _movementLogic.OnTargetLost(t);
    }

    private void OnAttackEnter(Transform t)
    {
        _movementLogic.CanMove = false;
        _attackLogic.OnTargetInRange(t.parent.GetComponent<UnitLogic>());
    }

    private void OnAttackExit(Transform t)
    {
        OnAttackExit(t.parent.GetComponent<UnitLogic>());
    }

    private void OnAttackExit(UnitLogic unit)
    {
        bool noTargets = _attackLogic.OnTargetOutOfRange(unit);
        if (noTargets)
        {
            _movementLogic.CanMove = true;
        }
    }

    public void OnTargetKilled(UnitLogic logic)
    {
        OnDetectionExit(logic.transform);
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
        if (_currentHp <= 0)
        {
            // DED
            gameObject.SetActive(false);
            _onDeath?.Invoke(this);
        }
    }
}
