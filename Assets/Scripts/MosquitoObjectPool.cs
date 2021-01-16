using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoObjectPool : MonoBehaviour
{
    public static MosquitoObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public ExterminationManager exterminationManager;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
        exterminationManager.SpawnMosquitos();
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
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
