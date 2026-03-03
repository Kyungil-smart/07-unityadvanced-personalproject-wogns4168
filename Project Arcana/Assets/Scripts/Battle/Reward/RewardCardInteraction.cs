using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RewardCardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private CardData _cardData;
    private Action<CardData> _onSelected;
    private Vector3 _originalScale;

    public void Setup(CardData cardData, Action<CardData> onSelected)
    {
        _cardData = cardData;
        _onSelected = onSelected;
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = _originalScale * 1.1f; // 호버 시 살짝 커짐
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _originalScale; // 원래 크기로
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onSelected?.Invoke(_cardData);
    }
}