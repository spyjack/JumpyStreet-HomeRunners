using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiliyPadHandler : MonoBehaviour
{
    [SerializeField]
    Transform instanceToDestroy = null;

    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            Destroy(instanceToDestroy.gameObject);
            //Could impliment an animation here.
        }
    }
}
