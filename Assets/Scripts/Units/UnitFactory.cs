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
    public int Hp = 100;
    public int Atk = 10;
    public UnitShape Shape;
    public UnitSize Size;
    public UnitColour Colour;
}

public class UnitFactory
{
    private UnitsCharacteristicConfig _config;
    private GameObject _baseUnit = null;
    private bool _initialized = false;

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

        var unit = Object.Instantiate(_baseUnit);
        UnitLogic logic = unit.GetComponent<UnitLogic>();
        
        

        return null;
    }

    private float GetMovementSpeed()
    {
        return 0.0f;
    }
}
