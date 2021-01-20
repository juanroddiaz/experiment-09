using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _cubeShape;
    [SerializeField]
    private GameObject _sphereShape;

    private UnitConfig _config;
    private float _movementSpeed = 0.0f;
    private float _attacksPerSecond = 0.0f;

    public void Initialize(UnitConfig config, float movementSpeed, float attacksPerSecond)
    {
        _config = config;
        _movementSpeed = movementSpeed;
        _attacksPerSecond = attacksPerSecond;
    }
}
