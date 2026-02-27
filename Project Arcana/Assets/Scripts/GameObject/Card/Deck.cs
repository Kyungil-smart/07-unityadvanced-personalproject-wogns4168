using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardData> deckPool = new List<CardData>(); // 전체 카드
    public List<CardData> drawPile = new List<CardData>();
    public List<CardData> hand = new List<CardData>();
    public List<CardData> discardPile = new List<CardData>();
    public List<CardData> exhaustPile = new List<CardData>();

    [SerializeField] private GameObject cardPrefab;

    void Start()
    {
        drawPile = new List<CardData>(deckPool);
        Shuffle(drawPile);
    }

    // 랜덤 카드 뽑기
    public CardView DrawRandomCard()
    {
        if (drawPile.Count == 0) ReshuffleDiscard();
        if (drawPile.Count == 0) return null;

        int index = Random.Range(0, drawPile.Count);
        CardData data = drawPile[index];
        drawPile.RemoveAt(index);
        hand.Add(data);

        GameObject cardGO = PoolManager.Instance.Spawn(cardPrefab);
        CardView cardView = cardGO.GetComponent<CardView>();
        cardView.Setup(data);
        return cardView;
    }

    // 카드 사용
    public void UseCard(CardData card)
    {
        if (!hand.Contains(card)) return;
        hand.Remove(card);

        if (card.isExhaust) exhaustPile.Add(card);
        else discardPile.Add(card);
    }

    private void ReshuffleDiscard()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Shuffle(drawPile);
    }

    private void Shuffle<T>(List<T> list)
    {
        list = list.OrderBy(x => Random.value).ToList();
    }
}