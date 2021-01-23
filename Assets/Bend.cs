using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bend : MonoBehaviour
{
    public float distance;
    public float originalHeight;
    public Transform raft;
    // Start is called before the first frame update
    void Start()
    {
        originalHeight = transform.localScale.y;
        raft = GameObject.Find("Raft").transform;
    }

    void FixedUpdate()
    {
        distance = Mathf.Abs((transform.position - raft.position).magnitude);
        if (distance < 6.25f)
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                                distance/6.25f*originalHeight,
                                                transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x,
                                    originalHeight,
                                    transform.localScale.z);

        }
    }
}
