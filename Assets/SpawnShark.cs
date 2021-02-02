using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShark : MonoBehaviour
{
    public float yOffset = 0f;
    public bool randomRotationX = true;
    public bool randomRotationY = true;
    public bool randomRotationZ = true;
    public float radius = 90f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y).normalized * radius;
    }
}
