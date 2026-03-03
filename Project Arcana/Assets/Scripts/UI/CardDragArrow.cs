using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragArrow : MonoBehaviour, IPointerDownHandler
{
    public Arrow arrow;
    public CardView CardView => _cardView; // public 프로퍼티로 노출
    private CardView _cardView;
    private RectTransform _rectTransform;
    private ITargetable _lastHovered;
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

        // 1. 레이캐스트로 타겟 감지
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        ITargetable currentTarget = hit?.GetComponent<ITargetable>();

        // 2. 호버 처리 (색상 변경)
        if (currentTarget != _lastHovered)
        {
            _lastHovered?.OnHoverExit();
            _lastHovered = currentTarget;
            _lastHovered?.OnHoverEnter();
        }

        // 3. 우클릭 취소
        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
            return;
        }

        // 4. 클릭 사용
        if (Input.GetMouseButtonDown(0) && currentTarget != null)
        {
            // 중요: 카드를 사용하기 전에 호버 상태부터 강제로 끕니다.
            _lastHovered?.OnHoverExit();
            _lastHovered = null;

            currentTarget.OnSelect(); // 여기서 BattleView의 UseSelectedCard가 호출됨
        
            // 카드 사용 후 화살표 제거 및 선택 해제
            Deselect(); 
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (_cardView.IsSelected) return;

        // Self 타겟 카드는 화살표 없이 바로 사용
        if (_cardView.GetCardData().targetType == CardTargetType.Self)
        {
            _cardView.Select();
            OnCardSelected?.Invoke(_cardView); // _selectedCardView 세팅
        
            BattleView battleView = FindAnyObjectByType<BattleView>();
            battleView?.UseSelectedCard(null); // null = 자기 자신
            return;
        }

        // Enemy 타겟은 기존대로
        _cardView.Select();
        arrow.SetupAndActivate(_rectTransform);
        OnCardSelected?.Invoke(_cardView);
    }

    public void Deselect()
    {
// 화살표가 남지 않도록 호버 상태 완전 초기화
        if (_lastHovered != null)
        {
            _lastHovered.OnHoverExit();
            _lastHovered = null;
        }

        _cardView.Deselect();
        if (arrow != null) arrow.Deactivate(); // 화살표 비활성화
        OnCardDeselected?.Invoke(_cardView);
    }
}