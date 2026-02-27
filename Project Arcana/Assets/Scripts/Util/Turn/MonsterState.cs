using System.Collections;
using UnityEngine;

public class MonsterState : ITurn
{
    TurnSystem _turnSystem;

    public MonsterState(TurnSystem turnSystem)
    {
        _turnSystem = turnSystem;
    }


    public void Enter()
    {
        Debug.Log("Entering MonsterState");
        BattleManager.Instance.MonsterTurnStart();
    }

    public void Update()
    {
    }

    public void Exit()
    {
        // 상태이상이 있다면 효과 적용 (poison)
    }
    
}