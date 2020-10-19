using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private WorldGenerationController worldGenerator;

    private Animator animator;

    private bool isHopping;

    public GameObject deathPanel;

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

        deathPanel.SetActive(false);
    }

    private void Update()
    {
        if(playerSeat != null)
        {
            transform.position = playerSeat.position;
            if(transform.position.x > (worldGenerator.GetWorldWidth/2 - 1))
            {
                playerSeat = null;
                MoveHorizontal(Vector3.left);
            }else if(transform.position.x < (-(worldGenerator.GetWorldWidth / 2) + 1))
            {
                playerSeat = null;
                MoveHorizontal(Vector3.right);
            }
        }

        HopTimer();

        //Player movement could be refactored to a simple script that checks the assigned direction and executes through the list.
        //Would need two versions, one for forward movement and one for sideways movement.
        if (!isDead)
        {
            if (Input.GetAxisRaw("Vertical") > 0.1f && !isHopping && GameUIController.pauseMenuIsOn == false)
            {
                int colCheck = CheckCollision(Vector3.forward);
                if (colCheck == 0)
                {
                    if (moveBacks > 0) { moveBacks--; }
                    MoveCharacter(Vector3.zero);
                    transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f, TerrainHeight(Vector3.forward).y, transform.position.z);
                    worldGenerator.PullWorld(new Vector3(0, 0, -1));
                }
                else if (colCheck == 4)
                {
                    worldGenerator.PullWorld(new Vector3(0, 0, -1));
                    MoveCharacter(Vector3.zero);
                    transform.position = playerSeat.position;
                }
                else if (colCheck != 1)
                {
                    MoveCharacter(Vector3.zero);
                    transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f, TerrainHeight(Vector3.forward).y, transform.position.z);
                    worldGenerator.PullWorld(new Vector3(0, 0, -1));
                    if (colCheck == 3)
                    {
                        KillPlayer(DeathType.Drowned);
                    }
                    else
                    {
                        KillPlayer(DeathType.Flattened);
                    }
                }
            }
            else if (Input.GetAxisRaw("Vertical") < -0.1f && !isHopping && moveBacks < moveBacksMax && GameUIController.pauseMenuIsOn == false)
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
                else if (colCheck != 1)
                {
                    MoveCharacter(Vector3.zero);
                    this.transform.position = new Vector3(transform.position.x, TerrainHeight(Vector3.back).y, transform.position.z);
                    worldGenerator.PullWorld(new Vector3(0, 0, 1));
                    if (colCheck == 3)
                    {
                        KillPlayer(DeathType.Drowned);
                    }
                    else
                    {
                        KillPlayer(DeathType.Flattened);
                    }
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.15f && !isHopping && GameUIController.pauseMenuIsOn == false)
            {
                MoveHorizontal(Vector3.left);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0.15f && !isHopping && GameUIController.pauseMenuIsOn == false)
            {
                MoveHorizontal(Vector3.right);   
            }
        }
    }

    void MoveHorizontal(Vector3 _dir)
    {
        int colCheck = CheckCollision(_dir);
        if (colCheck == 0 && this.transform.position.x + _dir.x < (worldGenerator.GetWorldWidth / 2 + (_dir.x * -1)))
        {
            MoveCharacter(_dir);
            this.transform.position = new Vector3(transform.position.x, TerrainHeight(_dir).y, transform.position.z);
        }
        else if (colCheck == 4)
        {
            MoveCharacter(_dir);
            transform.position = playerSeat.position;
        }
        else if (colCheck != 1 && this.transform.position.x + _dir.x < (worldGenerator.GetWorldWidth / 2 - 1))
        {
            MoveCharacter(_dir);
            this.transform.position = new Vector3(transform.position.x, TerrainHeight(_dir).y, transform.position.z);
            if (colCheck == 3)
            {
                KillPlayer(DeathType.Drowned);
            }
            else
            {
                KillPlayer(DeathType.Flattened);
            }
        }
    }

    void KillPlayer(DeathType _deathType)
    {
        isDead = true;
        if(_deathType==DeathType.Drowned)
        {
            animator.SetTrigger("drowned");
            deathPanel.SetActive(true);
        }else
        {
            animator.SetTrigger("flatten");
            deathPanel.SetActive(true);
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
            }
            else if (hit.transform.tag == "ObstacleMoving")
            {
                return _colReport = 2;
            }
            else if (hit.transform.tag == "Water")
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

public enum DeathType
{
    Drowned,
    Flattened
}

