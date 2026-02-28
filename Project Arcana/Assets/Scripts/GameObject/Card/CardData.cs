using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public int cost;
    public bool isExhaust;
    public CardType type;
    public float attackValue;
    public float defenseValue;
    public float healValue;
    
    public Sprite cardImage;
}
