﻿using System.Collections.Generic;
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
}

public class UnitFactory
{
    private UnitsCharacteristicConfig _config;
    private GameObject _baseUnit = null;
    private bool _initialized = false;
    private int _unitIndex = 0;

    public void Initialize(UnitsCharacteristicConfig config, GameObject baseUnit)
    {
        _config = config;
        _baseUnit = baseUnit;
        _initialized = true;
    }

    public GameObject CreateRandomUnit()
    {
        if (!_initialized)
        {
            return null;
        }

        var unitConfig = GetRandomUnitConfig();       
        var unit = Object.Instantiate(_baseUnit);
        UnitLogic logic = unit.GetComponent<UnitLogic>();
        int movementSpeed = GetMovementSpeed(UnitMainStats.Hp, unitConfig.Hp);
        int attackSpeed = GetAttackSpeed(UnitMainStats.Atk, unitConfig.Atk);
        logic.Initialize(unitConfig, movementSpeed, attackSpeed);
        return unit;
    }

    private UnitConfig GetRandomUnitConfig()
    {
        UnitConfig unitConfig = new UnitConfig
        {
            Id = "RandomUnit_" + _unitIndex,
            Hp = _config.BaseHp,
            Atk = _config.BaseAtk,
            Shape = Random.Range(0.0f, 0.1f) < 0.5f ? UnitShape.Cube : UnitShape.Sphere,
            Size = Random.Range(0.0f, 0.1f) < 0.5f ? UnitSize.Big : UnitSize.Small,
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

        _unitIndex++;
        return unitConfig;
    }

    private UnitColour GetRandomUnitColour()
    {
        float random = Random.Range(0.0f, 1.0f);
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

    private int GetMovementSpeed(UnitMainStats stat, int currentValue)
    {
        SpeedModifier mod = _config.MovementSpeedModifiers.Find(m => m.MainStat == stat);
        return GetSpeedModifierInterpolation(mod, currentValue);
    }

    private int GetAttackSpeed(UnitMainStats stat, int currentValue)
    {
        SpeedModifier mod = _config.AttackSpeedModifiers.Find(m => m.MainStat == stat);
        return GetSpeedModifierInterpolation(mod, currentValue);
    }

    private int GetSpeedModifierInterpolation(SpeedModifier mod, int currentValue)
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
        float percentage = diff / statInterval;
        int speedInterval = mod.MaximumCap.y - mod.MinimumCap.y;
        int speed = Mathf.FloorToInt(speedInterval * percentage);
        return speed;
    }
}