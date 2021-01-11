using System.Collections;
using System.Collections.Generic;
using Ditzelgames;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour { 
    //Drags
    public Transform boatTransform;
    private Rigidbody boatRB;

    //How fast should the engine accelerate?
    public float powerFactor;
    public float maxSpeed;
    public float rotationSpeed;
    private float thrustFromWaterJet = 0f;
    private float boatRotation_Y = 0f;
    private float rotationDirection = -1f;
    private bool rotating = false;
    void Start() 
	{
        boatRB = GetComponent<Rigidbody>();
        rotationDirection = -1f;
        rotating = false;
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
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rotating = true;
            rotationDirection *= -1f;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            rotating = false;
        }
        
        // todo delete
        if (Input.GetKeyUp(KeyCode.T))
        {
            GameManager.Instance.MosquitoesTriggered(true);
            Debug.Log("Triggered!");
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            GameManager.Instance.MosquitoesTriggered(false);
        }
        // todo delete
        
    }

    void Rotate()
    {
        if (rotating)
        {
            boatRotation_Y = boatTransform.localEulerAngles.y + rotationSpeed * rotationDirection;
            Vector3 newRotation = new Vector3(0f, boatRotation_Y, 0f);
            boatTransform.localEulerAngles = newRotation;
        }
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
