using UnityEngine;
using UnityEngine.EventSystems;  // 마우스 이벤트 처리용

// 카드 드래그 시 화살표로 타겟 지정, 선택까지 처리하는 클래스
public class CardDragArrow : MonoBehaviour,
    IPointerDownHandler,   // 마우스 클릭 시
    IPointerUpHandler,     // 마우스 버튼 떼기
    IDragHandler           // 마우스 드래그 시
{
    // 실제 화살표 UI를 연결
    public Arrow arrow;

    // 현재 마우스가 가리키는 타겟 (Player, Monster 등)
    private ITargetable currentTarget;

    // 마우스 클릭 시작 (카드 선택)
    public void OnPointerDown(PointerEventData eventData)
    {
        // 카드 위치를 origin으로 화살표 활성화
        arrow.SetupAndActivate(transform);
    }

    // 마우스 이동 시 호출
    public void OnDrag(PointerEventData eventData)
    {
        CheckTarget(); // 타겟 갱신
    }

    // 마우스 버튼 떼기
    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentTarget != null)
        {
            // 타겟이 있으면 즉시 효과 적용
            currentTarget.OnSelect();  // 카드 공격/효과 적용
        }

        // 화살표 UI 비활성화
        arrow.Deactivate();

        // 타겟 초기화
        ClearTarget();
    }

    // 마우스 위치에 있는 타겟 체크
    void CheckTarget()
    {
        // 마우스 월드 좌표
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 마우스가 가리키는 콜라이더 확인
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if (hit != null)
        {
            // 콜라이더가 ITargetable인지 확인
            ITargetable target = hit.GetComponent<ITargetable>();

            // 새로운 타겟이면 갱신
            if (target != currentTarget)
            {
                ClearTarget(); // 이전 타겟 초기화
                currentTarget = target;
                currentTarget?.OnHoverEnter(); // 하이라이트 효과
            }
        }
        else
        {
            // 아무것도 없으면 타겟 초기화
            ClearTarget();
        }
    }

    // 현재 타겟 초기화 및 하이라이트 제거
    void ClearTarget()
    {
        currentTarget?.OnHoverExit();
        currentTarget = null;
    }
}