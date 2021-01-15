using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Helpers.Extensions;

public class GameManager : Singleton<GameManager>, IDestroyable {
    protected GameManager() { }
    
    public bool ShouldDestroyOnLoad() => true;
    
    public void OnSceneChanged(Scene previousScene, Scene nextScene) { }

    [SerializeField] private int exterminationScore;
    [SerializeField] private int exterminationVictoryThreshold;

    [SerializeField] private int explorationScore;
    [SerializeField] private int explorationVictoryThreshold;

    [SerializeField] private int fishingScore;
    [SerializeField] private int fishingVictoryThreshold;

    [SerializeField] private GameObject winningUI;
    [SerializeField] private Text winningText;



    public enum Channels
    {
        MosquitoesEngaged,
        MosquitoesInCamera,
        MosquitoeHit
    }


        //TODO - remove update and add the victory checking to the score increase.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            explorationScore++;
        }
        //check victory
        Victory victory = checkVictory();
        if (victory.Value != Victory.None.Value)
        {
            //stop the game, activate victory scenario.
            //TEMP stop the game, show UI victory overlay, assign victory type to text.
            Debug.Log(victory.Value);
            Time.timeScale = 0;
            winningText.text = victory.Value;
            winningUI.SetActive(true);
        }
    }

    private Victory checkVictory()
    {
        //check each victory condition, return
        if(exterminationScore >= exterminationVictoryThreshold)
        {
            Debug.Log("extermination victory");
            return Victory.Extermination;
        }
        else if(explorationScore >= explorationVictoryThreshold)
        {
            Debug.Log("exploration victory");
            return Victory.Exploration;
        }
        else if(fishingScore >= fishingVictoryThreshold)
        {
            Debug.Log("fishing victory");
            return Victory.Fishing;
            
        }
        return Victory.None;
    }

    public void MosquitoesInCamera(bool isMosquitoesInCamera)
    {
        Messenger<bool>.Broadcast(Channels.MosquitoesInCamera.GetPath(), isMosquitoesInCamera, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public void MosquitoesTriggered(bool MosquitoesTriggeredMode) {
        Messenger<bool>.Broadcast(Channels.MosquitoesEngaged.GetPath(), MosquitoesTriggeredMode, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public void MosquitoeHit(int MosquitoeNumber)
    {
        Messenger<int>.Broadcast(Channels.MosquitoeHit.GetPath(), MosquitoeNumber, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}