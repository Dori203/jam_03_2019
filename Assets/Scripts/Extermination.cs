
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class Extermination : ListeningMonoBehaviour
{
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new Listener {Event = GameManager.Channels.MosquitoeHit.GetPath(), Callback = MosquitoeHit}
    };
    [SerializeField] private ArrayList mosquitosEngaged = new ArrayList();
    [SerializeField] private ArrayList mosquitosInCamera = new ArrayList();
    [SerializeField] private GameObject killingScorePanel;
    [SerializeField] private GameObject scorePanelMark;

    [SerializeField] private AudioClip swat;
    [SerializeField] private AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void MosquitoeHit()
    {
        //add another score to the extermination scores.
        Instantiate(scorePanelMark, killingScorePanel.transform);
        audiosource.PlayOneShot(swat);
    }

    /**
    private void mosquitoesEngaged(GameObject mosquito)
    {
        mosquitosEngaged.Add(mosquito);
    }
    **/
}