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

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isHopping)
        {
            if (hopTimer < hopDelay)
            {
                hopTimer += Time.deltaTime;
            }else
            {
                isHopping = false;
                hopTimer = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.W) && !isHopping)
        {
            animator.SetTrigger("hop");
            isHopping = true;

            worldGenerator.PullWorld(new Vector3(0, 0, -1));
        }
        else if(Input.GetKeyDown(KeyCode.A) && !isHopping)
        {
            //WOrld moves on Z axis, player should move on X axis for side to side.
            MoveCharacter(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isHopping)
        {
            //WOrld moves on Z axis, player should move on X axis for side to side.
            MoveCharacter(new Vector3(1, 0, 0));
        }
    }

    private void MoveCharacter(Vector3 difference)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        transform.position = (transform.position + difference);
    }

    public void FinishHop()
    {
        isHopping = false;
    }
}
