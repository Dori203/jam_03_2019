using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objects;
    public int numberOfObjects = 500;
    public float squareLength = 240;
    public float rockMinScale = 1f;
    public float rockMaxScale = 5f;
    public float heightVariation = 2f;
    public float restrictAreaSize = 10f;
    public float yOffset = 0f;
    public bool randomRotationX = true;
    public bool randomRotationY = true;
    public bool randomRotationZ = true;
    public float radius = 90f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            /* Distance around the circle */
            var radians = 2 * Mathf.PI / numberOfObjects * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            var spawnPos = spawnDir * radius; // Radius is just the distance away from the 

            /* Now spawn */
            GameObject objectType = objects[Random.Range(0, objects.Length)];
            GameObject currentObject = Instantiate(objectType, spawnPos, Quaternion.identity, gameObject.transform);
            currentObject.transform.localRotation = Quaternion.Euler(new Vector3(System.Convert.ToInt32(randomRotationX) * (Random.Range(0f, 360f)),
                            System.Convert.ToInt32(randomRotationY) * (Random.Range(0f, 360f)),
                            System.Convert.ToInt32(randomRotationZ) * (Random.Range(0f, 360f))));

            float objectScale = Random.Range(rockMinScale, rockMaxScale);
            currentObject.transform.localScale = new Vector3(objectScale, objectScale, objectScale);
        }
    }
}
