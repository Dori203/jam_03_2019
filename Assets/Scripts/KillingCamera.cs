using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
//using UnityEngine.XR.WSA.Input;

public class KillingCamera : ListeningMonoBehaviour {
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<int> {Event = GameManager.Channels.MosquitoesEngaged.GetPath(), Callback = mosquitoesEngaged },
        new BaseListener<int> {Event = GameManager.Channels.MosquitoeHit.GetPath(), Callback = MosquitoeHit }
    };

    public float MosquitoInRangeSensitivity = 0.1f;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 engagedPosition;
    [SerializeField] private Quaternion rotationOffset;
    [SerializeField] private bool followRotation;
    [SerializeField] private GameObject aim;




    private bool mosquitoesEngagedMode;

    private void Awake()
    {
        engagedPosition = Vector3.forward;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = player.position + positionOffset;
        transform.position = newPosition;

        //cruising.
        if (followRotation && !mosquitoesEngagedMode)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, player.rotation * rotationOffset, 45.0f * Time.deltaTime);
        //engaged in battle.
        } else if (mosquitoesEngagedMode) {
            aim.SetActive(true);

            //get closest mosquito
            int mosquitoIndex = findNextMosquito();
            //GameObject mosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(mosquitoIndex);
            Vector3 relativePos = engagedPosition - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 45.0f * Time.deltaTime);

            //stop rotating once camera is close to pointing at mosquito.
            float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up));
            if (angle < MosquitoInRangeSensitivity)
            {
                GameManager.Instance.MosquitoesInCamera(true);
            }
        }
    }

    private void mosquitoesEngaged(int MosquitoesTriggered) {
        mosquitoesEngagedMode = true; 
    }

    private int findNextMosquito()
    {
        //calculate angles
        float minAngle = 360;
        float curAngle = 0;
        int minIndex = -1;

        //find minimum angle mosquito
        for (int i = 0; i < ExterminationManager.SharedInstance.getEngagedListSize(); i++)
        {
            curAngle = calculateAngle(ExterminationManager.SharedInstance.getEngagedMosquitoPositionByIndex(i));
            if(curAngle < minAngle)
            {
                minAngle = curAngle;
                minIndex = i;
            }
        }
        return minIndex;
    }

    private float calculateAngle(Vector3 mosquitoPosition)
    {
        //float angle = Vector3.SignedAngle(this.transform.forward, this.transform.position - mosquitoPosition, this.transform.forward);
        //return Mathf.Abs(angle);
        float angle = Vector3.Angle(this.transform.forward, this.transform.position - mosquitoPosition);
        if(angle > 180)
        {
            return -1 * (angle - 180);
        }
        return angle;
    }

    private void MosquitoeHit(int MosquitoeNumber) {
        int mosquitoIndex = findNextMosquito();
        engagedPosition = MosquitoSpawner.SharedInstance.getMosquitoPositionByIndex(mosquitoIndex);
    }
}
