using System.Collections;

public class MonsterState : ITurn
{
    private TurnSystem _turnSystem;
    private BattleModel _model;
    private BattlePresenter _presenter;

    public MonsterState(TurnSystem turnSystem, BattleModel model, BattlePresenter presenter)
    {
        _turnSystem = turnSystem;
        _model = model;
        _presenter = presenter;
    }

    public void Enter()
    {
        foreach (var monster in _model.Monsters)
        {
            if (monster == null || monster.isDead) continue;
            monster.StartCoroutine(ActAndEndTurn(monster));
            return;
        }

        _turnSystem.ChangeTurn(_turnSystem.playerState);
    }

    private IEnumerator ActAndEndTurn(MonsterBase monster)
    {
        // 몬스터 턴 시작 상태이상 처리
        monster.StatusManager.OnTurnStart();

        yield return monster.StartCoroutine(monster.Act());

        // 몬스터 턴 종료 상태이상 처리
        monster.StatusManager.OnTurnEnd();

        _presenter.OnMonsterTurnEnd();

        if (_model.CheckBattleResult() == BattleResult.None)
            _turnSystem.ChangeTurn(_turnSystem.playerState);
    }

    public void Update() { }
    public void Exit() { }
}