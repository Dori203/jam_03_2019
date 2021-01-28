using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFish : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FishingArea")
        {
            Debug.Log("Raft at Fishing area");
            //Update current fishing area.
            FishingManager.SharedInstance.enterFishingArea(other.gameObject.GetComponent<FishingArea>().getFishType());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FishingArea")
        {
            Debug.Log("Raft left Fishing area");
            FishingManager.SharedInstance.leaveFishingArea(other.gameObject.GetComponent<FishingArea>().getFishType());
        }
    }
}
