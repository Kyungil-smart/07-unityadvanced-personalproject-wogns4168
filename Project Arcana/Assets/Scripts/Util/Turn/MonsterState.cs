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
        yield return monster.StartCoroutine(monster.Act());

        // 몬스터 공격 후 전투 종료 체크
        _presenter.OnMonsterTurnEnd();

        // 전투가 끝나지 않았으면 플레이어 턴으로
        if (_model.CheckBattleResult() == BattleResult.None)
            _turnSystem.ChangeTurn(_turnSystem.playerState);
    }

    public void Update() { }
    public void Exit() { }
}