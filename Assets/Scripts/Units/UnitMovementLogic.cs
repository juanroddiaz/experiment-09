using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementLogic : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 0.0f;
    [SerializeField]
    private List<Transform> _targets = new List<Transform>();

    public bool CanMove = false;
    private float _onPauseTimeout = 1.0f;
    private float _onPauseTimer = 0.0f;
    private Vector3 _currentDirection = Vector3.zero;

    public void Initialize(float speed)
    {
        _movementSpeed = speed;
        CanMove = true;
        _currentDirection = transform.forward.normalized;
    }

    public void OnTogglePauseMovement(bool toggle)
    {
        CanMove = toggle;
    }

    public void OnTargetDetected(Transform t)
    {
        _targets.Add(t);
        _targets.Sort(TargetSortingByDistance);
    }

    public void OnTargetLost(Transform t)
    {
        _targets.Remove(t);
        _targets.Sort(TargetSortingByDistance);
    }

    private int TargetSortingByDistance(Transform x, Transform y)
    {
        float distanceX = Vector3.Distance(transform.position, x.position);
        float distanceY = Vector3.Distance(transform.position, y.position);
        if (distanceX < distanceY)
        {
            return -1;
        }
        if (distanceX > distanceY)
        {
            return 1;
        }
        return 0;
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
        Quaternion.LookRotation(_currentDirection), 200.0f * Time.deltaTime);

        if (!CanMove)
        {
            _onPauseTimer = _onPauseTimeout;
            return;
        }

        if (_onPauseTimer < _onPauseTimeout)
        {
            _onPauseTimer += Time.deltaTime;
            return;
        }

        if (_targets.Count > 0)
        {
            _currentDirection = Vector3.Normalize(_targets[0].position - transform.position);
        }
        else
        {
            _currentDirection = transform.forward;
        }

        transform.position += _currentDirection * _movementSpeed * Time.deltaTime;
    }
}
