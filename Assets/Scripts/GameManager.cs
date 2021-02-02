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

    [SerializeField] private int exterminationMaxHealth;
    [SerializeField] private int exterminationHealth;
    [SerializeField] private int exterminationDeathThreshold;

    [SerializeField] private int explorationMaxHealth = 3;
    [SerializeField] private int explorationHealth = 3;
    [SerializeField] private int explorationDeathThreshold = 0; //TODO RANDOM NUMBER

    [SerializeField] private int fishingMaxHealth;
    [SerializeField] private int fishingHealth;
    [SerializeField] private int fishingDeathThreshold;

    [SerializeField] private Animator raftHealthAnim;
    [SerializeField] private Animator explorationBarAnim;

    [SerializeField] private AudioSource explorationSuccess;

    public enum Channels
    {
        MosquitoesEngaged,
        MosquitoesInCamera,
        MosquitoeHit,
        Winning,
        InterestPointHit,
        MosquitoeNext,
        NewFishCaught,
        Losing,
        HealthUpdate
    }

    public enum HealthType
    {
        Fishing,
        Exploration,
        Extermination,
        All
    }

    public void incExterminationScore()
    {
        exterminationScore++;
        checkVictory();
    }

    public void incExplorationScore()
    {
        explorationScore++;
        explorationSuccess.Play();
        checkVictory();
    }

    public void incFishingScore()
    {
        fishingScore++;
        checkVictory();
    }

    public void decFishingHealth(int amount)
    {
        fishingHealth = fishingHealth - amount;
        if(fishingHealth < fishingDeathThreshold)
        {
            if (FishingManager.SharedInstance.consumeFish())
            {
                Debug.Log("consume fish from game manager");
                // a fish was consumed, restart count.
                fishingHealth = fishingMaxHealth;
            }
        }
        HealthUpdate(HealthType.Fishing);
        checkLoss();
    }

    public void incExterminationHealth(int amount)
    {
        if(exterminationHealth < exterminationMaxHealth)
        {
            exterminationHealth += amount;
            HealthUpdate(HealthType.Extermination);
        }
    }

    public void decExplorationHealth(int amount)
    {
        explorationHealth = explorationHealth - amount;
        HealthUpdate(HealthType.Exploration);
        raftHealthAnim.SetInteger("hp", explorationHealth);
        explorationBarAnim.SetInteger("hp", explorationHealth);
        checkLoss();
    }

    public void decExterminationHealth(int amount)
    {
        exterminationHealth = exterminationHealth - amount;
        HealthUpdate(HealthType.Extermination);
        checkLoss();
    }

    public float getFishingHealthRatio()
    {
        return (float) fishingHealth / fishingMaxHealth;
    }

    public float getExterminationHealthRatio()
    {
        return (float)exterminationHealth / exterminationMaxHealth;
    }

    public float getExplorationHealthRatio()
    {
        return (float)explorationHealth / explorationMaxHealth;
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
            /*
            Time.timeScale = 0;
            */
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

    public void VictoryMessage(Victory victoryType)
    {
        Messenger<Victory>.Broadcast(Channels.Winning.GetPath(), victoryType, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    private void LossMessage(LossConditions lossType)
    {
        Debug.Log("SentLossMessage");
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

    public void HealthUpdate(HealthType healthType)
    {
        Debug.Log("Sent health refresh message");
        Messenger<HealthType>.Broadcast(Channels.HealthUpdate.GetPath(), healthType, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}