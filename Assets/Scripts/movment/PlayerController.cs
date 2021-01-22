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

    [HorizontalGroup("Split", 0.5f, LabelWidth = 75)] [BoxGroup("Split/rawing")]
    public float force = 5f;

    [BoxGroup("Split/rawing")] public float backPos = 2.54f;

    [BoxGroup("Split/rotate")] public float RotateMin = 0.5f;

    [BoxGroup("Split/rotate")] public float RotateMax = 1.81f;

    [BoxGroup("Split/rotate")] public int RotateStep = 4;

    public GameObject oarRight;
    public GameObject oarLeft;

    private Animator oarRightAnim;
    private Animator oarLeftAnim;

    private bool paddleInput = false;
    private bool PaddleRight = false;
    private float sidePos;

    private float nextActionTime = 0.0f;
    private float stepInc = 1;
    private bool startPaddle;

    private Vector3 forwardVector3 = Vector3.forward;


    void Start() {
        oarRightAnim = oarRight.GetComponent<Animator>();
        oarLeftAnim = oarLeft.GetComponent<Animator>();

        oarLeftAnim.speed = 0f;
        oarRightAnim.speed = 0f;

        boatRB = GetComponent<Rigidbody>();
        // rotationDirection = -1f; // todo delete
        paddleInput = false;
        sidePos = RotateMin;
        stepInc = (RotateMax - RotateMin) / RotateStep;
    }

    void Update() {
        UserInput();
        AnimatePaddle();
    }

    void FixedUpdate() {
        if (Time.time > nextActionTime) {
            startPaddle = !startPaddle;
            nextActionTime += startPaddle ? PaddleDuration : restDuration;

            sidePos += (Mathf.Abs(sidePos) < Mathf.Abs(RotateMax)) ? stepInc : 0;
        } else {
            if (startPaddle && paddleInput) Paddle();
        }
    }

    void Paddle() {
        boatRB.AddForceAtPosition(boatRB.rotation * forwardVector3 * force,
            boatRB.rotation * new Vector3(sidePos, 0, backPos) + boatRB.position,
            ForceMode.Force);
        Debug.Log("Paddling");
    }

    void AnimatePaddle() {
        if (paddleInput) {
            if (startPaddle) {
                float framePercentage = Mathf.Clamp(1 - ((nextActionTime - Time.time) / PaddleDuration), 0f, 0.99f);
                if (!PaddleRight) {
                    oarRightAnim.Play("Scur", 0, framePercentage);
                    Debug.Log(framePercentage);
                }

                if (!startPaddle) {
                    startPaddle = true;
                    nextActionTime = Time.time + PaddleDuration;
                } else {
                    oarLeftAnim.Play("Scur", 0, framePercentage);
                }
            } else {
                float framePercentage = Mathf.Clamp(1 - ((nextActionTime - Time.time) / restDuration), 0f, 0.99f);
                if (!PaddleRight) {
                    oarRightAnim.Play("Return", 0, framePercentage);
                } else {
                    oarLeftAnim.Play("Return", 0, framePercentage);
                }
            }
        }
    }

    void UserInput() {
        //Steer left
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            paddleInput = true;
            stepInc = -1f * stepInc;
            sidePos = RotateMin * Mathf.Sign(stepInc);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            paddleInput = false;
            {
                PaddleRight = !PaddleRight; // switch direction
                if (PaddleRight) {
                    oarRight.SetActive(false);
                    oarLeft.SetActive(true);
                } else {
                    oarLeft.SetActive(false);
                    oarRight.SetActive(true);
                }
            }
        }
    }
}