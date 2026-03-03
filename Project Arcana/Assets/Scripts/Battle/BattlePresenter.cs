using UnityEngine;

public class BattlePresenter
{
    private BattleModel _model;
    private BattleView _view;
    private TurnSystem _turnSystem;
    private BattleHUD _hud;

    public BattlePresenter(BattleModel model, BattleView view, BattleHUD hud)
    {
        _model = model;
        _view = view;
        _hud = hud;

        _view.OnCardSelected += SelectCard;
        _view.OnCardUsed += UseCard;

        _hud.SetEndTurnCallback(OnEndTurnPressed);
        _hud.SetPlayerTurn();

        _turnSystem = new TurnSystem();
        _turnSystem.Init();

        DrawInitialHand();
        RefreshHUD();
    }

    private void SelectCard(CardView cardView)
    {
        _view.RefreshHandLayout();
    }

    private void UseCard(CardView cardView)
    {
        _model.UseCard(cardView.GetCardData());
        _view.SpawnHand(_model.CurrentHand);
        RefreshHUD();
    }

    private void OnEndTurnPressed()
    {
        _hud.SetEnemyTurn();
        _turnSystem.ChangeTurn(_turnSystem.monsterState);
    }

    private void RefreshHUD()
    {
        _hud.UpdateDeckInfo(
            _model.Deck.drawPile.Count,
            _model.Deck.discardPile.Count,
            _model.Deck.exhaustPile.Count
        );
        // _hud.UpdateEnergy(_model.CurrentEnergy, _model.MaxEnergy); // 에너지 구현 후 주석 해제
    }

    private void DrawInitialHand()
    {
        _model.DrawCards(5);
        _view.SpawnHand(_model.CurrentHand);
    }

    public void Update()
    {
        _turnSystem.Update();
    }

    public void OnDestroy()
    {
        _view.OnCardSelected -= SelectCard;
        _view.OnCardUsed -= UseCard;
    }
}