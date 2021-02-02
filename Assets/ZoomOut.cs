using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    private int numObjects;
    private bool zoomerOut;
    [SerializeField] Animator explorationCamera;
    // Start is called before the first frame update

    void Start()
    {
        numObjects = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            numObjects += 1;
            explorationCamera.SetBool("Shark", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            numObjects -= 1;
            if (numObjects == 0)
            {
                explorationCamera.SetBool("Shark", false);
                Debug.Log("Zoom In");
            }
        }
    }
}
