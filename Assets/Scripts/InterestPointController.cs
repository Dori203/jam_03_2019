using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPointController : MonoBehaviour
{
    [SerializeField] private int type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "raftExplorationRadar")
        {
            ExplorationManager.SharedInstance.interestPointHit(type);
        }
    }
}
