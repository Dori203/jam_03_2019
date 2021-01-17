using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private List<int> fishCaughtList = new List<int>();
    public static FishingManager SharedInstance;

    void Awake()
    {
        SharedInstance = this;
    }

    public void fishHit(int fishTypeIndex)
    {

        //check if fish already caught.
        if (!fishCaughtList.Contains(fishTypeIndex))
        {
            fishCaughtList.Add(fishTypeIndex);

            //increase fishing score.
            GameManager.Instance.incFishingScore();

            //broadcast
            GameManager.Instance.NewFishCaught(fishTypeIndex);
        }
        

    }
}
