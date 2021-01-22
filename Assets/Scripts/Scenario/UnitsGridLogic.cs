using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsGridLogic : MonoBehaviour
{
    [SerializeField]
    private ScenarioController _controller;
    [SerializeField]
    private List<Transform> _tiles = null;
    
    public void Initialize(ScenarioController controller)
    {
        _controller = controller;
    }

    public Transform GetTileTransform(int index)
    {
        if (index < 0 || index >= _tiles.Count)
        {
            Debug.LogError("Wrong Index!");
            return null;
        }

        return _tiles[index];
    }
}
