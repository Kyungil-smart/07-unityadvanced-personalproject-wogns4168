using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject dimPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button goldButton;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Button cardRewardButton;
    [SerializeField] private TMP_Text cardRewardButtonText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button gameOverTitleButton;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowVictory(int goldReward, Action onGoldCollect, Action onCardReward)
    {
        dimPanel.SetActive(true);
        panel.SetActive(true);
        resultText.text = "전리품!";

        // 골드 버튼 설정
        goldText.text = $"{goldReward} 골드";
        goldButton.gameObject.SetActive(true);
        goldButton.onClick.RemoveAllListeners();
        goldButton.onClick.AddListener(() =>
        {
            RunManager.Instance.AddGold(goldReward);
            goldButton.gameObject.SetActive(false); // 골드 버튼 숨기기
        });

        // 카드 보상 버튼
        cardRewardButton.gameObject.SetActive(true);
        cardRewardButton.onClick.RemoveAllListeners();
        cardRewardButton.onClick.AddListener(() =>
        {
            Hide();
            onCardReward?.Invoke();
        });
    }

    public void ShowDefeat(Action onContinue)
    {
        dimPanel.SetActive(true);
        panel.SetActive(false); // 기존 패널 숨기고
        gameOverPanel.SetActive(true); // 게임오버 패널 표시

        gameOverTitleButton.onClick.RemoveAllListeners();
        gameOverTitleButton.onClick.AddListener(() =>
        {
            onContinue?.Invoke();
        });
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
    
    public void HideAll()
    {
        dimPanel.SetActive(false);
        panel.SetActive(false);
    }
}