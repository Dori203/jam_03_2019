using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helpers;



public class UIManager : ListeningMonoBehaviour
{
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<Victory> {Event = GameManager.Channels.Winning.GetPath(), Callback = activateWinUI},
        new BaseListener<LossConditions> {Event = GameManager.Channels.Losing.GetPath(), Callback = activateLossUI}
    };

    [SerializeField] private GameObject winningUI;
    [SerializeField] private Text winningText;
    [SerializeField] private GameObject losingUI;
    [SerializeField] private Text losingText;

    private void activateWinUI(Victory victoryType)
    {
        Debug.Log("Message received");

        winningText.text = victoryType.Value;
        winningUI.SetActive(true);
    }

    private void activateLossUI(LossConditions lossType)
    {
        Debug.Log("Message received");

        losingText.text = lossType.Value;
        losingUI.SetActive(true);
    }
}
