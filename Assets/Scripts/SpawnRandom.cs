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
    public float innerRadius = 90f;
    public float outerRadius = 90f;
    public bool spawnEqualy = false;
    public bool fishingeaer = false;

    // Start is called before the first frame update
    void Start() {
        int equalPart = numberOfObjects / objects.Length;
        if (fishingeaer) Debug.Log("equalPart : " + equalPart);
        for (int i = 0; i < numberOfObjects; i++)
        {
            float ratio = innerRadius / outerRadius + 1;
            float radius = Mathf.Sqrt(Random.Range(ratio * ratio, 1f)) * outerRadius;
            var xz = Random.insideUnitCircle.normalized * radius;
            var spawnPosition = new Vector3(xz.x, 0, xz.y);
            int objectNum = spawnEqualy ? i / equalPart : Random.Range(0, objects.Length);
            if (objectNum >= objects.Length) break;
            if (fishingeaer) Debug.Log("objectNum : " + objectNum);
            GameObject objectType = objects[objectNum];
            GameObject currentObject = Instantiate(objectType, spawnPosition, Quaternion.identity, gameObject.transform);
            currentObject.transform.localRotation = Quaternion.Euler(new Vector3(System.Convert.ToInt32(randomRotationX) * (Random.Range(0f, 360f)),
                            System.Convert.ToInt32(randomRotationY) * (Random.Range(0f, 360f)),
                            System.Convert.ToInt32(randomRotationZ) * (Random.Range(0f, 360f))));
            float rockScale = Random.Range(rockMinScale, rockMaxScale);
            currentObject.transform.localScale = new Vector3(rockScale, rockScale, rockScale);
        }
    }
}