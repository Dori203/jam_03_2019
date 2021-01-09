﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour {
    [SerializeField] private float pullForce = 5.42f;
    private Rigidbody rb;
    private bool inRaft;
    private GameObject player;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    //When the Mosquito collides with the MosquitoAttractor, it will start moving towards the raft.
    private void OnTriggerStay(Collider other) {
        if (!inRaft && other.tag == "MosquitoAttractor") {
            Vector3 forceDirection = other.transform.position - transform.position;
            // apply force on target towards raft.
            rb.AddForce(forceDirection.normalized * pullForce, ForceMode.Acceleration);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (inRaft && other.tag == "MosquitoAttractor") {
            inRaft = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "MosquitoLimit") {
            inRaft = true;
            rb.transform.SetParent(other.transform, false);

            GameManager.Instance.MosquitoesTriggered(true);
        }
    }
}