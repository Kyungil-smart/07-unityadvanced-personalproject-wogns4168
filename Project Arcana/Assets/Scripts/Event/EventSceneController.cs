using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EventSceneController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button choice1Button;
    [SerializeField] private TMP_Text choice1Text;
    [SerializeField] private Button choice2Button;
    [SerializeField] private TMP_Text choice2Text;
    [SerializeField] private Image eventImage;

    [Header("카드 보상 패널")]
    [SerializeField] private CardRewardPanel cardRewardPanel;

    private EventData _currentEvent;

    private void Start()
    {
        _currentEvent = EventManager.Instance.GetRandomEvent();
        titleText.text = _currentEvent.title;
        descriptionText.text = _currentEvent.description;
        choice1Text.text = _currentEvent.choice1.choiceText;
        choice2Text.text = _currentEvent.choice2.choiceText;
        eventImage.sprite = EventManager.Instance.GetSprite(_currentEvent.spriteIndex);

        choice1Button.onClick.AddListener(() => ApplyChoice(_currentEvent.choice1));
        choice2Button.onClick.AddListener(() => ApplyChoice(_currentEvent.choice2));
    }

    private void ApplyChoice(EventChoice choice)
    {
        choice1Button.interactable = false;
        choice2Button.interactable = false;

        switch (choice.effectType)
        {
            case EventEffectType.HealHp:
                float healAmount = RunManager.Instance.MaxHp * choice.value;
                float healedHp = Mathf.Min(RunManager.Instance.CurrentHp + healAmount, RunManager.Instance.MaxHp);
                RunManager.Instance.SavePlayerHp(healedHp, RunManager.Instance.MaxHp);
                Leave();
                break;

            case EventEffectType.GainGold:
                RunManager.Instance.AddGold((int)choice.value);
                Leave();
                break;

            case EventEffectType.GainCard:
                List<CardData> cards1 = RunManager.Instance.GetRandomRewardCards(3);
                cardRewardPanel.Show(cards1, Leave, 1);
                break;

            case EventEffectType.GainCards:
                List<CardData> cards2 = RunManager.Instance.GetRandomRewardCards(3);
                cardRewardPanel.Show(cards2, Leave, 2);
                break;

            case EventEffectType.LoseHpAndGainCard:
                if (LoseHp(choice.value)) return;
                List<CardData> loseHpCards = RunManager.Instance.GetRandomRewardCards(3);
                cardRewardPanel.Show(loseHpCards, Leave, 1);
                break;

            case EventEffectType.LoseHpAndGainGold:
                if (LoseHp(choice.value)) return;
                RunManager.Instance.AddGold(30);
                Leave();
                break;

            case EventEffectType.LoseHalfHpAndGainCards:
                if (LoseHp(RunManager.Instance.CurrentHp * choice.value)) return;
                List<CardData> halfHpCards = RunManager.Instance.GetRandomRewardCards(3);
                cardRewardPanel.Show(halfHpCards, Leave, 2);
                break;

            case EventEffectType.LoseGoldAndGainCards:
                RunManager.Instance.AddGold(-Mathf.Min((int)choice.value, RunManager.Instance.Gold));
                List<CardData> goldCards = RunManager.Instance.GetRandomRewardCards(3);
                cardRewardPanel.Show(goldCards, Leave, 2);
                break;
        }
    }

    // true 반환 시 게임오버 (이후 로직 중단)
    private bool LoseHp(float amount)
    {
        float newHp = Mathf.Max(RunManager.Instance.CurrentHp - amount, 0f);
        RunManager.Instance.SavePlayerHp(newHp, RunManager.Instance.MaxHp);

        if (newHp <= 0f)
        {
            SceneManager.LoadScene("EndingScene");
            return true;
        }
        return false;
    }

    private void Leave()
    {
        MapManager.Instance.OnNodeCleared();
    }
}