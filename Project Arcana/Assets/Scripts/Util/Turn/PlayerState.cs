using UnityEngine;

public class PlayerState : ITurn
{
    private TurnSystem _turnSystem;
    private BattleModel _model;
    private BattleHUD _hud;
    private BattleView _view;
    private Player _player;

    public PlayerState(TurnSystem turnSystem, BattleModel model, BattleHUD hud, BattleView view, Player player)
    {
        _turnSystem = turnSystem;
        _model = model;
        _hud = hud;
        _view = view;
        _player = player;
    }

    public void Enter()
    {
        // Intent + 상태이상 UI 갱신
        foreach (var monster in _model.Monsters)
            monster.GetComponent<MonsterIntentUI>()?.UpdateIntent();

        _player?.StatusManager.OnTurnStart();
        _player?.ResetShield();
        _model.RefillEnergy();
        _model.DrawCards(5);
        _view.SpawnHand(_model.CurrentHand);
        _hud.SetPlayerTurn();
        RefreshHUD();
    }

    public void Update() { }

    public void Exit()
    {
        Debug.Log("플레이어 턴 종료");

        // 상태이상 턴 종료 처리 (Poison 피해 등)
        _player?.StatusManager.OnTurnEnd();

        _model.DiscardHand();
        _view.SpawnHand(_model.CurrentHand);
        RefreshHUD();
    }

    private void RefreshHUD()
    {
        _hud.UpdateDeckInfo(
            _model.Deck.drawPile.Count,
            _model.Deck.discardPile.Count,
            _model.Deck.exhaustPile.Count
        );
        _hud.UpdateEnergy(_model.CurrentEnergy, _model.MaxEnergy);
    }
}