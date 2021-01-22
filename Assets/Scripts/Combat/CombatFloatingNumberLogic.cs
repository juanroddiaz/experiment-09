using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public enum CombatMessageType
{
    Normal = 0,
    CritHit,
    Healing,
    Lethal,
}

[System.Serializable]
public class CombatMessageColors
{
    public CombatMessageType Type;
    public Color MessageColor;
    public Sprite Icon;
}

public class CombatFloatingNumberLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _content = null;
    [SerializeField] 
    private TextMeshProUGUI _valueTxt = null;
    [SerializeField] 
    private Animation _animation = null;
    [SerializeField] 
    private Vector2 _randomPositionOffset = Vector2.zero;
    [SerializeField]
    private float _cameraZAxisOffset = 15.0f;

    [Header("Message configuration")]
    [SerializeField] private List<CombatMessageColors> _messageColorConfig = new List<CombatMessageColors>();

    private List<string> _animNames = new List<string>();
    private Transform _targetTransform;
    private Vector3 _targetPosition = Vector3.zero;

    public void Initialize(HudGameplayController hud, Transform target, int maxHp)
    {
        if (_animNames.Count == 0)
        {
            foreach (AnimationState a in _animation)
            {
                _animNames.Add(a.name);
            }
        }
        if (_animation.GetClipCount() != 3 || _animNames.Count != 3)
        {
            Debug.LogError("CombatFlyingText must have 3 anims!!");
        }

        transform.SetParent(hud.GetGameUI().transform);
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        _content.localScale = Vector3.one;
        _valueTxt.text = "Hp: " + maxHp;
        _targetTransform = target;
    }

    public float Trigger(string value, CombatMessageType type, bool toTheRight)
    {     
        UpdatePosition();

        _valueTxt.text = value;
        _valueTxt.color = _messageColorConfig[(int)type].MessageColor;
        int animIdx = -1;
        switch (type)
        {
            case CombatMessageType.Healing:
                animIdx = 2;
                break;
            case CombatMessageType.Normal:
            case CombatMessageType.Lethal:
            case CombatMessageType.CritHit:
                transform.position += new Vector3(Random.Range(-_randomPositionOffset.x, _randomPositionOffset.x),
                    Random.Range(-_randomPositionOffset.y, _randomPositionOffset.y),
                    0.0f);
                animIdx = toTheRight ? 0 : 1;
                break;
        }

        _animation.Play(_animNames[animIdx]);

        return _animation.GetClip(_animNames[animIdx]).length;
    }

    private void UpdatePosition()
    {
        _targetPosition = Camera.main.WorldToScreenPoint(_targetTransform.position);
        _targetPosition.y += _cameraZAxisOffset;
        transform.position = _targetPosition;
    }

    void Update()
    {
        UpdatePosition();
    }
}
