using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowingRowHandler : MonoBehaviour
{
    [SerializeField]
    private List<Transform> assetsToSpawn = new List<Transform>();

    [SerializeField]
    private Transform leftBounds = null;

    [SerializeField]
    private Transform rightBounds = null;

    [SerializeField]
    private float minSpawnTime = 1f;
    [SerializeField]
    private float maxSpawnTime = 1f;

    [SerializeField]
    private float minSpeed = 1f;
    [SerializeField]
    private float maxSpeed = 1f;

    [SerializeField]
    bool directionCanChange = false;
    [SerializeField]
    private int direction = 1;

    [SerializeField]
    private int maxInstances = 3;

    [SerializeField]
    private List<ObjectMover> instances = new List<ObjectMover>();

    bool configured = false;

    public int Direction { get { return direction; } }


    //Create the next moving object
    private void Spawn()
    {
        if(!directionCanChange)
        {
            CreateObject(direction);
        }
        else if(directionCanChange)//Used for trains
        {
            direction = Random.Range(0, 2);
            CreateObject(direction);
        }
    }

    private void CreateObject(int _dir)
    {
        ObjectMover newObject = Instantiate(assetsToSpawn[Random.Range(0, assetsToSpawn.Count)],this.transform).GetComponent<ObjectMover>();
        instances.Add(newObject);
        newObject.myInstanceList = instances;

        if (_dir == 0)
        {
            newObject.direction = new Vector3(-1, 0, 0);
            newObject.transform.position = leftBounds.position;
            newObject.deathPoint = rightBounds;
            newObject.speed = Random.Range(minSpeed, maxSpeed);
            //newObject.transform.Rotate(new Vector3(0, 1, 0), 180);
        }
        else if (_dir == 1)
        {
            newObject.direction = new Vector3(1, 0, 0);
            newObject.transform.position = rightBounds.position;
            newObject.deathPoint = leftBounds;
            newObject.speed = Random.Range(minSpeed, maxSpeed);
            newObject.transform.Rotate(new Vector3(0, 1, 0), 180);
        }
    }

    public void Configure(Transform _leftBounds, Transform _rightBounds, float _minSpawnTime, float _maxSpawnTime, float _speed, int _maxInst, int _startDir)
    {
        SetBounds(_leftBounds, _rightBounds);
        minSpawnTime = _minSpawnTime;           maxSpawnTime = _maxSpawnTime;
        minSpeed = _speed;                      maxSpeed = _speed;
        maxInstances = _maxInst;
        directionCanChange = false;        direction = _startDir;
        configured = true;
    }

    public void Configure(Transform _leftBounds, Transform _rightBounds, float _minSpawnTime, float _maxSpawnTime, float _minSpeed, float _maxSpeed, int _maxInst, bool _changeDir, int _startDir)
    {
        SetBounds(_leftBounds, _rightBounds);
        minSpawnTime = _minSpawnTime;                   maxSpawnTime = _maxSpawnTime;
        minSpeed = _minSpeed;                           maxSpeed = _maxSpeed;
        maxInstances = _maxInst;
        directionCanChange = _changeDir;                direction = _startDir;
        configured = true;
    }

    public void SetBounds(Transform _leftBoundsNew, Transform _rightBoundsNew)
    {
        leftBounds = _leftBoundsNew;
        rightBounds = _rightBoundsNew;
    }

    public void Run()
    {
        if (configured)
        {
            StopAllCoroutines();
            StartCoroutine(SpawningTimer());
        }
        else { Debug.LogWarning("Please configure " + this.gameObject.name + " settings first using Configure()."); }
    }

    private IEnumerator SpawningTimer()
    {
        while(true)
        {
            if (maxInstances > instances.Count)
            { Spawn(); }
    
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
