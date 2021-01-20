using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacteristicModifier
{
    public UnitMainStats Stat;
    public int Mod;
}

[Serializable]
public class ShapeModifier
{
    public UnitShape Shape;
    public List<CharacteristicModifier> Modifiers;
}

[Serializable]
public class SizeModifier
{
    public UnitSize Size;
    public List<CharacteristicModifier> Modifiers;
}

[Serializable]
public class ColourModifier
{
    public UnitColour Colour;
    public List<CharacteristicModifier> Modifiers;
}

[Serializable]
public class ShapeAndColourModifier
{
    public UnitShape Shape;
    public ColourModifier ColourModifier;
}

[Serializable]
public class SpeedModifier
{
    public UnitMainStats MainStat;
    public Vector2Int MinimumCap;
    public Vector2Int MaximumCap;
}

[CreateAssetMenu(menuName = "My Assets/Units Characteristic Config")]
public class UnitsCharacteristicConfig : ScriptableObject
{
    public int BaseHp = 100;
    public int BaseAtk = 10;
    public List<ShapeModifier> ShapeModifiersList = new List<ShapeModifier>();
    public List<SizeModifier> SizeModifiersList = new List<SizeModifier>();
    public List<ShapeAndColourModifier> ShapeAndColourModifiersList = new List<ShapeAndColourModifier>();
    [Header("Movement Speed")]
    public List<SpeedModifier> MovementSpeedModifiers;
    [Header("Attack Speed")]
    public List<SpeedModifier> AttackSpeedModifiers;
}
