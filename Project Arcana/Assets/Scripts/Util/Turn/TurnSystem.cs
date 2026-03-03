public class TurnSystem
{
    private ITurn _currentTurn;
    public PlayerState playerState;
    public MonsterState monsterState;

    public TurnSystem(BattleModel model, BattleHUD hud, BattleView view, BattlePresenter presenter)
    {
        playerState = new PlayerState(this, model, hud, view);
        monsterState = new MonsterState(this, model, presenter);
    }

    public void Init() => ChangeTurn(playerState);

    public void Update() => _currentTurn?.Update();

    public void ChangeTurn(ITurn nextTurn)
    {
        _currentTurn?.Exit();
        _currentTurn = nextTurn;
        _currentTurn?.Enter();
    }
}