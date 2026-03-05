using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class CardTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TMP_Text tooltipText;

    private void Awake()
    {
        if (tooltip != null) tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardView cardView = GetComponent<CardView>();
        if (cardView == null) return;

        CardData data = cardView.GetCardData();
        if (data == null) return;

        string text = BuildTooltipText(data);
        if (string.IsNullOrEmpty(text)) return;

        tooltipText.text = text;
        tooltip.SetActive(true);
        tooltip.transform.SetAsLastSibling(); // 부모 이동 없이 순서만 변경
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null) tooltip.SetActive(false);
    }

    private string BuildTooltipText(CardData data)
    {
        string text = "";

        // 상태이상 설명
        List<string> addedStatus = new List<string>();
        foreach (var effect in data.effects)
        {
            string statusDesc = GetStatusDescription(effect.effectType);
            if (!string.IsNullOrEmpty(statusDesc) && !addedStatus.Contains(statusDesc))
            {
                text += $"{statusDesc}\n";
                addedStatus.Add(statusDesc);
            }
        }

        // 소멸
        if (data.isExhaust)
            text += "소멸: 사용 후 해당 전투에서 사용 불가능";

        return text.TrimEnd();
    }

    private string GetStatusDescription(CardEffectType effectType)
    {
        return effectType switch
        {
            CardEffectType.ApplyPoison => "독: 턴 종료 시 스택만큼 데미지를 입고 1씩 감소",
            CardEffectType.ApplyWeak => "약화: 공격력 25% 감소",
            CardEffectType.ApplyBreak => "취약: 방어력 25% 감소",
            _ => ""
        };
    }
    
    public void ForceHide()
    {
        if (tooltip != null) tooltip.SetActive(false);
    }
}