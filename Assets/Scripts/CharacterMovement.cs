using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float gravity = 3;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.isGrounded)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            moveDirection = new Vector3(moveX, 0, moveZ);
            moveDirection *= moveSpeed;
        }
        moveDirection.y -= gravity;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
