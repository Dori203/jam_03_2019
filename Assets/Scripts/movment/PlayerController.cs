using System.Collections;
using System.Collections.Generic;
using Ditzelgames;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour { 
    //Drags
    public Transform boatTransform;

    //How fast should the engine accelerate?
    public float powerFactor;

    public float maxSpeed;

    public float rotationSpeed;

    private float thrustFromWaterJet = 0f;

    private Rigidbody boatRB;

    private float boatRotation_Y = 0f;

    private bool directionFlip;

    void Start() 
	{
        boatRB = GetComponent<Rigidbody>();
    }


    void Update() 
	{
        UserInput();
    }

    void FixedUpdate()
    {
        Rotate();
        UpdateWaterJet();
    }

    void UserInput() {
        //Steer left
        if (Input.GetKey(KeyCode.D))
        {
            boatRotation_Y = boatTransform.localEulerAngles.y + rotationSpeed;
        }
        //Steer right
        else if (Input.GetKey(KeyCode.A))
        {
            boatRotation_Y = boatTransform.localEulerAngles.y - rotationSpeed;
        }
    }

    void Rotate()
    {
        Vector3 newRotation = new Vector3(0f, boatRotation_Y, 0f);
        boatTransform.localEulerAngles = newRotation;
    }

    void UpdateWaterJet()
    {
        PhysicsHelper.ApplyForceToReachVelocity(boatRB, boatTransform.forward * powerFactor, 1f);
        if(boatRB.velocity.magnitude > maxSpeed){
            boatRB.velocity = Vector3.ClampMagnitude(boatRB.velocity, maxSpeed);
        }
        //
        // Debug.Log(boatController.CurrentSpeed);
        //
        // Vector3 forceToAdd = waterJetTransform.forward * currentJetPower;
        //
        // //Only add the force if the engine is below sea level
        // // float waveYPos = WaterController.current.GetWaveYPos(waterJetTransform.position, Time.time);
        // float waveYPos = 0.77f;
        //
        // if (waterJetTransform.position.y < waveYPos)
        // {
        //     boatRB.AddForceAtPosition(forceToAdd, waterJetTransform.position);
        // }
        // else
        // {
        //     boatRB.AddForceAtPosition(Vector3.zero, waterJetTransform.position);
        // }
    }
}
