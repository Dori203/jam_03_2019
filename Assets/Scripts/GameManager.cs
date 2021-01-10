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
    
    public enum Channels {
        MosquitoesEngaged
    }
    
    public void MosquitoesTriggered(bool MosquitoesTriggeredMode) {
        Messenger<bool>.Broadcast(Channels.MosquitoesEngaged.GetPath(), MosquitoesTriggeredMode, MessengerMode.DONT_REQUIRE_LISTENER);
    }

}