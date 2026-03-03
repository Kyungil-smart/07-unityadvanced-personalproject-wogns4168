using UnityEngine;

public class CardEffectProcessor
{
    private BattleContext _context;

    public CardEffectProcessor(BattleContext context)
    {
        _context = context;
    }

    public void Process(CardData card, ITargetable target)
    {
        switch (card.effectType)
        {
            case CardEffectType.DealDamage:
                DealDamage(card, target);
                break;
            case CardEffectType.GainBlock:
                GainBlock(card);
                break;
            case CardEffectType.Heal:
                HealPlayer(card);
                break;
            case CardEffectType.ApplyPoison:
                ApplyPoison(card, target);
                break;
            case CardEffectType.ApplyWeak:
                ApplyWeak(card, target);
                break;
            case CardEffectType.ApplyBreak:
                ApplyBreak(card, target);
                break;
            case CardEffectType.DrawCard:
                DrawCard(card);
                break;
            default:
                Debug.LogWarning($"처리되지 않은 effectType: {card.effectType}");
                break;
        }
    }

    private void DealDamage(CardData card, ITargetable target)
    {
        if (target is Health health)
            health.TakeDamage(card.attackValue);
        else
            Debug.LogWarning("DealDamage: target이 Health가 아님");
    }

    private void GainBlock(CardData card)
    {
        _context.Player.AddShield(card.defenseValue);
    }

    private void HealPlayer(CardData card)
    {
        _context.Player.Heal(card.healValue);
    }

    private void ApplyPoison(CardData card, ITargetable target)
    {
        // 상태이상 시스템 구현 후 연결
        Debug.Log($"Poison {card.statusValue} 적용 → {target}");
    }

    private void ApplyWeak(CardData card, ITargetable target)
    {
        Debug.Log($"Weak {card.statusValue} 적용 → {target}");
    }

    private void ApplyBreak(CardData card, ITargetable target)
    {
        Debug.Log($"Break {card.statusValue} 적용 → {target}");
    }

    private void DrawCard(CardData card)
    {
        _context.Model.DrawCards(card.drawValue);
    }
}