using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public float viewRange = 5f;
    public string cameraName;

    private Camera camera;
    private SpriteRenderer sr;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        camera = GameObject.Find(cameraName).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform);
        distance = Vector3.Distance(transform.position, camera.transform.position);
        /*
        if (distance > viewRange)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }
        */
    }
}
