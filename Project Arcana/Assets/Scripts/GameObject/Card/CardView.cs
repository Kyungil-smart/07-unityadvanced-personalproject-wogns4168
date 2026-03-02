using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPoolable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text exhaustText;
    [SerializeField] private Image artworkImage;
    
    public bool IsHover { get; private set; }
    public bool IsSelected { get; private set; }
    public bool IsDragging { get; private set; }
    
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
        exhaustText.gameObject.SetActive(cardData.isExhaust);
        artworkImage.sprite = cardData.cardImage;
    }

    public CardData GetCardData()
    {
        return _cardData;
    }
    
    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        _cardData = null;
        IsHover = false;
        IsSelected = false;
        IsDragging = false;
        gameObject.SetActive(false);
    }

    public void UseCard()
    {
        if (_cardData == null) return;

        BattleManager.Instance.OnCardUsed(this);
    }

    public void SetDragging(bool value)
    {
        IsDragging = value;
    }
    
    public void Select()
    {
        IsSelected = true;
        BattleManager.Instance.RefreshHandLayout();
    }

    public void Deselect()
    {
        IsSelected = false;
        BattleManager.Instance.RefreshHandLayout();
    }
    
    public void SetHover(bool value)
    {
        IsHover = value;
        BattleManager.Instance.RefreshHandLayout();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsDragging) return;   // 드래그 중이면 무시
        SetHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHover(false);
    }
}
