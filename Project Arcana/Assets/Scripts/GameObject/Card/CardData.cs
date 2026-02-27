using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int cost;
    public bool isExhaust;
    public string type;
    public float attackValue;
    public float defenseValue;
    public float healValue;
}
