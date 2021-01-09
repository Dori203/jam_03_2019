using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocks : MonoBehaviour
{
    public GameObject[] rocks;
    public int numberOfRocks = 500;
    public float squareLength = 240;
    public float rockMinScale = 1f;
    public float rockMaxScale = 5f;
    public float heightVariation = 2f;
    public float restrictAreaSize = 10f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector3 position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation, (Random.value - 0.5f) * squareLength);
            while ((position.x <= restrictAreaSize && position.x >= -restrictAreaSize) && (position.z <= restrictAreaSize && position.z >= -restrictAreaSize))
            {
                position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation, (Random.value - 0.5f) * squareLength);
            }
            GameObject rockType = rocks[Random.Range(0, rocks.Length)];
            GameObject currentRock = Instantiate(rockType, position, Random.rotation, gameObject.transform);
            float rockScale = Random.Range(rockMinScale, rockMaxScale);
            currentRock.transform.localScale = new Vector3(rockScale, rockScale, rockScale);
        }
    }
}
