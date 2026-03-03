using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragArrow : MonoBehaviour, IPointerDownHandler
{
    public Arrow arrow;
    private CardView _cardView;
    public event Action<CardView> OnCardSelected;
    public event Action<CardView> OnCardDeselected;

    private void Awake()
    {
        _cardView = GetComponent<CardView>();
        if (arrow == null)
            arrow = FindAnyObjectByType<Arrow>();
    }

    private void Update()
    {
        if (!_cardView.IsSelected) return;

        // 우클릭 → 선택 해제
        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
            return;
        }

        // 좌클릭 → 타겟 감지
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null)
            {
                var target = hit.GetComponent<ITargetable>();
                if (target != null)
                {
                    target.OnSelect(); // BattleView.UseSelectedCard 호출
                    Deselect();        // 화살표 제거
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (_cardView.IsSelected) return;

        _cardView.Select();
        arrow.SetupAndActivate(transform);
        OnCardSelected?.Invoke(_cardView);
    }

    public void Deselect()
    {
        _cardView.Deselect();
        arrow.Deactivate();
        OnCardDeselected?.Invoke(_cardView);
    }
}