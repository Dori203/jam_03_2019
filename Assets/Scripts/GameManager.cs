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





    public enum Channels
    {
        MosquitoesEngaged,
        MosquitoesInCamera,
        MosquitoeHit,
        Winning
    }


    public void incExterminationScore()
    {
        exterminationScore++;
        checkVictory();
    }

    private void checkVictory()
    {
        bool isGameOver = false;
        Victory victoryType = Victory.None;
        //check each victory condition, return
        if(exterminationScore >= exterminationVictoryThreshold)
        {
            Debug.Log("extermination victory");
            isGameOver = true;
            victoryType = Victory.Extermination;
        }
        else if(explorationScore >= explorationVictoryThreshold)
        {
            Debug.Log("exploration victory");
            isGameOver = true;
            victoryType = Victory.Exploration;
        }
        else if(fishingScore >= fishingVictoryThreshold)
        {
            isGameOver = true;
            victoryType = Victory.Fishing;
        }

        if (isGameOver)
        {
            VictoryMessage(victoryType);
            Time.timeScale = 0;
        }
    }

    public void MosquitoesInCamera(bool isMosquitoesInCamera)
    {
        Messenger<bool>.Broadcast(Channels.MosquitoesInCamera.GetPath(), isMosquitoesInCamera, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public void MosquitoesTriggered(int MosquitoeNumber) {
        Messenger<int>.Broadcast(Channels.MosquitoesEngaged.GetPath(), MosquitoeNumber, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public void MosquitoeHit(int MosquitoeNumber)
    {
        Messenger<int>.Broadcast(Channels.MosquitoeHit.GetPath(), MosquitoeNumber, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    private void VictoryMessage(Victory victoryType)
    {
        Messenger<Victory>.Broadcast(Channels.Winning.GetPath(), victoryType, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}