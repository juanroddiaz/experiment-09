
public class UnitFactory
{
    private UnitsCharacteristicConfig _config;
    private bool _initialized = false;

    public void Initialize(UnitsCharacteristicConfig config)
    {
        _config = config;
        _initialized = true;
    }

    public UnitLogic CreateRandomUnit()
    {
        if (!_initialized)
        {
            return null;
        }

        return null;
    }
}
