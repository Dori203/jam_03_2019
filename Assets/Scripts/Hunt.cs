using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxSpeedPatrol = 5f;
    [SerializeField] private float maxSpeedCooldown = 7f;
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
        startTimer();
        maxSpeed = maxSpeedPatrol;
    }

    private void FixedUpdate()
    {
        if (cooldown)
        {
            maxSpeed = maxSpeedCooldown;
            target = nullTarget;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                cooldown = false;
            }
        }
        else
        {
            maxSpeed = maxSpeedPatrol;
            target = raft;
        }

        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime);

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

    private void OnTriggerStay(Collider other)
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