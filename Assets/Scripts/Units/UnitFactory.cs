using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitMainStats
{
    Hp = 0,
    Atk,
}

public enum UnitShape
{
    Cube = 0,
    Sphere,
}
public enum UnitSize
{
    Big = 0,
    Small,
}

public enum UnitColour
{
    Blue = 0,
    Yellow,
    Green,
    Red,
}

public class UnitConfig
{
    public string Id;
    public int Hp;
    public int Atk;
    public UnitShape Shape;
    public UnitSize Size;
    public UnitColour Colour;
    public float MovementSpeed;
    public float AttackSpeed;

    public override string ToString()
    {
        return "Id: " + Id + ", hp: " + Hp + ", atk: " + Atk + ", Shape: " + Shape + ", Size: " + Size + ", Colour: " + Colour + ", movementSpd: " + MovementSpeed + ", atkSpd: " + AttackSpeed;
    }
}

public class UnitFactory
{
    private UnitsCharacteristicConfig _config;
    private GameObject _team1Unit = null;
    private GameObject _team2Unit = null;
    private bool _initialized = false;
    private int _unitIndex = 0;

    public void Initialize(UnitsCharacteristicConfig config, GameObject team1Unit, GameObject team2Unit)
    {
        _config = config;
        _team1Unit = team1Unit;
        _team2Unit = team2Unit;
        _initialized = true;
    }

    public UnitLogic CreateRandomUnit(Transform parent, UnitTeam team, Action<UnitLogic> onDeath)
    {
        if (!_initialized)
        {
            Debug.LogError("Not initialized!");
            return null;
        }

        var unitConfig = GetRandomUnitConfig();       
        var unit = UnityEngine.Object.Instantiate(team == UnitTeam.Team1 ? _team1Unit : _team2Unit, parent);
        UnitLogic logic = unit.GetComponent<UnitLogic>();
        unit.transform.forward *= team == UnitTeam.Team1 ? 1.0f : -1.0f;
        logic.Initialize(unitConfig, team, onDeath);
        return logic;
    }

    private UnitConfig GetRandomUnitConfig()
    {
        UnitConfig unitConfig = new UnitConfig
        {
            Id = "RandomUnit_" + _unitIndex,
            Hp = _config.BaseHp,
            Atk = _config.BaseAtk,
            Shape = UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f ? UnitShape.Cube : UnitShape.Sphere,
            Size = UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f ? UnitSize.Big : UnitSize.Small,
            Colour = GetRandomUnitColour(),
        };

        ShapeModifier shapeMod = _config.ShapeModifiersList.Find(m => m.Shape == unitConfig.Shape);
        if (shapeMod != null)
        {
            UpdateStats(shapeMod.Modifiers, ref unitConfig);
        }

        SizeModifier sizeMod = _config.SizeModifiersList.Find(m => m.Size == unitConfig.Size);
        if (sizeMod != null)
        {
            UpdateStats(sizeMod.Modifiers, ref unitConfig);
        }

        ShapeAndColourModifier shapeColourModifier = _config.ShapeAndColourModifiersList.Find(m => m.Shape == unitConfig.Shape && m.ColourModifier.Colour == unitConfig.Colour);
        if (shapeColourModifier != null)
        {
            UpdateStats(shapeColourModifier.ColourModifier.Modifiers, ref unitConfig);
        }

        unitConfig.MovementSpeed = GetMovementSpeed(UnitMainStats.Hp, unitConfig.Hp);
        unitConfig.AttackSpeed = GetAttackSpeed(UnitMainStats.Atk, unitConfig.Atk);

        _unitIndex++;
        return unitConfig;
    }

    private UnitColour GetRandomUnitColour()
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        if (random < 0.25f)
        {
            return UnitColour.Blue;
        }

        if (random < 0.5f)
        {
            return UnitColour.Green;
        }

        if (random < 0.75f)
        {
            return UnitColour.Yellow;
        }

        return UnitColour.Red;
    }

    private void UpdateStats(List<CharacteristicModifier> modifiers, ref UnitConfig unitConfig)
    {
        foreach (var mod in modifiers)
        {
            switch (mod.Stat)
            {
                case UnitMainStats.Hp:
                    unitConfig.Hp += mod.Mod;
                    break;
                case UnitMainStats.Atk:
                    unitConfig.Atk += mod.Mod;
                    break;
            }
        }
    }

    private float GetMovementSpeed(UnitMainStats stat, int currentValue)
    {
        SpeedModifier mod = _config.MovementSpeedModifiers.Find(m => m.MainStat == stat);
        return GetSpeedModifierInterpolation(mod, currentValue);
    }

    private float GetAttackSpeed(UnitMainStats stat, int currentValue)
    {
        SpeedModifier mod = _config.AttackSpeedModifiers.Find(m => m.MainStat == stat);
        return GetSpeedModifierInterpolation(mod, currentValue);
    }

    private float GetSpeedModifierInterpolation(SpeedModifier mod, int currentValue)
    {
        if (mod == null)
        {
            return 0;
        }

        if (currentValue <= mod.MinimumCap.x)
        {
            return mod.MinimumCap.y;
        }

        if (currentValue >= mod.MaximumCap.x)
        {
            return mod.MaximumCap.y;
        }

        int statInterval = mod.MaximumCap.x - mod.MinimumCap.x;
        int diff = currentValue - mod.MinimumCap.x;
        float percentage = diff / (float) statInterval;
        int speedInterval = mod.MaximumCap.y - mod.MinimumCap.y;
        float speed = mod.MinimumCap.y + (speedInterval * percentage);
        return speed;
    }
}
