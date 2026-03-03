using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    [TextArea] public string description;
    public int cost;
    public bool isExhaust;
    public CardTargetType targetType;
    public Sprite cardImage;

    // 여러 효과를 담는 리스트
    public List<CardEffect> effects = new List<CardEffect>();
}
