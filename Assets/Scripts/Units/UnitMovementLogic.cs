﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementLogic : MonoBehaviour
{
    [SerializeField]
    private Vector3 _movementNormalVector = Vector3.forward;
    private float _movementSpeed = 0.0f;

    public bool CanMove = false;

    public void Initialize(float speed)
    {
        _movementSpeed = speed;
        CanMove = true;
    }

    public void OnTargetDetected(Transform t)
    { 
    }

    public void OnTargetLost(Transform t)
    {
    }

    private void Update()
    {
        if (!CanMove)
        {
            return;
        }

        transform.position += _movementNormalVector * _movementSpeed * Time.deltaTime;
    }

}
