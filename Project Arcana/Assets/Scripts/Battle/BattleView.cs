using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleView : MonoBehaviour
{
    [Header("UI References")]
    public Transform handArea;
    public Arrow arrow;
    public GameObject cardPrefab;

    private List<CardView> _handViews = new List<CardView>();
    private CardDragArrow _selectedDragArrow;
    private CardView _selectedCardView;

    public event Action<CardView> OnCardSelected;
    public event Action<CardView, ITargetable> OnCardUsed;

    public void ClearHand()
    {
        foreach (var view in _handViews)
            Destroy(view.gameObject);

        _handViews.Clear();
        _selectedDragArrow = null;
        _selectedCardView = null;
    }

    public void SpawnHand(List<CardData> hand)
    {
        ClearHand();

        foreach (var card in hand)
        {
            GameObject obj = Instantiate(cardPrefab, handArea, false);
            CardView view = obj.GetComponent<CardView>();
            view.Setup(card);

            var dragArrow = obj.GetComponent<CardDragArrow>();
            dragArrow.arrow = arrow;
            dragArrow.OnCardSelected += HandleCardSelected;
            dragArrow.OnCardDeselected += HandleCardDeselected;

            view.OnHoverEnter += _ => RefreshHandLayout();
            view.OnHoverExit += _ => RefreshHandLayout();

            _handViews.Add(view);
        }

        RefreshHandLayout();
    }

    public void RefreshHandLayout()
    {
        int count = _handViews.Count;
        if (count == 0) return;

        float middleIndex = (count - 1) / 2f;
        float spacing = 2f;
        float angleStep = 5f;
        float yStep = 0.5f;

        for (int i = 0; i < count; i++)
        {
            CardView card = _handViews[i];
            RectTransform rect = card.GetComponent<RectTransform>();
            float offset = i - middleIndex;

            Vector2 pos = new Vector2(offset * spacing, -Mathf.Abs(offset) * yStep);
            float angle = -offset * angleStep;
            Vector3 scale = Vector3.one * 0.8f;

            if (card.IsSelected || card.IsHover)
            {
                pos += Vector2.up * 3f;
                scale = Vector3.one;
                rect.SetAsLastSibling();
            }

            rect.anchoredPosition = pos;
            rect.localRotation = Quaternion.Euler(0, 0, angle);
            rect.localScale = scale;
        }
    }

    public void UseSelectedCard(ITargetable target)
    {
        if (_selectedCardView == null) return;

        OnCardUsed?.Invoke(_selectedCardView, target); // target 같이 전달

        _selectedDragArrow?.Deselect();
        _selectedDragArrow = null;
        _selectedCardView = null;

        RefreshHandLayout();
    }

    private void HandleCardSelected(CardView view)
    {
        // 기존 선택 카드 Deselect (arrow는 바로 새 카드로 넘길 거라 끄지 않음)
        if (_selectedDragArrow != null)
        {
            _selectedDragArrow.CardView.Deselect();
            _selectedDragArrow = null;
        }

        _selectedCardView = view;
        _selectedDragArrow = view.GetComponent<CardDragArrow>();

        // 화살표 새 카드 기준으로 즉시 갱신
        arrow.SetupAndActivate(view.GetComponent<RectTransform>());

        OnCardSelected?.Invoke(view);
        RefreshHandLayout();
    }

    private void HandleCardDeselected(CardView view)
    {
        if (_selectedCardView == view)
        {
            _selectedCardView = null;
            _selectedDragArrow = null;
        }

        RefreshHandLayout();
    }
}