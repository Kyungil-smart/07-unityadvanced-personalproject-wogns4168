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
                if (target is Health health)
                {
                    float damage = effect.value;

                    // Weak 상태이상이면 플레이어 공격력 감소
                    int weakStack = _context.Player.StatusManager.GetStack("Weak");
                    if (weakStack > 0)
                        damage *= WeakStatus.DamageMultiplier;

                    health.TakeDamage(damage);
                }
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
            case CardEffectType.ApplyPoison:
                if (target is Health poisonTarget)
                    poisonTarget.StatusManager.Apply(new PoisonStatus((int)effect.value));
                break;
            case CardEffectType.ApplyWeak:
                if (target is Health weakTarget)
                    weakTarget.StatusManager.Apply(new WeakStatus((int)effect.value));
                break;
            case CardEffectType.ApplyBreak:
                if (target is Health breakTarget)
                    breakTarget.StatusManager.Apply(new BreakStatus((int)effect.value));
                break;
        }
    }
    
}