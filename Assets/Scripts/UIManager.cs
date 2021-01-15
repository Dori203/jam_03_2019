using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helpers;



public class UIManager : ListeningMonoBehaviour
{
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<Victory> {Event = GameManager.Channels.Winning.GetPath(), Callback = activateWinUI}
    };

    [SerializeField] private GameObject winningUI;
    [SerializeField] private Text winningText;

    private void activateWinUI(Victory victoryType)
    {
        Debug.Log("Message received");

        winningText.text = victoryType.Value;
        winningUI.SetActive(true);
    }
}
