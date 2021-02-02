using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPointController : MonoBehaviour
{
    [SerializeField] private int type;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject icon;
    [SerializeField] private bool found;
    void Start()
    {
        particles = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "raftExplorationRadar" && !found)
        {
            ExplorationManager.SharedInstance.interestPointHit(type);
            icon.SetActive(true);
            particles.Play();
            found = true;
        }
    }
}
