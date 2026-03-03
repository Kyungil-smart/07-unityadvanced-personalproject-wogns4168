using UnityEngine;

public class BreakStatus : IStatus
{
    public string Name => "Break";
    public int Stack { get; set; }

    public BreakStatus(int stack)
    {
        Stack = stack;
    }

    public void OnApply(Health target) { }

    public void OnTurnStart(Health target)
    {
        Stack--;
    }

    public void OnTurnEnd(Health target) { }

    public void OnRemove(Health target)
    {
        Debug.Log("Break 제거");
    }

    // 받는 피해 증가 계수 (외부에서 참조)
    public static float DamageMultiplier => 1.5f;
}