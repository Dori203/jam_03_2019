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
    [SerializeField] private GameObject engagedMosquito;
    [SerializeField] private Animator aimAnimator;
    [SerializeField] private LayerMask mosquitosLayer;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip swatSound;
    [SerializeField] private float minRandomPositionTime;
    [SerializeField] private float maxRandomPositionTime;
    [SerializeField] private float randomPositionRadius;
    [SerializeField] private float delayBetweenShots;

    private float lastShotTime;
    private Vector3 targetPosition;
    private bool mosquitoesEngagedMode;
    private bool mosquitoesInCamera;
    private Vector3 velocity = Vector3.zero;
    private bool waiting = false;
    private float timer = 0f;
    private Vector3 initialLocalPos;

    void Awake() {
        aimSpeed = 0.1f;
        initialLocalPos = transform.localPosition;
        lastShotTime = Time.time;
    }

    void Update() {

        if (engagedMosquito != null && mosquitoesInCamera) {

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                lerpPosition();
            }
            else
            {
                transform.localPosition =  Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity,  0.5f);
                //transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, aimSpeed);
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if((Time.time - lastShotTime > delayBetweenShots))
            {
                lastShotTime = Time.time;
                RaycastHit hit;
                Vector3 dir = transform.position - killingCamera.transform.position;

                int layerMask = 1 << 15;
                //try to hit mosquitos only.
                if (Physics.Raycast(killingCamera.transform.position, dir, out hit, 1000f, layerMask))
                {
                    Debug.Log("hit!");

                    GameObject mosquito = hit.transform.gameObject;
                    MosquitoController mosquitoController = mosquito.GetComponent<MosquitoController>();
                    int MosquitoeNumber = mosquitoController.MosquitoeNumber;

                    aimAnimator.Play("swat");
                    audio.PlayOneShot(swatSound);

                    ExterminationManager.SharedInstance.MosquitoeHit(MosquitoeNumber);

                    hit.transform.gameObject.SetActive(false);
                }
            }

            
        }
    }

    IEnumerator aimToTarget() {
        //wait for 0.5 a second.
        yield return new WaitForSeconds(0.1f);
        //StartCountdown(minRandomPositionTime, maxRandomPositionTime);

        RaycastHit hit;
    
        //Make Ray hit only aim layer.
        int layerMask = 1 << 18;
        Vector3 dir2 = engagedMosquito.transform.position - killingCamera.transform.position;
        yield return new WaitForSeconds(0.1f);
        if (Physics.Raycast(killingCamera.transform.position, dir2, out hit, 10000, layerMask)) {
            targetPosition = killingCamera.transform.InverseTransformPoint(hit.point) - hit.transform.localPosition;
            targetPosition.z = transform.localPosition.z;

            mosquitoesInCamera = true;
        }
    }

    private void lerpPosition()
    {
        targetPosition = initialLocalPos +  new Vector3(Random.Range(randomPositionRadius, -1*randomPositionRadius), Random.Range(randomPositionRadius, -1 * randomPositionRadius),0);
        StartCountdown(minRandomPositionTime, maxRandomPositionTime);

    }

    private void mosquitoesEngaged(int MosquitoeNumber) {
        if (MosquitoeNumber == -1) {
            mosquitoesEngagedMode = false;
            return;
        }
        if (!mosquitoesEngagedMode) { // first mosquito
            mosquitoesEngagedMode = true;
            engagedMosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
        }
    }
    
    private void mosquitoesNext(int MosquitoeNumber) {
        engagedMosquito = MosquitoSpawner.SharedInstance.GetPooledObjectByIndex(MosquitoeNumber);
    }

    private void mosquitoesInCameraMode(bool MosquitoesInCamera) {
        if (!mosquitoesEngagedMode) return;
        if (MosquitoesInCamera) {
            //StartCoroutine(aimToTarget());
            mosquitoesInCamera = true;
            return;
        } else {
            mosquitoesInCamera = false; // todo delete
        }
    }

    private void MosquitoeHit(int MosquitoeNumber) {
        mosquitoesInCamera = false;
    }

    private void StartCountdown(float min, float max)
    {
        timer = Random.Range(min, max);
    }
}