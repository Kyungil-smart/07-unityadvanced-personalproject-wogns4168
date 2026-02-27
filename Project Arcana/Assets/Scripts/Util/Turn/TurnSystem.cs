using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private ITurn _currentTurn;
    
    public PlayerState playerState;
    public MonsterState monsterState;

    private void Awake()
    {
        playerState = new PlayerState(this);
        monsterState = new MonsterState(this);
    }

    private void Start()
    {
        TurnChange(playerState);
    }
    
    public void Update()
    {
        _currentTurn?.Update();
    }
    
    public void TurnChange(ITurn nextTurn)
    {
        _currentTurn?.Exit();
        _currentTurn = nextTurn;
        _currentTurn?.Enter();
    }
}
