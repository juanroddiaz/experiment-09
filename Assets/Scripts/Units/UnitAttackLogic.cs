using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackLogic : MonoBehaviour
{
    [SerializeField]
    private List<UnitLogic> _targets = new List<UnitLogic>();

    private UnitLogic _unitLogic;
    
    private int _atk = 0;
    private float _atkSpeed = 0.0f;
    private float _attackCooldown = 0.0f;

    public void Initialize(UnitLogic unitLogic)
    {
        _unitLogic = unitLogic;
        _atk = unitLogic.Config.Atk;
        _atkSpeed = unitLogic.Config.AttackSpeed;
        _attackCooldown = 0.0f;
        _targets.Clear();
    }

    public void OnTargetInRange(UnitLogic target)
    {
        _targets.Add(target);
    }

    public bool OnTargetOutOfRange(UnitLogic target)
    {
        _targets.Remove(target);
        return _targets.Count == 0;
    }

    private void Update()
    {
        if (_targets.Count > 0)
        {
            if (_attackCooldown <= 0.0f)
            {
                Debug.Log("Attack! " + _targets[0].name);
                _attackCooldown = _atkSpeed;
                _targets[0].ReceiveAttack(_atk);
                return;
            }

            _attackCooldown -= Time.deltaTime;
        }
    }
}
