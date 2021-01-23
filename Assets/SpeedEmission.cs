using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEmission : MonoBehaviour
{
    private float currentSpeed = 0f;
    private ParticleSystem particleSystem;
    public Rigidbody raftRigidBody;


    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        currentSpeed = raftRigidBody.velocity.magnitude;
        if (currentSpeed > 1f)
        {
            particleSystem.emissionRate = 25f;
            //Debug.Log("Emission 25");
        }
        else
        {
            particleSystem.emissionRate = 0f;
            //Debug.Log("Emisison 0");
        }
    }
}
