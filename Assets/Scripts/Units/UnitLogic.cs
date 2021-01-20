using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitTeam
{ 
    Team1 = 0,
    Team2,
}

public class UnitLogic : MonoBehaviour
{
    [SerializeField]
    private CollisionTriggerLogic _detectionColliderLogic;

    [Header("Visual Feedback")]
    [SerializeField]
    private GameObject _cubeShape;
    [SerializeField]
    private GameObject _sphereShape;
    [SerializeField]
    private float _smallSizeScale = 0.8f;
    [SerializeField]
    private float _bigSizeScale = 1.2f;

    private UnitConfig _config;
    private Material _shapeMaterial;
    private GameObject _currentShapeObj;

    private UnitMovementLogic _movementLogic;
    private UnitAttackLogic _attackLogic;

    public void Initialize(UnitConfig config, UnitTeam team)
    {
        _config = config;
        _cubeShape.SetActive(false);
        _sphereShape.SetActive(false);

        Debug.Log(config.ToString());
        Configure();

        _movementLogic = GetComponent<UnitMovementLogic>();
        _movementLogic.Initialize(_config.MovementSpeed);

        _attackLogic = GetComponent<UnitAttackLogic>();

        _detectionColliderLogic.gameObject.tag = team.ToString();
        _detectionColliderLogic.Initialize(new CollisionTriggerData
        {
            ColliderEnterAction = OnColliderEnter,
            ColliderExitAction = OnColliderExit,
        });
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

    private void OnColliderEnter(Transform t)
    { 
    }

    private void OnColliderExit(Transform t)
    {
    }
}
