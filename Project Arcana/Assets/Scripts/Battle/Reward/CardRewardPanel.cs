using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardRewardPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform[] cardSlots; // 카드 슬롯 3개
    [SerializeField] private Button skipButton;
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> _spawnedCards = new List<GameObject>();
    private System.Action _onComplete;

    private void Start()
    {
        panel.SetActive(false);
        skipButton.onClick.AddListener(OnSkip);
    }

    public void Show(List<CardData> rewardCards, System.Action onComplete)
    {
        _onComplete = onComplete;
        panel.SetActive(true);

        // 기존 카드 정리
        foreach (var card in _spawnedCards)
            Destroy(card);
        _spawnedCards.Clear();

        // 카드 3장 생성
        for (int i = 0; i < rewardCards.Count && i < cardSlots.Length; i++)
        {
            GameObject obj = Instantiate(cardPrefab, cardSlots[i]);
            RectTransform cardRect = obj.GetComponent<RectTransform>();
            cardRect.localScale = Vector3.one * 40f; // 4x5 → 200x250 정도
            cardRect.anchoredPosition = Vector2.zero;
            obj.GetComponent<CardView>().Setup(rewardCards[i]);

            // CardDragArrow 비활성화 (보상 화면에서는 드래그 불필요)
            CardDragArrow dragArrow = obj.GetComponent<CardDragArrow>();
            if (dragArrow != null) dragArrow.enabled = false;

            // 보상 카드 클릭 스크립트 추가
            RewardCardInteraction interaction = obj.AddComponent<RewardCardInteraction>();
            interaction.Setup(rewardCards[i], OnCardSelected);

            _spawnedCards.Add(obj);
        }
    }

    private void OnCardSelected(CardData selectedCard)
    {
        // RunManager 덱에 추가
        RunManager.Instance.AddCardToDeck(selectedCard);
        Debug.Log($"{selectedCard.cardName} 덱에 추가!");

        Hide();
        _onComplete?.Invoke();
    }

    private void OnSkip()
    {
        Hide();
        _onComplete?.Invoke();
    }

    private void Hide()
    {
        panel.SetActive(false);
        foreach (var card in _spawnedCards)
            Destroy(card);
        _spawnedCards.Clear();
    }
}