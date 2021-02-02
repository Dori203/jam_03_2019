using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPointController : MonoBehaviour
{
    [SerializeField] private int type;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject icon;

    void Start()
    {
        particles = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "raftExplorationRadar")
        {
            ExplorationManager.SharedInstance.interestPointHit(type);
            icon.SetActive(true);
            particles.Play();
        }
    }
}
