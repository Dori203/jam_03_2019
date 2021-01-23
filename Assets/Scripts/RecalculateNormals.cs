using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateNormals : MonoBehaviour
{
    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.RecalculateNormals();
    }
}