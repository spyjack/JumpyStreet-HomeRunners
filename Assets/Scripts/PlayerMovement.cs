using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private WorldGenerationController worldGenerator;

    private Animator animator;

    private bool isHopping;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && !isHopping)
        {
            animator.SetTrigger("hop");
            isHopping = true;

            worldGenerator.PullWorld(new Vector3(0, 0, -1));
            /*float zDifference = 0;

            if(transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            MoveCharacter(new Vector3(1, 0, zDifference));*/
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
