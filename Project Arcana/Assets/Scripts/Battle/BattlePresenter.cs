using UnityEngine;

public class BattlePresenter
{
    private BattleModel _model;
    private BattleView _view;
    private TurnSystem _turnSystem;

    public BattlePresenter(BattleModel model, BattleView view)
    {
        _model = model;
        _view = view;

        _view.OnCardSelected += SelectCard;
        _view.OnCardUsed += UseCard;

        _turnSystem = new TurnSystem();
        _turnSystem.Init(); // 플레이어 턴 시작
        
        DrawInitialHand();
    }

    private void SelectCard(CardView cardView)
    {
        // 카드 선택 시 UI 표시
        cardView.Select();
        _view.RefreshHandLayout();
    }

    private void UseCard(CardView cardView)
    {
        // 모델에 카드 사용 반영
        _model.UseCard(cardView.GetCardData());
        _view.SpawnHand(_model.CurrentHand);

        // 사용 후 턴 종료 또는 추가 로직 가능
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
    
    private void DrawInitialHand()
    {
        _model.DrawCards(5); // 카드 5장 드로우
        _view.SpawnHand(_model.CurrentHand); // View에 표시
    }
}