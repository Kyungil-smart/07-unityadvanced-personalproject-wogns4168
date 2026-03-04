using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ShopCardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private GameObject _soldOutOverlay;
    private CardData _cardData;
    private int _price;
    private Action<CardData, int> _onSelected;
    private Vector3 _originalScale;
    private bool _isSold;
    private GameObject _goldGroup;

    public void Setup(CardData cardData, int price, Action<CardData, int> onSelected)
    {
        _cardData = cardData;
        _price = price;
        _onSelected = onSelected;
        _originalScale = transform.localScale;

        _soldOutOverlay = transform.Find("SoldOutOverlay")?.gameObject;
        if (_soldOutOverlay != null) _soldOutOverlay.SetActive(false);
    }

    public void SetGoldGroup(GameObject goldGroup)
    {
        _goldGroup = goldGroup;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSold) return;
        transform.localScale = _originalScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSold) return;
        transform.localScale = _originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isSold) return;

        if (RunManager.Instance.Gold < _price)
        {
            Debug.Log("골드 부족!");
            return;
        }

        RunManager.Instance.AddGold(-_price);
        RunManager.Instance.AddCardToDeck(_cardData);
        _isSold = true;

        if (_soldOutOverlay != null) _soldOutOverlay.SetActive(true);
        if (_goldGroup != null) _goldGroup.SetActive(false);
        transform.localScale = _originalScale;
        _onSelected?.Invoke(_cardData, _price);
    }
}