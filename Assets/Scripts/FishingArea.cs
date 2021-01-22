using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingArea : MonoBehaviour
{ 
    [SerializeField] private float colliderRadius;
    [SerializeField] private FishingManager.FishType fishType;

    
    void Awake()
    {
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.center = Vector3.zero; // the center must be in local coordinates
        sphereCollider.radius = colliderRadius;
        sphereCollider.isTrigger = true;
    }

    public FishingManager.FishType getFishType()
    {
        return fishType;
    }
}
