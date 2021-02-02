﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private float turningMagnitude = 50f;
    [SerializeField] private Transform nullTarget;
    [SerializeField] private Transform raft;
    private Rigidbody rb;
    public bool cooldown;
    private float timer;
    private Vector3 prevForce;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = raft;
    }

    private void FixedUpdate()
    {
        if (cooldown)
        {
            target = nullTarget;
        }
        else
        {
            target = raft;
        }

        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);

//        Quaternion rotation = new Quaternion();
//        rotation.SetLookRotation(rb.velocity);
//        transform.rotation = rotation;
        Vector3 distance = (new Vector3(target.position.x, 0, target.position.z) - transform.position);
        prevForce = distance.normalized * acceleration;
        rb.AddForce(distance.normalized * acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Raft") && !cooldown)
        {
            Debug.Log("hit player on trigger");
            ExplorationManager.SharedInstance.monsterHit();
            cooldown = true;
            startTimer();
        }
    }


    private void startTimer()
    {
        timer = cooldownTime;
    }
}