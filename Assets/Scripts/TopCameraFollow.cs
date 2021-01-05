using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCameraFollow : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position + offset;
        transform.position = newPosition;
     }
}
