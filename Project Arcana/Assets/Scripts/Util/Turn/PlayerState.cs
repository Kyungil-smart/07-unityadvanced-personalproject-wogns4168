using UnityEngine;

public class PlayerState : ITurn
{
    private TurnSystem _turnSystem;
    private BattleModel _model;
    private BattleHUD _hud;
    private BattleView _view;

    public PlayerState(TurnSystem turnSystem, BattleModel model, BattleHUD hud, BattleView view)
    {
        _turnSystem = turnSystem;
        _model = model;
        _hud = hud;
        _view = view;
    }

    public void Enter()
    {
        Debug.Log("플레이어 턴 시작");

        // 에너지 회복
        _model.RefillEnergy();

        // 카드 5장 드로우
        _model.DrawCards(5);
        _view.SpawnHand(_model.CurrentHand);

        // HUD 갱신
        _hud.SetPlayerTurn();
        RefreshHUD();
    }

    public void Update() { }

    public void Exit()
    {
        Debug.Log("플레이어 턴 종료");

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