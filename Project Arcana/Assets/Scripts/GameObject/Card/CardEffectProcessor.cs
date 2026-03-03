using UnityEngine;

public class CardEffectProcessor
{
    private BattleContext _context;

    public CardEffectProcessor(BattleContext context)
    {
        _context = context;
    }
    
    public bool Process(CardData card, ITargetable target)
    {
        // 에너지 부족하면 카드 사용 불가
        if (!_context.Model.UseEnergy(card.cost))
        {
            Debug.Log($"에너지 부족! 필요: {card.cost}, 현재: {_context.Model.CurrentEnergy}");
            return false;
        }

        foreach (var effect in card.effects)
        {
            ExecuteEffect(effect, target);
        }

        return true;
    }
    
    private void ExecuteEffect(CardEffect effect, ITargetable target)
    {
        switch (effect.effectType)
        {
            case CardEffectType.DealDamage:
                if (target is Health health) health.TakeDamage(effect.value);
                break;
            case CardEffectType.GainBlock:
                _context.Player.AddShield(effect.value);
                break;
            case CardEffectType.Heal:
                _context.Player.Heal(effect.value);
                break;
            case CardEffectType.DrawCard:
                _context.Model.DrawCards((int)effect.value);
                break;
            case CardEffectType.ApplyVulnerable:
            case CardEffectType.ApplyPoison:
            case CardEffectType.ApplyWeak:
            case CardEffectType.ApplyBreak:
                // TODO: 상태이상 인터페이스(IStatus) 구현 후 연결
                Debug.Log($"{effect.effectType} {effect.value} 적용 시도");
                break;
        }
    }
    
}