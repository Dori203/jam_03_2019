using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

//using UnityEngine.XR.WSA.Input;

public class AimController : ListeningMonoBehaviour {
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<bool>
            {Event = GameManager.Channels.MosquitoesInCamera.GetPath(), Callback = mosquitoesInCameraMode},
        new BaseListener<int> {Event = GameManager.Channels.MosquitoesEngaged.GetPath(), Callback = mosquitoesEngaged},
        new BaseListener<int> {Event = GameManager.Channels.MosquitoeNext.GetPath(), Callback = mosquitoesNext},
        new BaseListener<int> {Event = GameManager.Channels.MosquitoeHit.GetPath(), Callback = MosquitoeHit}
    };

    private IEnumerator coroutine;

    [SerializeField] private Camera killingCamera;
    [SerializeField] private float aimSpeed;
    [SerializeField] private bool doriTest;
    [SerializeField] private GameObject engagedMosquito;

    [SerializeField] private LayerMask mosquitosLayer;

    private Vector3 targetPosition;
    private bool mosquitoesEngagedMode;
    private bool mosquitoesInCamera;

    private bool first = true;
    private bool targetSet = false;

    void Awake() {
        aimSpeed = 0.1f;
    }

    void Update() {
        // if (mosquitoesInCamera) {
        //     StartCoroutine(findNextTarget());
        //
        //     if (targetSet) {
        //         {
        //             // transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, aimSpeed);
        //             if (engagedMosquito != null) {
        //                 transform.localPosition = Vector3.MoveTowards(transform.localPosition,
        //                     engagedMosquito.transform.position, aimSpeed);
        //             }
        //         }
        //     }
        // }
        
        if (engagedMosquito != null && mosquitoesInCamera) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, aimSpeed);
            // transform.localPosition = Vector3.MoveTowards(transform.localPosition,
            //     engagedMosquito.transform.position, aimSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            RaycastHit hit;
            Vector3 dir = transform.position - killingCamera.transform.position;

            int layerMask = 1 << 15;
            //try to hit mosquitos only.
            if (Physics.Raycast(killingCamera.transform.position, dir, out hit, 1000f, layerMask)) {
                Debug.Log("hit!");
                Debug.Log(hit.transform.name);
                // GameManager.Instance.MosquitoeHit(0); //todo change to MosquitoeNumber
                hit.transform.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator aimToTarget() {
        //wait for 0.5 a second.
        yield return new WaitForSeconds(0.5f);
    
        RaycastHit hit;
    
        //Make Ray hit only aim layer.
        int layerMask = 1 << 18;
        Vector3 dir2 = engagedMosquito.transform.position - killingCamera.transform.position;
        if (Physics.Raycast(killingCamera.transform.position, dir2, out hit, 10000, layerMask)) {
            targetPosition = killingCamera.transform.InverseTransformPoint(hit.point) - hit.transform.localPosition;
            targetPosition.z = transform.localPosition.z;
            targetSet = true;
        }
    }

    private void mosquitoesEngaged(int MosquitoeNumber) {
        if (MosquitoeNumber == -1) {
            mosquitoesEngagedMode = false;
        }
        if (!mosquitoesEngagedMode) { // first mosquito
            mosquitoesEngagedMode = true;
            engagedMosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
        }
    }
    
    private void mosquitoesNext(int MosquitoeNumber) {
        engagedMosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
    }

    private void mosquitoesInCameraMode(bool MosquitoesInCameraTriggered) {
        mosquitoesInCamera = MosquitoesInCameraTriggered;
        aimToTarget();
    }

    private void MosquitoeHit(int MosquitoeNumber) {
        mosquitoesInCamera = false;
    }
}