public class PlayerState : ITurn
{
    TurnSystem _turnSystem;

    public PlayerState(TurnSystem turnSystem)
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