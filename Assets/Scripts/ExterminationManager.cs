using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class ExterminationManager : ListeningMonoBehaviour
{
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<int> {Event = GameManager.Channels.MosquitoesEngaged.GetPath(), Callback = MosquitoesEngaged},
        new BaseListener<int> {Event = GameManager.Channels.MosquitoeHit.GetPath(), Callback = MosquitoeHit}
    };

    // [SerializeField] private ArrayList mosquitosEngaged = new ArrayList();
    // [SerializeField] private ArrayList mosquitosInCamera = new ArrayList();
    [SerializeField] private GameObject killingScorePanel;
    [SerializeField] private GameObject scorePanelMark;
    
    public float squareLength = 240;
    public float heightVariation = 2f;
    public float restrictAreaSize = 10f;
    public float yOffset = 0f;

    // Start is called before the first frame update
    public void SpawnMosquitos()
    {
        int amountToPool = MosquitoObjectPool.SharedInstance.amountToPool;
        for(int i = 0; i < amountToPool; i++) {
            GameObject mosquito = MosquitoObjectPool.SharedInstance.GetPooledObjectByIndex(i);
            MosquitoController mosquitoController = mosquito.GetComponent<MosquitoController>();
            
            Vector3 position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
            while ((position.x <= restrictAreaSize && position.x >= -restrictAreaSize) && (position.z <= restrictAreaSize && position.z >= -restrictAreaSize))
            {
                position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
            }
            mosquito.transform.position = position;
            
            mosquitoController.MosquitoeNumber = i;
            
            mosquito.SetActive(true);
        }
    }

    public void MosquitoesEngaged(int MosquitoeNumber)
    {
        GameObject mosquito = MosquitoObjectPool.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
        Debug.Log("MosquitoesEngaged : " + MosquitoeNumber);
    }
    private void MosquitoeHit(int MosquitoeNumber) // TODO attach Mosquito controller
    {
        //add another score to the extermination scores.
        Instantiate(scorePanelMark, killingScorePanel.transform);

        GameManager.Instance.MosquitoeHit(MosquitoeNumber);
        
        GameObject mosquito = MosquitoObjectPool.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
        Vector3 position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
        while ((position.x <= restrictAreaSize && position.x >= -restrictAreaSize) && (position.z <= restrictAreaSize && position.z >= -restrictAreaSize))
        {
            position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
        }
        mosquito.transform.position = position;
    }

    /**
    private void mosquitoesEngaged(GameObject mosquito)
    {
        mosquitosEngaged.Add(mosquito);
    }
    **/
}
