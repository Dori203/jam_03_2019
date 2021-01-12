using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAtCamera : MonoBehaviour
{
    private Camera cameraToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        if(tag == "mosquitoSprite")
        {
            cameraToLookAt = GameObject.Find("Killing Camera").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraToLookAt.transform);
    }
}
