using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _cubeShape;
    [SerializeField]
    private GameObject _sphereShape;
    [SerializeField]
    private float _smallSizeScale = 0.8f;
    [SerializeField]
    private float _bigSizeScale = 1.2f;

    private UnitConfig _config;
    private Material _shapeMaterial;

    public void Initialize(UnitConfig config)
    {
        _config = config;

        Debug.Log(config.ToString());
        ConfigureVisuals();
    }

    private void ConfigureVisuals()
    {
        switch (_config.Size)
        {
            case UnitSize.Big:
                transform.localScale *= _bigSizeScale;
                break;
            case UnitSize.Small:
                transform.localScale *= _smallSizeScale;
                break;
        }

        bool useCubeModel = _config.Shape == UnitShape.Cube;
        _cubeShape.SetActive(useCubeModel);
        _sphereShape.SetActive(!useCubeModel);

        _shapeMaterial = useCubeModel ? _cubeShape.GetComponent<MeshRenderer>().material : _sphereShape.GetComponent<MeshRenderer>().material;
        Color targetColor = Color.white;
        switch(_config.Colour)
        {
            case UnitColour.Blue:
                targetColor = Color.blue;
                break;
            case UnitColour.Green:
                targetColor = Color.green;
                break;
            case UnitColour.Yellow:
                targetColor = Color.yellow;
                break;
            case UnitColour.Red:
                targetColor = Color.red;
                break;
        }
        _shapeMaterial.color = targetColor;
    }
}
