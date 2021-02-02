using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{

    [SerializeField] Animator explorationCamera;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            explorationCamera.SetBool("Shark", true);
            Debug.Log("Zoom Out");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            explorationCamera.SetBool("Shark", false);
            Debug.Log("Zoom In");
        }
    }
}
