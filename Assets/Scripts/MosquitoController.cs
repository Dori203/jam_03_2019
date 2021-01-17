using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    [SerializeField] private float pullForce = 5.42f;
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float magnitude = 0.03f;

    private Rigidbody rb;
    private bool inRaft = false;
    public int MosquitoeNumber;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //When the Mosquito collides with the MosquitoAttractor, it will start moving towards the raft.
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MosquitoAttractor")
        {
            Vector3 forceDirection = other.transform.position - transform.position;
            // apply force on target towards raft.
            rb.AddForce(forceDirection.normalized * pullForce, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        if (other.tag == "MosquitoLimit")
        {
            sendMosquitoAway(other.transform);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MosquitoLimit")
        {
            inRaft = true;
            sendMosquitoAway(other.transform);
            ExterminationManager.SharedInstance.MosquitoesEngaged(MosquitoeNumber);
        }

    }

    private void sendMosquitoAway(Transform raft)
    {
        var force = transform.position - raft.transform.position;
        // normalize force vector to get direction only and trim magnitude
        force.Normalize();
        gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude, ForceMode.Impulse);
    }
}
