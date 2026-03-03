using UnityEngine;

public class PoisonStatus : IStatus
{
    public string Name => "Poison";
    public int Stack { get; set; }

    public PoisonStatus(int stack)
    {
        Stack = stack;
    }

    public void OnApply(Health target) { }

    public void OnTurnStart(Health target) { }

    public void OnTurnEnd(Health target)
    {
        // 턴 종료 시 스택만큼 피해 + 스택 1 감소
        target.TakeDamage(Stack);
        Stack--;
        Debug.Log($"Poison 피해 {Stack + 1}, 남은 스택: {Stack}");
    }

    public void OnRemove(Health target)
    {
        Debug.Log("Poison 제거");
    }
}