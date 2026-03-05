// SkeletonElite.cs
using System.Collections;
using UnityEngine;

public class SkeletonElite : MonsterBase
{
    private float attackDamage;
    [SerializeField] private Animator animator;

    public override string Name => "Skeleton Elite";
    public override int IntentDamage => (int)attackDamage;

    public override void SetStats(float hp, float attack)
    {
        base.SetStats(hp, attack);
        attackDamage = attack;
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override IEnumerator Act()
    {
        if (animator != null) animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            float damage = attackDamage;
            int weakStack = StatusManager.GetStack("Weak");
            if (weakStack > 0) damage *= WeakStatus.DamageMultiplier;
            player.TakeDamage(damage);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public override void Reward() { }
}