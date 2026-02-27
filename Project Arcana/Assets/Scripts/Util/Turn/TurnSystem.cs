using System;
using UnityEngine;

public class TurnSystem
{
    private ITurn _currentTurn;
    
    public PlayerState playerState;
    public MonsterState monsterState;

    public TurnSystem()
    {
        playerState = new PlayerState(this);
        monsterState = new MonsterState(this);
    }

    public void Init()
    {
        ChangeTurn(playerState);
    }
    
    public void Update()
    {
        _currentTurn?.Update();
    }

    public void ChangeTurn(ITurn nextTurn)
    {
        _currentTurn?.Exit();
        _currentTurn = nextTurn;
        _currentTurn?.Enter();
    }
    
}
