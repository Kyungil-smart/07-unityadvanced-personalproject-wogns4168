using System.Collections;
using UnityEngine;

public abstract class MonsterBase : Health, ITargetable
{
    public abstract string Name { get; }
    public abstract IEnumerator Act();
    public abstract void Reward();
    public abstract int IntentDamage { get; }

    private BattleView _battleView;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    protected virtual void Start()
    {
        _battleView = FindAnyObjectByType<BattleView>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
            _originalColor = _spriteRenderer.color;
    }

    public virtual void OnSelect()
    {
        if (_battleView == null) return;
        _battleView.UseSelectedCard(this);
    }

    public virtual void OnHoverEnter()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.color = Color.crimson; // 호버 하이라이트
    }

    public virtual void OnHoverExit()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.color = _originalColor;
    }

    public override void Die()
    {
        base.Die();
        Reward();
    }
    
    protected void Attack(Player player, float damage)
    {
        int weakStack = StatusManager.GetStack("Weak");
        if (weakStack > 0)
            damage *= WeakStatus.DamageMultiplier;

        player.TakeDamage(damage);
    }
    
    public virtual void SetStats(float hp, float attack)
    {
        maxHealth = hp;
        currentHealth = hp;
    }
}