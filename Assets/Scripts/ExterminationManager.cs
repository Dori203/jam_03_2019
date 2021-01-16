using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class ExterminationManager : MonoBehaviour{
    
    public static ExterminationManager SharedInstance;

    [SerializeField] private ArrayList mosquitosEngagedList = new ArrayList();
    // [SerializeField] private ArrayList mosquitosInCamera = new ArrayList();
    [SerializeField] private GameObject killingScorePanel;
    [SerializeField] private GameObject scorePanelMark;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    

    public void MosquitoesEngaged(int MosquitoeNumber)
    {
        GameObject mosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
        Debug.Log("MosquitoesEngaged : " + MosquitoeNumber);

        mosquitosEngagedList.Add(MosquitoeNumber);
        print("add : " + MosquitoeNumber + "\nmosquitosEngagedList : "  + mosquitosEngagedList.Count);

        GameManager.Instance.MosquitoesTriggered(MosquitoeNumber);
    }
    
    private void MosquitoeHit(int MosquitoeNumber) // TODO attach Mosquito controller
    {
        //add another score to the extermination scores.
        Instantiate(scorePanelMark, killingScorePanel.transform);

        mosquitosEngagedList.Remove(MosquitoeNumber);
        print("remove : " + MosquitoeNumber + "\nmosquitosEngagedList : "  + mosquitosEngagedList.Count);
        
        GameManager.Instance.MosquitoeHit(MosquitoeNumber);

        // GameObject mosquito = MosquitoObjectPool.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
        // Vector3 position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
        // while ((position.x <= restrictAreaSize && position.x >= -restrictAreaSize) && (position.z <= restrictAreaSize && position.z >= -restrictAreaSize))
        // {
        //     position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
        // }
        // mosquito.transform.position = position;
    }
}
