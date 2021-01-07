using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Quaternion rotationOffset;
    [SerializeField] private bool followRotation;
    private void LateUpdate()
    {
        Vector3 newPosition = player.position + positionOffset;
        transform.position = newPosition;
        if (followRotation)
        {
            transform.rotation = player.rotation * rotationOffset;
        }
    }
}
