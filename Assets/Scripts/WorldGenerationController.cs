using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationController : MonoBehaviour
{
    [SerializeField]
    private int worldHeightMax = 12;

    [SerializeField]
    private int worldWidthMax = 8;

    [SerializeField]
    private RowType[] worldRows;

    [SerializeField]
    private int[,] obstacleGrid;

    [SerializeField]
    private RowRarity rowRarities = new RowRarity();

    [SerializeField]
    private Transform[] groundPrefabs;

    [SerializeField]
    private Transform obstacleTest;
    // Start is called before the first frame update
    void Start()
    {
        InitializeWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeWorld()
    {
        worldRows = new RowType[worldHeightMax];
        obstacleGrid = new int[worldHeightMax, worldWidthMax];

        GenerateRows();

        PopulateRows();
    }

    void GenerateRows()
    {
        for(int r = 0; r < worldRows.Length; r++)
        {
            worldRows[r] = rowRarities.GetRow(Random.Range(0, 11));
        }

        for (int l = 0; l < obstacleGrid.GetLength(0); l++)
        {
            for (int c = 0; c < obstacleGrid.GetLength(1); c++)
            {
                if (Random.Range(0,101) > 50)
                {
                    obstacleGrid[l, c] = 1;
                }else
                {
                    obstacleGrid[l, c] = 0;
                }
            }
        }
    }

    void PopulateRows()
    {
        for (int r = 0; r < worldRows.Length; r++)
        {
            Transform lane = GameObject.Instantiate(GetGroundObject(worldRows[r]), new Vector3(0,0,r), Quaternion.identity);
            lane.localScale = new Vector3(worldWidthMax, 1, 1);

            for (int c = 0; c < obstacleGrid.GetLength(1); c++)
            {
                if (worldRows[r] == RowType.grass || worldRows[r] == RowType.forest && obstacleGrid[r,c] == 1)
                {
                    Instantiate(obstacleTest, new Vector3(c - (float)worldWidthMax/2 + 0.5f, 0.5f, r), Quaternion.identity);
                }
            }
        }
    }

    Transform GetGroundObject(RowType _rowType)
    {
        switch(_rowType)
        {
            case RowType.forest:
                {
                    return groundPrefabs[0];
                }
            case RowType.grass:
                {
                    return groundPrefabs[0];
                }
            case RowType.street:
                {
                    return groundPrefabs[1];
                }
            case RowType.road:
                {
                    return groundPrefabs[2];
                }
            case RowType.river:
                {
                    return groundPrefabs[3];
                }
            case RowType.railroad:
                {
                    return groundPrefabs[4];
                }
            default:
                {
                    return groundPrefabs[0];
                }
        }
    }
}