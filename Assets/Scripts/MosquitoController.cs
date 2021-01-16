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
<<<<<<< HEAD
<<<<<<< HEAD
        // if (!inRaft && other.tag == "MosquitoAttractor")
=======
>>>>>>> 25a449374b42386df868805736884f0e9c6986a2
=======
>>>>>>> 25a449374b42386df868805736884f0e9c6986a2
        if (other.tag == "MosquitoAttractor")
        {
            Vector3 forceDirection = other.transform.position - transform.position;
            // apply force on target towards raft.
            rb.AddForce(forceDirection.normalized * pullForce, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MosquitoLimit")
        {
            inRaft = true;
<<<<<<< HEAD
<<<<<<< HEAD
            // this.transform.SetParent(other.transform);
            // rb.isKinematic = true;
=======
>>>>>>> 25a449374b42386df868805736884f0e9c6986a2
=======
>>>>>>> 25a449374b42386df868805736884f0e9c6986a2
            
            // calculate force vector
            var force = transform.position - other.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude,ForceMode.Impulse);
            ExterminationManager.SharedInstance.MosquitoesEngaged(MosquitoeNumber);
        }

    }

}
