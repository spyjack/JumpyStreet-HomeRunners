using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private WorldGenerationController worldGenerator;

    private Animator animator;

    private bool isHopping;

    [SerializeField]
    float hopDelay = 0;
    float hopTimer = 0;

    [SerializeField]
    int moveBacksMax = 3;
    int moveBacks = 0;

    [SerializeField]
    Transform playerSeat = null;

    public AudioClip hop;
    AudioSource audioSource;

    [SerializeField]
    bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(playerSeat != null)
        {
            transform.position = playerSeat.position;
        }

        HopTimer();

        if(Input.GetAxisRaw("Vertical") > 0.1f && !isHopping)
        {
            int colCheck = CheckCollision(Vector3.forward);
            if (colCheck == 0)
            {
                if (moveBacks > 0) { moveBacks--; }
                MoveCharacter(Vector3.zero);
                transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f, TerrainHeight(Vector3.forward).y, transform.position.z);
                worldGenerator.PullWorld(new Vector3(0, 0, -1));
            }else if (colCheck == 4)
            {
                worldGenerator.PullWorld(new Vector3(0, 0, -1));
                MoveCharacter(Vector3.zero);
                transform.position = playerSeat.position;
            }
            else
            {
                isDead = true;
            }
        }
        else if(Input.GetAxisRaw("Vertical") < -0.1f && !isHopping && moveBacks < moveBacksMax)
        {
            int colCheck = CheckCollision(Vector3.back);
            if (colCheck == 0)
            {
                moveBacks++;
                MoveCharacter(Vector3.zero);
                this.transform.position = new Vector3(transform.position.x, TerrainHeight(Vector3.back).y, transform.position.z);
                worldGenerator.PullWorld(new Vector3(0, 0, 1));
            }
            else if (colCheck == 4)
            {
                worldGenerator.PullWorld(new Vector3(0, 0, 1));
                MoveCharacter(Vector3.zero);
                transform.position = playerSeat.position;
            }
            else
            {
                isDead = true;
            }
        }
        else if(Input.GetAxisRaw("Horizontal") < -0.15f && !isHopping)
        {
            int colCheck = CheckCollision(Vector3.left);
            //World moves on Z axis, player should move on X axis for side to side.
            if (colCheck == 0 && this.transform.position.x - 1 > ((worldGenerator.GetWorldWidth / 2 - worldGenerator.GetWorldWidth) + 1))
            {
                MoveCharacter(new Vector3(-1, 0, 0));
                this.transform.position = new Vector3(transform.position.x, TerrainHeight(Vector3.left).y, transform.position.z);
            }
            else if (colCheck == 4)
            {
                MoveCharacter(new Vector3(-1, 0, 0));
                transform.position = playerSeat.position;
            }
            else
            {
                isDead = true;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0.15f && !isHopping)
        {
            int colCheck = CheckCollision(Vector3.right);
            if (colCheck == 0 && this.transform.position.x + 1 < (worldGenerator.GetWorldWidth/2 - 1))
            {
                MoveCharacter(new Vector3(1, 0, 0));
                this.transform.position = new Vector3(transform.position.x, TerrainHeight(Vector3.right).y, transform.position.z);
            }
            else if (colCheck == 4)
            {
                MoveCharacter(new Vector3(1, 0, 0));
                transform.position = playerSeat.position;
            }else
            {
                isDead = true;
            }
        }
    }

    int CheckCollision(Vector3 _direction)
    {
        int _colReport = 0;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(_direction.x, transform.position.y + 40, _direction.z), Vector3.down, out hit, 45f))
        {
            Debug.DrawRay(transform.position + new Vector3(_direction.x, transform.position.y + 40, _direction.z), Vector3.down * hit.distance, Color.yellow);
            

            if(hit.transform.tag == "ObstacleBlocking")
            {
                return _colReport = 1;
            }else if (hit.transform.tag == "ObstacleMoving")
            {
                return _colReport = 2;
            }else if (hit.transform.tag == "Water")
            {
                return _colReport = 3;
            }
            else if (hit.transform.tag == "PlatformMoving")
            {
                playerSeat = hit.transform;
                return _colReport = 4;
            }

        }
        //Debug.Log("Did Hit " + _colReport);
        playerSeat = null;
        return _colReport;
    }

    Vector3 TerrainHeight(Vector3 _direction)
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(_direction.x, transform.position.y + 40, _direction.z), Vector3.down, out hit, 45f, layerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void MoveCharacter(Vector3 difference)
    {
        animator.SetTrigger("hop");
        audioSource.PlayOneShot(hop, 0.5f);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        isHopping = true;
        transform.position = (transform.position + difference);
    }

    void HopTimer()
    {
        if (isHopping)
        {
            if (hopTimer < hopDelay)
            {
                hopTimer += Time.deltaTime;
            }
            else
            {
                isHopping = false;
                hopTimer = 0;
            }
        }
    }
}
