using UnityEngine;

public class CardEffectProcessor
{
    private BattleContext _context;

    public CardEffectProcessor(BattleContext context)
    {
        _context = context;
    }

    // 에너지 체크 후 처리
    // 성공하면 true, 에너지 부족하면 false
    public bool Process(CardData card, ITargetable target)
    {
        // 에너지 부족하면 카드 사용 불가
        if (!_context.Model.UseEnergy(card.cost))
        {
            Debug.Log($"에너지 부족! 필요: {card.cost}, 현재: {_context.Model.CurrentEnergy}");
            return false;
        }

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
                Debug.Log($"Poison {card.statusValue} 적용");
                break;
            case CardEffectType.ApplyWeak:
                Debug.Log($"Weak {card.statusValue} 적용");
                break;
            case CardEffectType.ApplyBreak:
                Debug.Log($"Break {card.statusValue} 적용");
                break;
            case CardEffectType.DrawCard:
                DrawCard(card);
                break;
            default:
                Debug.LogWarning($"처리되지 않은 effectType: {card.effectType}");
                break;
        }

        return true;
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

    private void DrawCard(CardData card)
    {
        _context.Model.DrawCards(card.drawValue);
    }
}