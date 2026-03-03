using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public int cost;
    public bool isExhaust;
    public CardEffectType effectType;
    public CardTargetType targetType;
    public float attackValue;
    public float defenseValue;
    public float healValue;
    public int drawValue;      // DrawCard용
    public int statusValue;    // 상태이상 스택 수
    
    public Sprite cardImage;
}
