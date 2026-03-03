public interface IStatus
{
    string Name { get; }
    int Stack { get; set; }

    void OnApply(Health target);      // 부여 시
    void OnTurnStart(Health target);  // 턴 시작 시
    void OnTurnEnd(Health target);    // 턴 종료 시
    void OnRemove(Health target);     // 제거 시
}