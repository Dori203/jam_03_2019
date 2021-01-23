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


    [BoxGroup("rawing")] public float backPos = 2.54f;

    [BoxGroup("rawing")] public float RotateMin = 0.5f;

    [BoxGroup("rawing")] public float RotateMax = 1.81f;

    [BoxGroup("rawing")] public int RotateStep = 4;
    
    [BoxGroup("rawing")] public float Force = 4;
    
    [BoxGroup("rawing")] public AnimationCurve ForceCurve;

    public GameObject oarRight;
    public GameObject oarLeft;

    private Animator oarRightAnim;
    private Animator oarLeftAnim;

    private bool paddleInput = false;
    private bool PaddleRight = false;
    private bool startPaddle = false;
    private float sidePos;
    private float curForce = 1f;

    private float nextActionTime = 0.0f;
    private float stepInc = 1f;

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
        oarLeft.SetActive(false);
        
        ForceCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.4f, 4), new Keyframe(1, 1));
        ForceCurve.preWrapMode = WrapMode.PingPong;
        ForceCurve.postWrapMode = WrapMode.PingPong;
    }

    void Update() {
        UserInput();
        AnimatePaddle();
    }

    void FixedUpdate() {
        if (paddleInput) {
            //Debug.Log("Time.time : " + Time.time + "\nnextActionTime : " + nextActionTime);
            if (startPaddle) Paddle();
            if (Time.time > nextActionTime) {
                startPaddle = !startPaddle;
                nextActionTime = startPaddle ? Time.time + PaddleDuration : Time.time + restDuration;

                sidePos += (Mathf.Abs(sidePos) < Mathf.Abs(RotateMax)) ? stepInc : 0;
            }
        }
    }

    void Paddle() {
        curForce = Mathf.Clamp(1 - ((nextActionTime - Time.time) / PaddleDuration), 0f, 0.99f);
        curForce = Force * ForceCurve.Evaluate(curForce);

        boatRB.AddForceAtPosition(boatRB.rotation * forwardVector3 * curForce,
            boatRB.rotation * new Vector3(sidePos, 0, backPos) + boatRB.position,
            ForceMode.Force);
        Debug.Log("Paddling");
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
            if (startPaddle) nextActionTime = Time.time + restDuration;
            startPaddle = false;
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
}