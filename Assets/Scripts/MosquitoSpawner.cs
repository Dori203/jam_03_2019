using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoObjectPool : MonoBehaviour
{
    public static MosquitoObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool; 
    
    public float squareLength = 240;
    public float heightVariation = 2f;
    public float restrictAreaSize = 10f;
    public float yOffset = 0f;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        
        SpawnMosquitos();
    }
    
    private void SpawnMosquitos()
    {
        GameObject tmp;
        
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        } 
        
        for(int i = 0; i < amountToPool; i++) {
            GameObject mosquito = GetPooledObjectByIndex(i);
            MosquitoController mosquitoController = mosquito.GetComponent<MosquitoController>();
            mosquito.transform.position = NewRandomPosition();
            mosquitoController.MosquitoeNumber = i;
            mosquito.SetActive(true);
        }
    }

    public Vector3 NewRandomPosition() {
        Vector3 position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
        while ((position.x <= restrictAreaSize && position.x >= -restrictAreaSize) && (position.z <= restrictAreaSize && position.z >= -restrictAreaSize))
        {
            position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
        }
        return position;
    }

    public GameObject GetPooledObjectByIndex(int i) {
        if (i >= amountToPool) return null; 
        return pooledObjects[i];
    }

    // GameObject bullet = ObjectPool.SharedInstance.GetPooledObject(); 
    //     if (bullet != null) {
    //     bullet.transform.position = turret.transform.position;
    //     bullet.transform.rotation = turret.transform.rotation;
    //     bullet.SetActive(true);
    // }
    
    // Destroy(gameObject); -> gameobject.SetActive(false);
}
