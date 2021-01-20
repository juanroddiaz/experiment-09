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

[CreateAssetMenu(menuName = "My Assets/Unit Config")]
public class UnitConfig : ScriptableObject
{
    public string Id;
    public int Hp = 100;
    public int Atk = 10;
    public UnitShape Shape;
    public UnitSize Size;
    public UnitColour Colour;
}
