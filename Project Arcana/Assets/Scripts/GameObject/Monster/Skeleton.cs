using System.Collections;
using UnityEngine;

public class Skeleton : MonsterBase
{
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private Animator animator;

    public override string Name => "Skeleton";
    
    public override int IntentDamage => (int)attackDamage;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override IEnumerator Act()
    {
        Debug.Log("Skeleton 공격!");

        // 공격 애니메이션
        if (animator != null)
            animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        // 플레이어 찾아서 데미지
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
            player.TakeDamage(attackDamage);

        yield return new WaitForSeconds(0.5f);
    }

    public override void Reward()
    {
        Debug.Log("Skeleton 처치! 보상 지급");
        // 추후 골드/카드 보상 연결
    }
}