using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helpers;



public class UIManager : ListeningMonoBehaviour
{
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<Victory> {Event = GameManager.Channels.Winning.GetPath(), Callback = activateWinUI},
        new BaseListener<LossConditions> {Event = GameManager.Channels.Losing.GetPath(), Callback = activateLossUI},
        new BaseListener<GameManager.HealthType> {Event = GameManager.Channels.HealthUpdate.GetPath(), Callback = updateHealthBars}
    };

    [SerializeField] private GameObject winningUI;
    [SerializeField] private Text winningText;
    [SerializeField] private GameObject losingUI;
    [SerializeField] private Text losingText;
    [SerializeField] private GameObject fishingHealthBar;
    [SerializeField] private GameObject exterminationHealthBar;
    private Image fishingHealthImage;
    private Image exterminationHealthImage;


    private void Awake()
    {
        fishingHealthImage = fishingHealthBar.GetComponent<Image>();
        exterminationHealthImage = exterminationHealthBar.GetComponent<Image>();
        updateHealthBars(GameManager.HealthType.All);
    }

    private void activateWinUI(Victory victoryType)
    {

        winningText.text = victoryType.Value;
        winningUI.SetActive(true);
    }

    private void activateLossUI(LossConditions lossType)
    {
        Debug.Log("Loss UI updated");   
        losingText.text = lossType.Value;
        losingUI.SetActive(true);
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            updateHealthBars(0);
        }
    }

    private void updateHealthBars(GameManager.HealthType healthType)
    {
        switch (healthType)
        {
            case GameManager.HealthType.Fishing:
                fishingHealthImage.fillAmount = GameManager.Instance.getFishingHealthRatio();
                break;

            case GameManager.HealthType.Extermination:
                exterminationHealthImage.fillAmount = GameManager.Instance.getExterminationHealthRatio();
                break;

            case GameManager.HealthType.All:
                fishingHealthImage.fillAmount = GameManager.Instance.getFishingHealthRatio();
                exterminationHealthImage.fillAmount = GameManager.Instance.getExterminationHealthRatio();
                break;
        }

    }
}
