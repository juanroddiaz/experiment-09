using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTriggerData
{
    public Action<Transform> ColliderEnterAction;
    public Action<Transform> ColliderExitAction;
}

public class CollisionTriggerLogic : MonoBehaviour
{
    private CollisionTriggerData _data;
    private bool _initialized = false;

    public void Initialize(CollisionTriggerData data)
    {
        _data = data;
        _initialized = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("On collision enter: other " + other.gameObject.name + ", collider: " + gameObject.name);
        if (_initialized)
        {
            _data.ColliderEnterAction?.Invoke(other.transform);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log("On collision exit: other " + other.gameObject.name + ", collider: " + gameObject.name);
        if (_initialized)
        {
            _data.ColliderExitAction?.Invoke(other.transform);
        }
    }
}
