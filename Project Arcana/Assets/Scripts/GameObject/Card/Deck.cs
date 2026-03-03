using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<CardData> drawPile = new();
    public List<CardData> hand = new();
    public List<CardData> discardPile = new();
    public List<CardData> exhaustPile = new();

    public void Init(List<CardData> deckPool)
    {
        drawPile = new List<CardData>(deckPool);
        Shuffle(drawPile);
    }

    public CardData Draw()
    {
        if (drawPile.Count == 0)
            ReshuffleDiscard();

        if (drawPile.Count == 0)
            return null;

        CardData card = drawPile[0];
        drawPile.RemoveAt(0);
        hand.Add(card);

        return card;
    }

    public void UseCard(CardData card)
    {
        if (!hand.Contains(card)) return;

        hand.Remove(card);

        if (card.isExhaust)
            exhaustPile.Add(card);
        else
            discardPile.Add(card);
    }

    private void ReshuffleDiscard()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Shuffle(drawPile);
    }

    private void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
    
    public void AddCard(CardData card)
    {
        drawPile.Add(card);
    }
}