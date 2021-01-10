using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class AimController : ListeningMonoBehaviour
{
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<bool> {Event = GameManager.Channels.MosquitoesEngaged.GetPath(), Callback = mosquitoesEngaged}
    };


    [SerializeField] private GameObject killingCamera;
    [SerializeField] private float aimSpeed;
    [SerializeField] private bool doriTest;


    public GameObject mosquito;
    [SerializeField] private LayerMask AimLayer;

    private Vector3 targetPosition = new Vector3(-0.4f, -0.4f, 1);
    private bool mosquitoesEngagedMode;
    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (mosquitoesEngagedMode)
        {
            if(targetPosition == null)
            {
                findNextTarget();
            }
            //Move Aim towards mosquito.
            float step = aimSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }

        if (doriTest)
        {
            if (first)
            {
                targetPosition = transform.InverseTransformPoint(mosquito.transform.position);
                Debug.Log("target");
                //Move Aim towards given target.
                Debug.Log(targetPosition);
                first = false;
            }

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, 0.01f);

        }
    }

    private void findNextTarget()
    {
        //TODO: choose randomly a target from all targets on screen.
        Ray r = new Ray(killingCamera.transform.position, mosquito.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 10000, AimLayer))
        {
            targetPosition = hit.point;
            Debug.Log(hit.point);
        }
        //TODO else - find a random target on screen.
    }

    private void mosquitoesEngaged(bool MosquitoesTriggered)
    {
        mosquitoesEngagedMode = MosquitoesTriggered;
    }
}
