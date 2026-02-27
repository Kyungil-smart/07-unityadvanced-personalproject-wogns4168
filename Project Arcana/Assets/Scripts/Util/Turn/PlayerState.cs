using UnityEngine;

public class PlayerState : ITurn
{
    TurnSystem _turnSystem;

    public PlayerState(TurnSystem turnSystem)
    {
        _turnSystem = turnSystem;
    }
    public void Enter()
    {
        Debug.Log("Entering PlayerState");
        // 플레이어 에너지 회복
        // 카드 드로우
    }

    public void Update()
    {
        // 카드 사용 로직
    }

    public void Exit()
    {
        // 현재 카드 모두 버림
        // 상태이상이 있다면 효과 적용 (poison)
    }
    
}