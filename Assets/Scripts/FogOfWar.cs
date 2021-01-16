using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask fogLayer;
    [SerializeField] private float radius = 20f;
    [SerializeField] private Color initialColor;
    [SerializeField] private Color newColor;
    private float m_radiusSqr { get { return radius * radius; } }

    private ProBuilderMesh ground;
    public Material fogMaterial;
    private Vector3[] points;
    [SerializeField] private int UVLength = 100;
    [SerializeField] private Vector3 center;
    [SerializeField] private int edgeLength;

    private Mesh m_mesh;
    private Vector3[] m_vertices;
    private Color[] m_colors;

    // Use this for initialization
    void Start()
    {
        Initialize();

        m_mesh = ground.GetComponent<MeshFilter>().mesh;
        m_vertices = m_mesh.vertices;
        m_colors = new Color[m_vertices.Length];
        for (int i = 0; i < m_colors.Length; i++)
        {
            m_colors[i] = initialColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray r = new Ray(transform.position, player.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 10000, fogLayer, QueryTriggerInteraction.Collide))
        {
            for (int i = 0; i < m_vertices.Length; i++)
            {
                Vector3 v = ground.transform.TransformPoint(m_vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < m_radiusSqr)
                {
                    //float alpha = 0f; // Mathf.Min(m_colors[i].a, dist / m_radiusSqr);
                    m_colors[i].a = 0f;
                }
            }
            UpdateColor();
        }
    }

    void Initialize()
    {
        Vector3[] points = new Vector3[UVLength * UVLength];
        Face[] faces = new Face[(UVLength - 1) * (UVLength - 1)];
        float xThreshold = (edgeLength) / UVLength;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(center.x + (xThreshold * (i % UVLength)), center.y, center.z + (xThreshold * (i / UVLength)));
        }
        int j = 0;
        for (int i = 0; i < points.Length - UVLength; i++)
        {
            if (i % UVLength != UVLength - 1)
            {
                faces[j] = new Face(new int[] { i, i + 1, i + UVLength, i + 1, i + UVLength + 1, i + UVLength });
                j++;
            }
        }

        ground = ProBuilderMesh.Create(points, faces);
        ground.CenterPivot(new int[] { 0, UVLength - 1, UVLength * (UVLength - 1), (UVLength * UVLength) - 1 });
        ground.Refresh();
        MeshRenderer meshRend = ground.GetComponent<MeshRenderer>();
        meshRend.material = fogMaterial;
        ground.transform.position = center;
        ground.gameObject.layer = 9;
        ground.gameObject.name = "fog";
        ground.gameObject.AddComponent<MeshCollider>();
        ground.transform.Rotate(new Vector3(180f, 0, 0));
        ground.transform.localScale = (new Vector3(1/3f, 1/3f, 1/3f));
    }

    void UpdateColor()
    {
        m_mesh.colors = m_colors;
    }
}
