using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject goldGroupPrefab;

    [Header("버튼")]
    [SerializeField] private Button healButton;
    [SerializeField] private TMP_Text healCostText;
    [SerializeField] private Button removeCardButton;
    [SerializeField] private TMP_Text removeCostText;
    [SerializeField] private Button leaveButton;

    [Header("카드 삭제 패널")]
    [SerializeField] private GameObject removeCardPanel;
    [SerializeField] private Transform removeCardContainer;
    [SerializeField] private Button closeButton;

    [Header("가격 설정")]
    [SerializeField] private int healCost = 50;
    [SerializeField] private int removeCardCost = 75;
    [SerializeField] private int cardMinCost = 50;
    [SerializeField] private int cardMaxCost = 100;

    [Header("골드 표시 위치 오프셋")]
    [SerializeField] private Vector2 goldGroupOffset = new Vector2(50f, -80f);

    private List<GameObject> _spawnedCards = new List<GameObject>();
    private List<GameObject> _spawnedGoldGroups = new List<GameObject>();
    private List<GameObject> _removeCards = new List<GameObject>();

    private void Start()
    {
        removeCardPanel.SetActive(false);

        healCostText.text = $"{healCost}";
        removeCostText.text = $"{removeCardCost}";

        healButton.onClick.AddListener(OnHealClicked);
        removeCardButton.onClick.AddListener(OnRemoveCardClicked);
        leaveButton.onClick.AddListener(() => MapManager.Instance.OnNodeCleared());
        closeButton.onClick.AddListener(() => removeCardPanel.SetActive(false));

        SpawnShopCards();
    }

    private void SpawnShopCards()
    {
        foreach (var card in _spawnedCards) Destroy(card);
        foreach (var gold in _spawnedGoldGroups) Destroy(gold);
        _spawnedCards.Clear();
        _spawnedGoldGroups.Clear();

        List<CardData> shopCards = RunManager.Instance.GetRandomRewardCards(5);

        for (int i = 0; i < shopCards.Count && i < cardSlots.Length; i++)
        {
            int price = Random.Range(cardMinCost, cardMaxCost + 1);

            GameObject obj = Instantiate(cardPrefab, cardSlots[i]);
            RectTransform cardRect = obj.GetComponent<RectTransform>();
            cardRect.localScale = Vector3.one * 70f;
            cardRect.anchoredPosition = Vector2.zero;
            obj.GetComponent<CardView>().Setup(shopCards[i]);

            CardDragArrow dragArrow = obj.GetComponent<CardDragArrow>();
            if (dragArrow != null) dragArrow.enabled = false;

            ShopCardInteraction interaction = obj.AddComponent<ShopCardInteraction>();
            interaction.Setup(shopCards[i], price, OnCardBought);

            _spawnedCards.Add(obj);

            if (goldGroupPrefab != null)
            {
                GameObject goldGroup = Instantiate(goldGroupPrefab, cardSlots[i]);
                RectTransform goldRect = goldGroup.GetComponent<RectTransform>();
                goldRect.anchoredPosition = goldGroupOffset;
                goldRect.GetComponentInChildren<TMP_Text>().text = price.ToString();
                _spawnedGoldGroups.Add(goldGroup);
                interaction.SetGoldGroup(goldGroup);
            }
        }
    }

    private void SpawnRemoveCardList()
    {
        foreach (var card in _removeCards) Destroy(card);
        _removeCards.Clear();
        
        var deck = RunManager.Instance.currentDeck;

        List<CardData> allCards = new List<CardData>(RunManager.Instance.currentDeck.drawPile);
        allCards.AddRange(RunManager.Instance.currentDeck.discardPile);
        allCards.AddRange(RunManager.Instance.currentDeck.exhaustPile);
        allCards.AddRange(deck.hand);

        foreach (var cardData in allCards)
        {
            GameObject obj = Instantiate(cardPrefab, removeCardContainer);
            RectTransform cardRect = obj.GetComponent<RectTransform>();
            cardRect.localScale = Vector3.one * 70f;
            obj.GetComponent<CardView>().Setup(cardData);

            CardDragArrow dragArrow = obj.GetComponent<CardDragArrow>();
            if (dragArrow != null) dragArrow.enabled = false;

            RewardCardInteraction interaction = obj.AddComponent<RewardCardInteraction>();
            interaction.Setup(cardData, (selected) =>
            {
                RunManager.Instance.AddGold(-removeCardCost);
                RunManager.Instance.RemoveCardFromDeck(selected);
                removeCardPanel.SetActive(false);
            });

            _removeCards.Add(obj);
        }
    }

    private void OnCardBought(CardData card, int price) { }

    private void OnHealClicked()
    {
        if (RunManager.Instance.Gold < healCost)
        {
            Debug.Log("골드 부족!");
            return;
        }
        Player player = FindAnyObjectByType<Player>();
        if (player == null) return;

        RunManager.Instance.AddGold(-healCost);
        player.Heal(player.maxHealth * 0.3f);
        RunManager.Instance.SavePlayerHp(player.currentHealth, player.maxHealth);
    }

    private void OnRemoveCardClicked()
    {
        if (RunManager.Instance.Gold < removeCardCost)
        {
            Debug.Log("골드 부족!");
            return;
        }
        removeCardPanel.SetActive(true);
        SpawnRemoveCardList();
    }
}