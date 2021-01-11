using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
//using UnityEngine.XR.WSA.Input;

public class KillingCamera : ListeningMonoBehaviour {
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<bool> {Event = GameManager.Channels.MosquitoesEngaged.GetPath(), Callback = mosquitoesEngaged}
    };

    public float MosquitoInRangeSensitivity = 0.1f;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Quaternion rotationOffset;
    [SerializeField] private bool followRotation;
    [SerializeField] private GameObject aim;


    private bool mosquitoesEngagedMode;

    public GameObject Mosquito;
        
    private void LateUpdate()
    {
        Vector3 newPosition = player.position + positionOffset;
        transform.position = newPosition;
        if (followRotation && !mosquitoesEngagedMode)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, player.rotation * rotationOffset, 45.0f * Time.deltaTime);
        } else if (mosquitoesEngagedMode) {
            aim.SetActive(true);
            Vector3 relativePos = Mosquito.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 45.0f * Time.deltaTime);
            float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up));
            if (angle < MosquitoInRangeSensitivity)
            {
                GameManager.Instance.MosquitoesInCamera(true);
            }
        }
    }

    private void mosquitoesEngaged(bool MosquitoesTriggered) {
        mosquitoesEngagedMode = MosquitoesTriggered;
    }
    
}
