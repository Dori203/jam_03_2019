using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{

    public static ExplorationManager SharedInstance;


    //[SerializeField] private ArrayList interetPoints = new ArrayList();
    [SerializeField] private List<int> interetPointsEngagedList = new List<int>();

    void Awake()
    {
        SharedInstance = this;
    }

    public void interestPointHit(int interestPointIndex) 
    {
        if (!interetPointsEngagedList.Contains(interestPointIndex))
        {
            //mark interest point as visited.
            interetPointsEngagedList.Add(interestPointIndex);

            //broadcast
            GameManager.Instance.InterestPointHit(interestPointIndex);

            //increase exploration score.
            GameManager.Instance.incExplorationScore();
        }

    }
}
