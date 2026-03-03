using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragArrow : MonoBehaviour, IPointerDownHandler
{
    public Arrow arrow;
    public CardView CardView => _cardView; // public 프로퍼티로 노출
    private CardView _cardView;
    private RectTransform _rectTransform;
    public event Action<CardView> OnCardSelected;
    public event Action<CardView> OnCardDeselected;

    private void Awake()
    {
        _cardView = GetComponent<CardView>();
        _rectTransform = GetComponent<RectTransform>();
        if (arrow == null)
            arrow = FindAnyObjectByType<Arrow>();
    }

    private void Update()
    {
        if (!_cardView.IsSelected) return;

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null)
            {
                var target = hit.GetComponent<ITargetable>();
                if (target != null)
                {
                    target.OnSelect();
                    Deselect();
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (_cardView.IsSelected) return;

        _cardView.Select();
        arrow.SetupAndActivate(_rectTransform);
        OnCardSelected?.Invoke(_cardView);
    }

    public void Deselect()
    {
        _cardView.Deselect();
        arrow.Deactivate();
        OnCardDeselected?.Invoke(_cardView);
    }
}