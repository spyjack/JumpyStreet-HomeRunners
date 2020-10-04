using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Vector3 direction = new Vector3(0,0,0);
    public float speed = 0;
    public Transform deathPoint = null;

    public List<ObjectMover> myInstanceList = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(this.transform.position, deathPoint.position) < 0.1f)
        {
            if (myInstanceList != null)
            {
                myInstanceList.Remove(this);
            }

            Destroy(this.gameObject);
        }

        this.transform.position += direction * speed * Time.deltaTime;
    }
}
