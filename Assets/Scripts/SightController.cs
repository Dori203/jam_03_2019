using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class SightController : ListeningMonoBehaviour {
    private bool mosquitoesEngagedMode;

    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<bool> {Event = GameManager.Channels.MosquitoesEngaged.GetPath(), Callback = mosquitoesEngaged}
    };
    
    [SerializeField] public Transform camera;
    
    public float angle = 0;
    public float speed = (2 * Mathf.PI) / 5f; //2*PI in degress is 360, so you get 5 seconds to complete a circle
    public float radius = 5;
    private float x;
    private float y;

    void Update() {
        angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
        x = Mathf.Cos(angle) * radius;
        y = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, 9);
    }

    private void mosquitoesEngaged(bool MosquitoesTriggered) {
        mosquitoesEngagedMode = MosquitoesTriggered;
    }
}