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
    public List<CharacteristicModifier> Modifier;
}

[Serializable]
public class SizeModifier
{
    public UnitSize Size;
    public CharacteristicModifier Modifier;
}

[Serializable]
public class ColourModifier
{
    public UnitColour Colour;
    public CharacteristicModifier Modifier;
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
    public CharacteristicModifier CharacteristicMod;
    public float Value;
}


[CreateAssetMenu(menuName = "My Assets/Units Characteristic Config")]
public class UnitsCharacteristicConfig : ScriptableObject
{
    public List<ShapeModifier> ShapeModifiersList = new List<ShapeModifier>();
    public List<SizeModifier> SizeModifiersList = new List<SizeModifier>();
    public List<ShapeAndColourModifier> ShapeAndColourModifiersList = new List<ShapeAndColourModifier>();
    [Header("Movement Speed")]
    public SpeedModifier MinMovementSpeedValue;
    public SpeedModifier MaxMovementSpeedValue;
    [Header("Attack Speed")]
    public SpeedModifier MinAttackSpeedValue;
    public SpeedModifier MaxAttackSpeedValue;

}
