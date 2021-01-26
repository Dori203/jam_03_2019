using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

//using UnityEngine.XR.WSA.Input;

public class KillingCamera : ListeningMonoBehaviour {
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<int> {Event = GameManager.Channels.MosquitoesEngaged.GetPath(), Callback = mosquitoesEngaged},
        new BaseListener<int> {Event = GameManager.Channels.MosquitoeHit.GetPath(), Callback = MosquitoeHit}
    };

    public float MosquitoInRangeSensitivity = 0.1f;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private GameObject engagedMosquito;
    [SerializeField] private Quaternion rotationOffset;
    [SerializeField] private bool followRotation = true;
    [SerializeField] private GameObject aim;
    [SerializeField] private bool rotateTowardsMosquitos;


    private bool mosquitoesEngagedMode;

    private void Awake() {
        engagedMosquito = aim; // todo test
    }

    private void LateUpdate() {
        Vector3 newPosition = player.position + positionOffset;
        transform.position = newPosition;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, player.rotation * rotationOffset, 45.0f * Time.deltaTime);

        if (mosquitoesEngagedMode) {
            //get closest mosquito
            Vector3 relativePos = engagedMosquito.transform.position - transform.position;

            //stop rotating once camera is close to pointing at mosquito.
            float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up));
            GameManager.Instance.MosquitoesInCamera(angle < MosquitoInRangeSensitivity);
        }
    }

    private void mosquitoesEngaged(int MosquitoeNumber) {
        if (MosquitoeNumber == -1) {
            mosquitoesEngagedMode = false;
            return;
        }

        if (!mosquitoesEngagedMode) {
            mosquitoesEngagedMode = true;
            engagedMosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
        }
    }

    private int findNextMosquito() {
        //calculate angles
        float minAngle = 360;
        float curAngle = 0;
        int curIndex = -1;
        int minIndex = -1;

        //find minimum angle mosquito
        for (int i = 0; i < ExterminationManager.SharedInstance.getEngagedListSize(); i++) {
            curAngle = calculateAngle(ExterminationManager.SharedInstance.getEngagedMosquitoPositionByIndex(i));
            if (curAngle < minAngle) {
                minAngle = curAngle;
                minIndex = ExterminationManager.SharedInstance.getEngagedMosquitoNumberByIndex(i);
            }
        }

        //Debug.Log("minIndex : " + minIndex);
        return minIndex;
    }

    private float calculateAngle(Vector3 mosquitoPosition) {
        //float angle = Vector3.SignedAngle(this.transform.forward, this.transform.position - mosquitoPosition, this.transform.forward);
        //return Mathf.Abs(angle);
        float angle = Vector3.Angle(this.transform.forward, this.transform.position - mosquitoPosition);
        if (angle > 180) {
            return -1 * (angle - 180);
        }

        return angle;
    }

    private void MosquitoeHit(int MosquitoeNumber) {
        int mosquitoIndex = findNextMosquito();
        engagedMosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(mosquitoIndex);
    }
}