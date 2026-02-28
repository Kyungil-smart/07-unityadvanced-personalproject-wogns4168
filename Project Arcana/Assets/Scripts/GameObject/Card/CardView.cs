using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour, IPoolable
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text exhaustText;
    [SerializeField] private Image artworkImage;
    
    
    private CardData _cardData;

    public void Setup(CardData cardData)
    {
        _cardData = cardData;
        
        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        switch (cardData.type)
        {
            case CardType.Attack:
                descriptionText.text =
                    string.Format(cardData.description, cardData.attackValue);
                break;

            case CardType.Defense:
                descriptionText.text =
                    string.Format(cardData.description, cardData.defenseValue);
                break;

            case CardType.Heal:
                descriptionText.text =
                    string.Format(cardData.description, cardData.healValue);
                break;
        }
        if (cardData.isExhaust) exhaustText.gameObject.SetActive(true);
        artworkImage.sprite = cardData.cardImage;
    }
    
    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        _cardData = null;
        gameObject.SetActive(false);
    }

    public void UseCard()
    {
        if (_cardData == null) return;
        
        PoolManager.Instance.Despawn(gameObject, this.gameObject);
    }
}
