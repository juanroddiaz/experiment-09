using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackLogic : MonoBehaviour
{
    private int _atk = 0;
    private float _atkSpeed = 0.0f;

    public void Initialize(int atk, float atkSpeed)
    {
        _atk = atk;
        _atkSpeed = atkSpeed;        
    }

    public void OnTargetInRange(Transform t)
    { 

    }

    public void OnTargetOutOfRange(Transform t)
    {

    }
}
