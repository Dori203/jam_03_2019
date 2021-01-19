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

    [SerializeField] private int exterminationScore = 0;
    [SerializeField] private int exterminationVictoryThreshold = 20; //TODO RANDOM NUMBER

    [SerializeField] private int explorationScore = 0;
    [SerializeField] private int explorationVictoryThreshold = 100; //TODO RANDOM NUMBER

    [SerializeField] private int fishingScore = 0;
    [SerializeField] private int fishingVictoryThreshold = 20; //TODO RANDOM NUMBER

    [SerializeField] private int exterminationHealth = 40;
    [SerializeField] private int exterminationDeathThreshold = 0; //TODO RANDOM NUMBER

    [SerializeField] private int explorationHealth = 3;
    [SerializeField] private int explorationDeathThreshold = 0; //TODO RANDOM NUMBER

    [SerializeField] private int fishingHealth = 20;
    [SerializeField] private int fishingDeathThreshold = 0; //TODO RANDOM NUMBER

    public enum Channels
    {
        MosquitoesEngaged,
        MosquitoesInCamera,
        MosquitoeHit,
        Winning,
        InterestPointHit,
        MosquitoeNext,
        NewFishCaught,
        Losing
    }


    public void incExterminationScore()
    {
        exterminationScore++;
        checkVictory();
    }

    public void incExplorationScore()
    {
        explorationScore++;
        checkVictory();
    }

    public void incFishingScore()
    {
        fishingScore++;
        checkVictory();
    }

    public void decFishingHealth()
    {
        fishingHealth--;
        checkLoss();
    }

    public void decExplorationHealth(int amount)
    {
        explorationHealth = explorationHealth - amount;
        checkLoss();
    }

    public void decExterminationHealth()
    {
        Debug.Log("gotmikk");
        exterminationHealth--;
        checkLoss();
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
            Debug.Log("fishing victory");
            isGameOver = true;
            victoryType = Victory.Fishing;
        }

        if (isGameOver)
        {
            VictoryMessage(victoryType);
            Time.timeScale = 0;
        }
    }

    private void checkLoss()
    {
        bool isGameOver = false;
        LossConditions lossType = LossConditions.None;

        //check each loss condition, return
        if (exterminationHealth < exterminationDeathThreshold)
        {
            Debug.Log("extermination loss");
            isGameOver = true;
            lossType = LossConditions.Extermination;
        }
        else if (explorationHealth < explorationDeathThreshold)
        {
            Debug.Log("exploration loss");
            isGameOver = true;
            lossType = LossConditions.Exploration;
        }
        else if (fishingHealth < fishingDeathThreshold)
        {
            Debug.Log("Fishing loss");
            isGameOver = true;
            lossType = LossConditions.Fishing;
        }

        if (isGameOver)
        {
            LossMessage(lossType);
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
    
    public void MosquitoeNext(int MosquitoeNumber)
    {
        Messenger<int>.Broadcast(Channels.MosquitoeNext.GetPath(), MosquitoeNumber, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    private void VictoryMessage(Victory victoryType)
    {
        Messenger<Victory>.Broadcast(Channels.Winning.GetPath(), victoryType, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    private void LossMessage(LossConditions lossType)
    {
        Messenger<LossConditions>.Broadcast(Channels.Losing.GetPath(), lossType, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public void InterestPointHit(int interetPointType)
    {
        Messenger<int>.Broadcast(Channels.InterestPointHit.GetPath(), interetPointType, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    public void NewFishCaught(int FishType)
    {
        Messenger<int>.Broadcast(Channels.NewFishCaught.GetPath(), FishType, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}