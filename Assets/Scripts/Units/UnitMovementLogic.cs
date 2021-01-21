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
    private Vector3 _currentDirection = Vector3.zero;
    private Rigidbody _rb;

    public void Initialize(float speed, Rigidbody rigidbody)
    {
        _movementSpeed = speed;
        _rb = rigidbody;
        CanMove = true;
        _currentDirection = transform.forward.normalized;
    }

    public void OnTogglePauseMovement(bool toggle)
    {
        CanMove = toggle;
        _rb.velocity = Vector3.zero;
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

    private void FixedUpdate()
    {
        if (!CanMove)
        {
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

        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation,
        Quaternion.LookRotation(_currentDirection), 200.0f * Time.deltaTime));
         Vector3 nextPosition = transform.position + _currentDirection * _movementSpeed * Time.fixedDeltaTime;
         _rb.MovePosition(nextPosition);
        transform.position = _rb.position;
        transform.rotation = _rb.rotation;
    }
}
