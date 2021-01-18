using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class ExterminationManager : MonoBehaviour {
    public static ExterminationManager SharedInstance;

    [SerializeField] private List<int> mosquitosEngagedList = new List<int>();

    // [SerializeField] private ArrayList mosquitosInCamera = new ArrayList();
    [SerializeField] private GameObject killingScorePanel;
    [SerializeField] private GameObject scorePanelMark;
    [SerializeField] private Animator aimAnimator;

    void Awake() {
        SharedInstance = this;
    }

    // Start is called before the first frame update


    public Vector3 getEngagedMosquitoPositionByIndex(int i) {
        int mosquitosNumber = mosquitosEngagedList[i];
        //Debug.Log("VAgetEngagedMosquitoPositionByIndex : " + mosquitosNumber);
        return MosquitoSpawner.SharedInstance.getMosquitoPositionByIndex(mosquitosNumber);
    }
    
    public int getEngagedMosquitoNumberByIndex(int i) {
        int mosquitosNumber = mosquitosEngagedList[i];
        //Debug.Log("getEngagedMosquitoNumberByIndex : " + mosquitosNumber);
        return mosquitosNumber;
    }

    public int getEngagedListSize() {
        return mosquitosEngagedList.Count;
    }

    public void MosquitoesEngaged(int MosquitoeNumber) {
        if(!mosquitosEngagedList.Contains(MosquitoeNumber)) mosquitosEngagedList.Add(MosquitoeNumber);
        GameManager.Instance.MosquitoesTriggered(MosquitoeNumber);
    }

    public void MosquitoeHit(int MosquitoeNumber) // TODO attach Mosquito controller
    {
        //add another score to the extermination scores.
        Instantiate(scorePanelMark, killingScorePanel.transform);

        //add mosquito score in gameManager
        GameManager.Instance.incExterminationScore();

        //remove mosquito from engaged list
        if (mosquitosEngagedList.Contains(MosquitoeNumber)) mosquitosEngagedList.Remove(MosquitoeNumber);

        //set a new random position for mosquito
        MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber).transform.position =
            MosquitoSpawner.SharedInstance.NewRandomPosition();

        //check if no mosquitos engaged in battle.
        if (mosquitosEngagedList.Count == 0) {
            GameManager.Instance.MosquitoesTriggered(-1);
        } else {
            GameManager.Instance.MosquitoeHit(MosquitoeNumber);
        }

        aimAnimator.Play("swat");
        // Debug.Log("Played Swat Animation");
    }
}