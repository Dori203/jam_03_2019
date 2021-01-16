    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 distance = (new Vector3(target.position.x, 0, target.position.z) - transform.position);
        rb.AddForce(distance.normalized * acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //todo lose game
        }
    }
}
