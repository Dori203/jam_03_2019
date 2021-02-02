using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    private bool zoomedOut;

    [SerializeField] Animator explorationCamera;
    // Start is called before the first frame update

    void Start()
    {
        zoomedOut = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && !zoomedOut)
        {
            zoomedOut = true;
            explorationCamera.SetBool("Shark", true);
            Debug.Log("Zoom Out");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster") && zoomedOut)
        {
            zoomedOut = false;
            explorationCamera.SetBool("Shark", false);
            Debug.Log("Zoom In");
        }
    }
}
