using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAtCamera : MonoBehaviour
{
    private Camera killingCamera;
    private SpriteRenderer sr;
    public float viewRange = 5f;
    private Camera cameraToLookAt;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (tag == "mosquitoSprite")
        {
            cameraToLookAt = GameObject.Find("Killing Camera").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraToLookAt.transform);
        distance = Vector3.Distance(transform.position, cameraToLookAt.transform.position);
        if (distance > viewRange)
        {
            HideMosquitoSprite();
        }
        else
        {
            ShowMosquitoSprite();
        }
    }

    private void ShowMosquitoSprite()
    {
        sr.enabled = true;
    }

    private void HideMosquitoSprite()
    {
        sr.enabled = false;
    }
}
