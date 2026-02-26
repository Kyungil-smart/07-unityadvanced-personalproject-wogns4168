using UnityEngine;
using UnityEngine.EventSystems;
using Kalkatos.DottedArrow;

public class CardDragArrow : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    public Arrow arrow;

    private ITargetable currentTarget;

    public void OnPointerDown(PointerEventData eventData)
    {
        arrow.SetupAndActivate(transform); // 카드 위치 origin
    }

    public void OnDrag(PointerEventData eventData)
    {
        CheckTarget();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentTarget != null)
        {
            // 타겟이 있으면 즉시 효과 적용
            currentTarget.OnSelect(); // 카드 공격/효과 적용
        }

        // 화살표는 놓자마자 사라짐
        arrow.Deactivate();
        ClearTarget();
    }

    void CheckTarget()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if (hit != null)
        {
            ITargetable target = hit.GetComponent<ITargetable>();

            if (target != currentTarget)
            {
                ClearTarget();
                currentTarget = target;
                currentTarget?.OnHoverEnter();
            }
        }
        else
        {
            ClearTarget();
        }
    }

    void ClearTarget()
    {
        currentTarget?.OnHoverExit();
        currentTarget = null;
    }
}