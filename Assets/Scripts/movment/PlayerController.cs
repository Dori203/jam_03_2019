using System.Collections;
using System.Collections.Generic;
using Ditzelgames;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour {
    private Rigidbody boatRB;

    [BoxGroup("paddle timing")] public float restDuration = 0.7f;

    [BoxGroup("paddle timing")] public float PaddleDuration = 0.5f;

    [HorizontalGroup("Split", 0.5f, LabelWidth = 75)] 
    [BoxGroup("Split/rawing")] public float force = 5f;

    [BoxGroup("Split/rawing")] public float backPos = 2.54f;

    [BoxGroup("Split/rotate")] public float RotateMin = 0.5f;

    [BoxGroup("Split/rotate")] public float RotateMax = 1.81f;

    [BoxGroup("Split/rotate")] public int RotateStep = 4;

    private bool paddling = false;
    private bool PaddleRight = false;
    private float sidePos;

    private float nextActionTime = 0.0f;
    private float stepInc = 1;
    private bool startPaddle;

    private Vector3 forwardVector3 = Vector3.forward;


    void Start() {
        boatRB = GetComponent<Rigidbody>();
        // rotationDirection = -1f; // todo delete
        paddling = false;
        sidePos = RotateMin;
        stepInc = (RotateMax - RotateMin) / RotateStep;
    }

    void Update() {
        UserInput();
    }

    void FixedUpdate() {
        if (Time.time > nextActionTime) {
            startPaddle = !startPaddle;
            nextActionTime += startPaddle ? PaddleDuration : restDuration;

            sidePos += (Mathf.Abs(sidePos) < Mathf.Abs(RotateMax)) ? stepInc : 0;
        } else {
            if (startPaddle && paddling) Paddle();
        }
    }

    void Paddle() {
        boatRB.AddForceAtPosition(boatRB.rotation * forwardVector3 * force,
            boatRB.rotation * new Vector3(sidePos, 0, backPos) + boatRB.position,
            ForceMode.Force);
    }

    void UserInput() {
        //Steer left
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            paddling = true;
            PaddleRight = !PaddleRight; // switch direction
            stepInc = -1f * stepInc;
            sidePos = RotateMin * Mathf.Sign(stepInc);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            paddling = false;
        }
    }
}