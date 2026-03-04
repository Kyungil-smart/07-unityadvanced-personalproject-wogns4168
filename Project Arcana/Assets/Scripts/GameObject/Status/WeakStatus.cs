using UnityEngine;

public class WeakStatus : IStatus
{
    public string Name => "Weak";
    public int Stack { get; set; }

    public WeakStatus(int stack)
    {
        Stack = stack;
    }

    public void OnApply(Health target) { }

    public void OnTurnStart(Health target)
    {
        // 턴 시작마다 스택 1 감소
        Stack--;
    }

    public void OnTurnEnd(Health target) { }

    public void OnRemove(Health target)
    {
        Debug.Log("Weak 제거");
    }

    // 공격력 감소 계수 (외부에서 참조)
    public static float DamageMultiplier => 0.5f;
}