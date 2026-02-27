public class MonsterState : ITurn
{
    TurnSystem _turnSystem;

    public MonsterState(TurnSystem turnSystem)
    {
        _turnSystem = turnSystem;
    }
    
    
    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}