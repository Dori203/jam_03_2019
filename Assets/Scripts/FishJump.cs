using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishJump : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float timerMin;
    [SerializeField] private float timerMax;
    [SerializeField] private float forceFactor;
    [SerializeField] private float torqueFactor;

    [SerializeField] private int sortOrder;
    private SpriteMask spriteMask;
    private SpriteRenderer spriteRenderer;

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(timerMin, timerMax);
        rb = GetComponent<Rigidbody>();
        spriteMask = GetComponent<SpriteMask>();
        spriteMask.frontSortingOrder = sortOrder + 1;
        spriteMask.backSortingOrder = sortOrder;

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = sortOrder + 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(5f, 7.5f), Random.Range(-1f, 1f)).normalized * forceFactor,ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1, 1f), Random.Range(-1f, 1f)).normalized * torqueFactor, ForceMode.Impulse);
            timer = timer = Random.Range(timerMin, timerMax);
        }
    }
}

