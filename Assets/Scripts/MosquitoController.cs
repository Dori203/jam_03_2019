using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    [SerializeField] private float pullForce = 5.42f;
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float magnitude = 0.03f;
    [SerializeField] private float randomMovementMagnitude;
    [SerializeField] private float randomPositionRadius;
    [SerializeField] private float minRandomPositionTime;
    [SerializeField] private float maxRandomPositionTime;

    private Rigidbody rb;
    private bool inRaft = false;
    public int MosquitoeNumber;
    [SerializeField] private Transform playerFace;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerFace = GameObject.Find("Face").transform;
    }

    //When the Mosquito collides with the MosquitoAttractor, it will start moving towards the raft.
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MosquitoAttractor")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                //randomize a vector in small range to move mosquito to.
                var force = new Vector3(Random.Range(randomPositionRadius, -1 * randomPositionRadius), Random.Range( 1 * randomPositionRadius, -1 * randomPositionRadius), Random.Range(1 * randomPositionRadius, -1 * randomPositionRadius));
                // normalize force vector to get direction only and trim magnitude
                rb.AddForce(force * randomMovementMagnitude, ForceMode.Impulse);
                StartCountdown(minRandomPositionTime, maxRandomPositionTime);
            }
            Vector3 forceDirection = playerFace.transform.position - transform.position;
            forceDirection.y = 0;
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
        rb.AddForce(force * magnitude, ForceMode.Impulse);
    }


    private void StartCountdown(float min, float max)
    {
        timer = Random.Range(min, max);
    }
}
