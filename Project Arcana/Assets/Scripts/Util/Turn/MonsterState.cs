using UnityEngine;

public class MonsterState : ITurn
{
    private TurnSystem _turnSystem;

    public MonsterState(TurnSystem turnSystem)
    {
        _turnSystem = turnSystem;
    }

    public void Enter()
    {
        Debug.Log("Monster's turn started");

        // 예: 몬스터 공격 로직
        // 여기서 공격 후 플레이어 턴으로 전환
        _turnSystem.ChangeTurn(_turnSystem.playerState);
    }

    public void Update() { }

    public void Exit()
    {
        Debug.Log("Monster's turn ended");
    }
}