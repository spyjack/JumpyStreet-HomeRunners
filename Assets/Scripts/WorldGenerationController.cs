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
    private int guaranteedPaths = 2;

    [SerializeField]
    private int maxChunks = 12;

    [SerializeField]
    private List<Transform> worldChunks = null;

    private int[] pathEndsX;

    [SerializeField]
    private float clearDistance = 24;

    [SerializeField]
    private Transform focusTransform = null;

    [SerializeField]
    private RowType[] worldRows = null;

    [SerializeField]
    private int[,] obstacleGrid = null;

    [SerializeField]
    private RowRarity rowRarities = new RowRarity();

    [SerializeField]
    private Transform[] groundPrefabs = null;

    [SerializeField]
    private Transform[] treePrefabs = null;

    [SerializeField]
    private Transform[] waterPrefabs = null;

    [SerializeField]
    private Transform[] vehiclePrefabs = null;

    [SerializeField]
    private Transform pathMarker = null;

    [SerializeField]
    private Transform spawnPoint = null;

    bool playerSpawnChunk = false;

    private int chunkIndex = 0;

    private FlowingRowHandler prevRowFlowHandler = null;

    public Transform GetSpawnPoint
    {
        get {
            if(spawnPoint==null)
            {
                Debug.LogError("Spawnpoint is null!");
            }
            return spawnPoint;
        }
    }

    public float GetWorldWidth
    {
        get { return (float)worldWidthMax; }
    }

    public bool debugMode = false;
    // Start is called before the first frame update
    void Start()
    {
        pathEndsX = new int[guaranteedPaths];
        if (focusTransform == null) { focusTransform = this.transform; }
        for(int i = 0; i<maxChunks;i++)
        {
            if (i == 1) { playerSpawnChunk = true; } else { playerSpawnChunk = false; }
            InitializeWorld(guaranteedPaths);
        }
        PullWorld(new Vector3(0, 0, -worldHeightMax*2));
        GameObject.FindGameObjectWithTag("Player").transform.position = spawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && debugMode)
        {
            InitializeWorld(guaranteedPaths);
        }
        if(Input.GetAxis("Vertical") > 0 && debugMode)
        {
            PullWorld(new Vector3(0, 0, -0.25f));
        }
        if(Input.GetKeyDown(KeyCode.L) && debugMode)
        {
            StopAllCoroutines();
            StartCoroutine(GenerateTimer());
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///Used to move the terrain in any direction, it instantly snaps it so use lerp and such in other scripts to make it smooth!!
    public void PullWorld(Vector3 _direction)
    {
        for (int i = worldChunks.Count-1; i >= 0; i--)
        {
            worldChunks[i].position += _direction;
            if(worldChunks[i].position.z <= focusTransform.position.z - (clearDistance + worldHeightMax))
            {
                Transform newChunk = InitializeWorld(guaranteedPaths);
                if(worldChunks[0] != newChunk)
                {
                    newChunk.position += _direction;
                }
            }
        }
    }

    //Physically generates the world based on the 2 arrays, the terrain array and the obstacles array.
    Transform InitializeWorld(int _paths)
    {
        worldRows = new RowType[worldHeightMax];
        obstacleGrid = new int[worldHeightMax, worldWidthMax];

        GenerateRows();

        for (int i = 0; i < _paths; i++)
        {
            if (playerSpawnChunk)
            {
                CreatePath(4, (obstacleGrid.GetLength(1) - 1) / 2, 0);
            }
            else if (pathEndsX == null || i > pathEndsX.Length-1)
            {
                CreatePath(4, (obstacleGrid.GetLength(1) - 1) / Random.Range(1, 8), 0);
            }else
            {
                CreatePath(4, pathEndsX[i], 0);
            }
        }

        Transform chunk = CreateChunk();
        Debug.Log("Creating New Chunk at " + chunk.position);
        PopulateRows(chunk);
        chunk.position = new Vector3(chunk.position.x, chunk.position.y, GetChunkZ(chunk));
        SetSpawnPoint(chunk);

        return chunk;
    }

    //Fills out the row and obstacle arrays, doesn't generate anything in the scene.
    void GenerateRows()
    {
        worldRows[0] = RowType.grass;
        for (int r = 1; r < worldRows.Length; r++)
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

    //Physically creates the terrain and the obstacles based on the arrays.
    void PopulateRows(Transform _chunk)
    {
        for (int r = 0; r < worldRows.Length; r++)
        {
            Transform lane = GameObject.Instantiate(GetGroundObject(worldRows[r]), new Vector3(0,0,r), Quaternion.identity, _chunk);
            lane.localScale = new Vector3(worldWidthMax*1.5f, lane.localScale.y, 1);

            for (int c = 0; c < obstacleGrid.GetLength(1); c++)
            {
                if ((worldRows[r] == RowType.grass || worldRows[r] == RowType.forest) && obstacleGrid[r,c] == 1)
                {
                    Transform tree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Length-1)], new Vector3(c - (float)worldWidthMax/2 + 0.5f, 0.5f, r), Quaternion.identity, _chunk);
                    tree.localScale = new Vector3(tree.localScale.x, Random.Range(1f, 2.5f), tree.localScale.z);
                }else if(worldRows[r] == RowType.riverPads && obstacleGrid[r, c] == 1)
                {
                    Instantiate(waterPrefabs[0], new Vector3(c - (float)worldWidthMax / 2 + 0.5f, 0.25f, r), Quaternion.identity, _chunk);
                }
            }

            if (worldRows[r] == RowType.river)
            {
                //Determine river direction
                //Configure logs to spawn
                FlowingRowHandler newFlowingRow = Instantiate(waterPrefabs[1], _chunk.position, Quaternion.Euler(0,0,0), _chunk).GetComponent<FlowingRowHandler>();
                newFlowingRow.name = newFlowingRow.name + "_inst_" + r;
                newFlowingRow.transform.position = new Vector3(_chunk.position.x + (float)worldWidthMax / 2 + 1, _chunk.position.y, _chunk.position.z + r);

                int riverDir = Random.Range(0, 2);
                if (worldRows[Mathf.Max(0, r - 1)] == RowType.river && prevRowFlowHandler != null)
                {
                    riverDir = prevRowFlowHandler.Direction;
                }
                Transform leftBounds = new GameObject("Left Boundry").transform;
                leftBounds.position = new Vector3(_chunk.position.x + (float)worldWidthMax / 2 + 1, _chunk.position.y, _chunk.position.z + r);
                Transform rightBounds = new GameObject("Right Boundry").transform;
                rightBounds.position = new Vector3(_chunk.position.x - (float)worldWidthMax / 2 - 1, _chunk.position.y, _chunk.position.z + r);

                rightBounds.parent = newFlowingRow.transform; leftBounds.parent = newFlowingRow.transform;

                newFlowingRow.Configure(
                    leftBounds,
                    rightBounds,
                    1f, 6f,
                    Random.Range(2, 5),
                    Random.Range(3, 6),
                    riverDir
                    );

                newFlowingRow.Run();
                prevRowFlowHandler = newFlowingRow;
            }
            else if (worldRows[r] == RowType.road || worldRows[r] == RowType.street)
            {
                //configure road direction
                //configure road intensity
                FlowingRowHandler newFlowingRow = Instantiate(vehiclePrefabs[0], _chunk.position, Quaternion.identity, _chunk).GetComponent<FlowingRowHandler>();
                newFlowingRow.name = newFlowingRow.name + "_inst_" + r;
                newFlowingRow.transform.position = new Vector3(_chunk.position.x + (float)worldWidthMax / 2 + 1, _chunk.position.y, _chunk.position.z + r);

                int roadDir = Random.Range(0, 2);
                if ((worldRows[Mathf.Max(0, r - 1)] == RowType.road) && prevRowFlowHandler != null)
                {
                    roadDir = prevRowFlowHandler.Direction;
                }
                Transform leftBounds = new GameObject("Left Boundry").transform;
                leftBounds.position = new Vector3(_chunk.position.x + (float)worldWidthMax / 2 + 1, _chunk.position.y+0.65f, _chunk.position.z + r);
                Transform rightBounds = new GameObject("Right Boundry").transform;
                rightBounds.position = new Vector3(_chunk.position.x - (float)worldWidthMax / 2 - 1, _chunk.position.y+0.65f, _chunk.position.z + r);

                rightBounds.parent = newFlowingRow.transform; leftBounds.parent = newFlowingRow.transform;

                newFlowingRow.Configure(
                    leftBounds,
                    rightBounds,
                    1f, 4f,
                    Random.Range(2, 10),
                    Random.Range(3, 6),
                    roadDir
                    );

                newFlowingRow.Run();
                prevRowFlowHandler = newFlowingRow;
            }
            else if (worldRows[r] == RowType.railroad)
            {
                //configure direction and intensity
                FlowingRowHandler newFlowingRow = Instantiate(vehiclePrefabs[1], _chunk.position, Quaternion.identity, _chunk).GetComponent<FlowingRowHandler>();
                newFlowingRow.name = newFlowingRow.name + "_inst_" + r;
                newFlowingRow.transform.position = new Vector3(_chunk.position.x + (float)worldWidthMax / 2 + 1, _chunk.position.y, _chunk.position.z + r);

                Transform leftBounds = new GameObject("Left Boundry").transform;
                leftBounds.position = new Vector3(_chunk.position.x + (float)worldWidthMax / 2 + 12, _chunk.position.y + 0.65f, _chunk.position.z + r);
                Transform rightBounds = new GameObject("Right Boundry").transform;
                rightBounds.position = new Vector3(_chunk.position.x - (float)worldWidthMax / 2 - 12, _chunk.position.y + 0.65f, _chunk.position.z + r);

                rightBounds.parent = newFlowingRow.transform; leftBounds.parent = newFlowingRow.transform;

                newFlowingRow.Configure(
                    leftBounds,
                    rightBounds,
                    6f, 10f,
                    6f, 14f,
                    Random.Range(3, 6),
                    true,
                    Random.Range(0,2)
                    );

                newFlowingRow.Run();
                prevRowFlowHandler = newFlowingRow;
            }
        }
    }

    //Creates "drunken walkers" that stumble through the obstacle grid array, and clear any objects they cross so a path is guaranteed.
    void CreatePath(int _vChance, int _gridX, int _gridY)
    {
        //print("Walker starting at: " + _gridX + ", " + _gridY);
        ClearObstacle(_gridX, _gridY);
        while (_gridY < obstacleGrid.GetLength(0)-1)
        {
            if (Random.Range(0,11) < _vChance)
            {
                int randWChance = Random.Range(0, 2);
                if (_gridX < obstacleGrid.GetLength(1)-2 && randWChance == 0)
                {
                    _gridX++;
                    ClearObstacle(_gridX, _gridY);
                }
                else if (_gridX >= 2 && obstacleGrid.GetLength(1) > 1 && randWChance == 1)
                {
                    _gridX--;
                    ClearObstacle(_gridX, _gridY);
                }
            }else
            {
                _gridY++;
                ClearObstacle(_gridX, _gridY);
            }
            //Instantiate(pathMarker, new Vector3(gridX - (float)worldWidthMax / 2 + 0.5f, 3f, _gridY), Quaternion.identity);
            //print("Walker at: " + _gridX + ", " + _gridY);
        }
    }

    //Thisi s what actually clears the objects in the obstacle array grid.
    void ClearObstacle(int _x, int _y)
    {
        //print("Replacing obstacle found at " + __x + ", " + _y);
        if (obstacleGrid[_y, _x] == 1 && (worldRows[_y] == RowType.grass || worldRows[_y] == RowType.forest))
        {
            obstacleGrid[_y, _x] = 0;
            //print("Obstacle found at " + _x + ", " + _y + ". Set to " + obstacleGrid[_y, _x]);
        }else if (worldRows[_y] == RowType.riverPads && obstacleGrid[_y, _x] == 0)
        {
            obstacleGrid[_y, _x] = 1;
        }
    }

    //Initializes the spawnpoint.
    void SetSpawnPoint(Transform spawnTransform)
    {
        if (spawnPoint == null)
        {
            spawnPoint = new GameObject("SpawnPoint").transform;
            spawnPoint.position = new Vector3(spawnTransform.position.x - 0.5f, spawnTransform.position.y + 0.75f, spawnTransform.position.z-worldHeightMax);
            focusTransform = spawnPoint;
        }
    }

    //Creates a new terrain chunk so it can be used.
    Transform CreateChunk()
    {
        GameObject newChunk;
        if(worldChunks.Count<maxChunks)
        {
            newChunk = new GameObject("Chunk_" + (chunkIndex));
            worldChunks.Add(newChunk.transform);
            chunkIndex++;
        }else
        {
            if(chunkIndex >= maxChunks)
            {
                chunkIndex = 0;
            }
            Destroy(worldChunks[chunkIndex].gameObject);
            newChunk = new GameObject("Chunk_" + (chunkIndex));
            worldChunks[chunkIndex] = newChunk.transform;
            chunkIndex++;
        }

        return newChunk.transform;
    }

    //Gets the Z position of a chunk.
    float GetChunkZ(Transform _chunk)
    {
        float zPos = 0;
        int index = worldChunks.IndexOf(_chunk);
        if (index - 1 < 0)
        {
            zPos = worldChunks[worldChunks.Count - 1].position.z + worldHeightMax;
        }else
        {
            zPos = worldChunks[index - 1].position.z + worldHeightMax;
        }
        return zPos;
    }

    //Returns a prefab based on the corresponding RowType.
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
            case RowType.riverPads:
                {
                    return groundPrefabs[3];
                }
            default:
                {
                    return groundPrefabs[0];
                }
        }
    }

    IEnumerator GenerateTimer()
    {
        while(true)
        {
            //InitializeWorld(guaranteedPaths);
            PullWorld(new Vector3(0, 0, -0.1f));
            yield return new WaitForSeconds(0.001f);
        }
    }
}