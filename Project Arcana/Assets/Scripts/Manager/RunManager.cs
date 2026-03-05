using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    public int MaxEnergy { get; private set; }
    
    public List<CardData> startingDeck = new List<CardData>();
    public List<CardData> allCards; // 보상으로 줄 수 있는 카드 풀 (Inspector에서 설정)
    public int baseMaxEnergy = 3;

    public Deck currentDeck { get; set; }
    public int Gold { get; private set; }
    public MapNode CurrentMapNode { get; private set; }
    public float CurrentHp { get; private set; }
    public float MaxHp { get; private set; }
    public string PlayerName { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartNewRun();
    }

    public void StartNewRun()
    {
        currentDeck = new Deck();
        currentDeck.Init(startingDeck);
        Gold = 100;
        CurrentHp = 100; // 초기화 (0이면 BattleInitializer에서 maxHealth 사용)
        MaxHp = 100;
        MaxEnergy = baseMaxEnergy;
    }

    public void AddGold(int amount)
    {
        Gold = Mathf.Max(0, Gold + amount);
        Debug.Log($"골드 {amount}, 현재 골드: {Gold}");
    }

    public void AddCardToDeck(CardData card)
    {
        currentDeck.AddCard(card);
        Debug.Log($"{card.cardName} 덱에 추가, 현재 덱: {currentDeck.drawPile.Count + currentDeck.discardPile.Count}장");
    }

    // 보상용 랜덤 카드 3장 뽑기
    public List<CardData> GetRandomRewardCards(int count = 3)
    {
        if (allCards == null || allCards.Count == 0)
        {
            Debug.LogWarning("allCards가 비어있음!");
            return new List<CardData>();
        }

        List<CardData> pool = new List<CardData>(allCards);
        List<CardData> result = new List<CardData>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int rand = Random.Range(0, pool.Count);
            result.Add(pool[rand]);
            pool.RemoveAt(rand); // 중복 방지
        }

        return result;
    }
    
    public void SetCurrentNode(MapNode node)
    {
        CurrentMapNode = node;
    }
    
    public void SavePlayerHp(float current, float max)
    {
        CurrentHp = current;
        MaxHp = max;
    }
    
    public void RemoveCardFromDeck(CardData card)
    {
        currentDeck.RemoveCard(card);
        Debug.Log($"{card.cardName} 덱에서 제거");
    }
    
    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }
    
    public void SetGold(int amount)
    {
        Gold = amount;
    }
    
    public void IncreaseMaxEnergy(int amount = 1)
    {
        MaxEnergy += amount;
    }
}