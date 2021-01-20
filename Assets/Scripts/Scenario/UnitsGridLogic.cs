using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsGridLogic : MonoBehaviour
{
    [SerializeField]
    private ScenarioController _controller;
    [SerializeField]
    private List<Transform> _tiles;
    
    public void Initialize(ScenarioController controller)
    {
        _controller = controller;
    }
}
