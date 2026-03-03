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

    public event Action<CardView> OnSelected;
    public event Action<CardView> OnUsed;
    public event Action<CardView> OnHoverEnter;
    public event Action<CardView> OnHoverExit;

    public void Setup(CardData cardData)
    {
        _cardData = cardData;
        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        artworkImage.sprite = cardData.cardImage;

        if (cardData.effects.Count > 0)
        {
            // 수치들을 색상이 입혀진 문자열로 변환
            object[] styledValues = new object[cardData.effects.Count];
            for (int i = 0; i < cardData.effects.Count; i++)
            {
                float val = cardData.effects[i].value;
                CardEffectType type = cardData.effects[i].effectType;

                // 타입에 따라 다른 색상 적용
                string colorHex = type switch
                {
                    CardEffectType.DealDamage => "#FF5B5B", // 연빨강 (공격)
                    CardEffectType.GainBlock => "#5B5BFF",  // 연파랑 (방어)
                    CardEffectType.ApplyVulnerable => "#FFD700", // 골드 (취약)
                    CardEffectType.DrawCard => "#50C878",   // 에메랄드 (드로우)
                    _ => "#FFFFFF" // 기본 흰색
                };

                // 수치에 색상 태그 입히기 (예: <color=#FF5B5B>8</color>)
                styledValues[i] = $"<color={colorHex}>{val}</color>";
            }

            try {
                descriptionText.text = string.Format(cardData.description, styledValues);
            } catch {
                descriptionText.text = cardData.description;
            }
        }
        else {
            descriptionText.text = cardData.description;
        }
    }

    public CardData GetCardData() => _cardData;

    public void OnSpawn() => gameObject.SetActive(true);
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
        OnUsed?.Invoke(this);
    }

    public void SetDragging(bool value) => IsDragging = value;

    public void Select()
    {
        if (IsSelected) return;
        IsSelected = true;
        OnSelected?.Invoke(this);
    }

    public void Deselect() => IsSelected = false;

    public void SetHover(bool value) => IsHover = value;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsDragging) return;
        SetHover(true);
        OnHoverEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHover(false);
        OnHoverExit?.Invoke(this);
    }
}