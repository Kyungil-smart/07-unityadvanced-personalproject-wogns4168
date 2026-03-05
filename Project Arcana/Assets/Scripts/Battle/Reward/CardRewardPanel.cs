using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardRewardPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private Button skipButton;
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> _spawnedCards = new List<GameObject>();
    private System.Action _onComplete;
    private int _selectCount;
    private int _selectedCount;

    private void Start()
    {
        panel.SetActive(false);
        skipButton.onClick.AddListener(OnSkip);
    }

    public void Show(List<CardData> rewardCards, System.Action onComplete, int selectCount = 1)
    {
        _onComplete = onComplete;
        _selectCount = selectCount;
        _selectedCount = 0;
        panel.SetActive(true);

        foreach (var card in _spawnedCards)
            PoolManager.Instance.Despawn(card, cardPrefab);
        _spawnedCards.Clear();

        for (int i = 0; i < rewardCards.Count && i < cardSlots.Length; i++)
        {
            GameObject obj = PoolManager.Instance.Spawn(cardPrefab);
            obj.transform.SetParent(cardSlots[i], false);
            RectTransform cardRect = obj.GetComponent<RectTransform>();
            cardRect.localScale = Vector3.one * 40f;
            cardRect.anchoredPosition = Vector2.zero;
            obj.GetComponent<CardView>().Setup(rewardCards[i]);

            CardDragArrow dragArrow = obj.GetComponent<CardDragArrow>();
            if (dragArrow != null) dragArrow.enabled = false;

            RewardCardInteraction interaction = obj.GetComponent<RewardCardInteraction>() 
                                                ?? obj.AddComponent<RewardCardInteraction>();
            interaction.Setup(rewardCards[i], OnCardSelected);

            _spawnedCards.Add(obj);
        }
    }


    private void OnCardSelected(CardData selectedCard)
    {
        RunManager.Instance.AddCardToDeck(selectedCard);
        Debug.Log($"{selectedCard.cardName} 덱에 추가!");

        _selectedCount++;

        if (_selectedCount >= _selectCount)
        {
            Hide();
            _onComplete?.Invoke();
        }
        else
        {
            // 선택한 카드 슬롯 비활성화
            foreach (var obj in _spawnedCards)
            {
                CardView cardView = obj.GetComponent<CardView>();
                if (cardView != null && cardView.GetCardData() == selectedCard)
                {
                    obj.SetActive(false);
                    break;
                }
            }
        }
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
            PoolManager.Instance.Despawn(card, cardPrefab);
        _spawnedCards.Clear();
    }
    
    public void ForceHide()
    {
        if (panel != null) panel.SetActive(false);
        foreach (var card in _spawnedCards)
        {
            if (card != null)
                PoolManager.Instance.Despawn(card, cardPrefab);
        }
        _spawnedCards.Clear();
    }
    
    private void OnDestroy()
    {
        foreach (var card in _spawnedCards)
            PoolManager.Instance.Despawn(card, cardPrefab);
        _spawnedCards.Clear();
    }
}