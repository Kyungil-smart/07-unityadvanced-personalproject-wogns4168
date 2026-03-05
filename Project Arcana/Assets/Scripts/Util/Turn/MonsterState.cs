using System.Collections;
using System.Collections.Generic;

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
        List<MonsterBase> aliveMonsters = new List<MonsterBase>();
        foreach (var monster in _model.Monsters)
        {
            if (monster == null || monster.isDead) continue;
            aliveMonsters.Add(monster);
        }

        if (aliveMonsters.Count == 0)
        {
            _turnSystem.ChangeTurn(_turnSystem.playerState);
            return;
        }

        // 첫 번째 몬스터에서 코루틴 시작 (순서대로 실행)
        aliveMonsters[0].StartCoroutine(ActAllMonsters(aliveMonsters));
    }

    private IEnumerator ActAllMonsters(List<MonsterBase> monsters)
    {
        foreach (var monster in monsters)
        {
            if (monster == null || monster.isDead) continue;

            monster.StatusManager.OnTurnStart();
            yield return monster.StartCoroutine(monster.Act());
            monster.StatusManager.OnTurnEnd();

            // 전투 종료 체크
            if (_model.CheckBattleResult() != BattleResult.None)
            {
                _presenter.OnMonsterTurnEnd();
                yield break;
            }
        }

        _presenter.OnMonsterTurnEnd();

        if (_model.CheckBattleResult() == BattleResult.None)
            _turnSystem.ChangeTurn(_turnSystem.playerState);
    }

    public void Update() { }
    public void Exit() { }
}