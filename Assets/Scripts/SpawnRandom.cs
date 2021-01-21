using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
            while ((position.x <= restrictAreaSize && position.x >= -restrictAreaSize) && (position.z <= restrictAreaSize && position.z >= -restrictAreaSize))
            {
                position = new Vector3((Random.value - 0.5f) * squareLength, (Random.value - 0.5f) * heightVariation + yOffset, (Random.value - 0.5f) * squareLength);
            }
            GameObject objectType = objects[Random.Range(0, objects.Length)];
            GameObject currentObject = Instantiate(objectType, position, Quaternion.identity, gameObject.transform);
            currentObject.transform.localRotation= Quaternion.Euler(new Vector3(System.Convert.ToInt32(randomRotationX) * (Random.Range(0f, 360f)),
                                        System.Convert.ToInt32(randomRotationY) * (Random.Range(0f, 360f)),
                                        System.Convert.ToInt32(randomRotationZ) * (Random.Range(0f, 360f))));
            float rockScale = Random.Range(rockMinScale, rockMaxScale);
            currentObject.transform.localScale = new Vector3(rockScale, rockScale, rockScale);
        }
    }
}
