using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationController : MonoBehaviour
{
    [SerializeField]
    private int worldHeightMax = 10;

    [SerializeField]
    private RowType[] worldRows;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeWorld()
    {
        worldRows = new RowType[worldHeightMax];
    }
}

public enum RowType
{
    street,
    road,
    river,
    railroad,
    grass,
    forest
}
