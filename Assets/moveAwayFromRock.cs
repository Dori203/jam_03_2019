using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAwayFromRock : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rockHitForce;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            rb.AddForce(rockHitForce * (rb.position - collision.transform.position).normalized, ForceMode.Impulse);
        }
    }
}
