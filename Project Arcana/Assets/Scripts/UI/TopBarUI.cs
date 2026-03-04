using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopBarUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text floorText;
    [SerializeField] private Button deckButton;
    [SerializeField] private TMP_Text playerNameText;

    [Header("덱 보기 패널")]
    [SerializeField] private GameObject deckPanel;
    [SerializeField] private Transform deckCardContainer;
    [SerializeField] private GameObject cardPrefab;

    private List<GameObject> _deckCards = new List<GameObject>();

    private void Start()
    {
        Refresh();
        deckButton.onClick.AddListener(OnDeckButtonClicked);
        if (deckPanel != null) deckPanel.SetActive(false);
    }

    public void Refresh()
    {
        // 골드
        if (goldText != null)
            goldText.text = RunManager.Instance.Gold.ToString();

        // HP
        if (hpText != null)
        {
            Player player = FindAnyObjectByType<Player>();
            if (player != null)
                hpText.text = $"{(int)player.currentHealth}/{(int)player.maxHealth}";
        }

        // 층수
        if (floorText != null)
        {
            MapNode node = RunManager.Instance.CurrentMapNode;
            int floor = node != null ? node.Floor + 1 : 1;
            floorText.text = $"{floor}층";
        }
        
        if (playerNameText != null)
            playerNameText.text = RunManager.Instance.PlayerName;
    }

    private void OnDeckButtonClicked()
    {
        if (deckPanel == null) return;

        bool isOpen = !deckPanel.activeSelf;
        deckPanel.SetActive(isOpen);

        if (isOpen) SpawnDeckCards();
        else ClearDeckCards();
    }

    private void SpawnDeckCards()
    {
        ClearDeckCards();

        var deck = RunManager.Instance.currentDeck;
        List<CardData> allCards = new List<CardData>(deck.drawPile);
        allCards.AddRange(deck.discardPile);
        allCards.AddRange(deck.exhaustPile);
        allCards.AddRange(deck.hand);

        foreach (var cardData in allCards)
        {
            GameObject obj = Instantiate(cardPrefab, deckCardContainer);
            obj.GetComponent<RectTransform>().localScale = Vector3.one * 70f;
            obj.GetComponent<CardView>().Setup(cardData);

            CardDragArrow drag = obj.GetComponent<CardDragArrow>();
            if (drag != null) drag.enabled = false;

            _deckCards.Add(obj);
        }
    }

    private void ClearDeckCards()
    {
        foreach (var card in _deckCards) Destroy(card);
        _deckCards.Clear();
    }
}