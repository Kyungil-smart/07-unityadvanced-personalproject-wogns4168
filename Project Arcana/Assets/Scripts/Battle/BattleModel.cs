using System.Collections.Generic;
using UnityEngine;

public class BattleModel
{
    public List<MonsterBase> Monsters { get; private set; }
    public Deck Deck { get; private set; }
    
    // Deck.hand를 직접 참조
    public List<CardData> CurrentHand => Deck.hand;

    public BattleModel(List<MonsterBase> monsters, Deck deck)
    {
        Monsters = monsters;
        Deck = deck;
    }

    public void DrawCard()
    {
        Deck.Draw(); // Deck.hand에 자동으로 추가됨
    }

    public void UseCard(CardData card)
    {
        Deck.UseCard(card); // Deck 내부에서 hand에서 제거
    }

    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
            DrawCard();
            
        Debug.Log($"손패 수: {CurrentHand.Count}");
    }
}