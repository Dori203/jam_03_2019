using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateNormals : MonoBehaviour
{
    private Mesh mesh;

    void Awake()
    {
        if (GetComponent<MeshFilter>() != null)
        {
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.RecalculateNormals();
        }
        else if (GetComponent<SkinnedMeshRenderer>() != null)
        {
            mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
            mesh.RecalculateNormals();
            Debug.Log("Recalculated Normals!");
            Debug.Log(GetComponent<SkinnedMeshRenderer>().sharedMesh);
        }
    }
}