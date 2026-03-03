using UnityEngine;

public class BattlePresenter
{
    private BattleModel _model;
    private BattleView _view;
    private TurnSystem _turnSystem;
    private BattleHUD _hud;
    private CardEffectProcessor _effectProcessor;

    public BattlePresenter(BattleModel model, BattleView view, BattleHUD hud, BattleContext context)
    {
        _model = model;
        _view = view;
        _hud = hud;
        _effectProcessor = new CardEffectProcessor(context);

        _view.OnCardSelected += SelectCard;
        _view.OnCardUsed += UseCard;

        _hud.SetEndTurnCallback(OnEndTurnPressed);
        
        _turnSystem = new TurnSystem(_model, _hud, _view, this);
        _turnSystem.Init();

        RefreshHUD();
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

    private void SelectCard(CardView cardView)
    {
        _view.RefreshHandLayout();
    }

    private void UseCard(CardView cardView, ITargetable target)
    {
        CardData card = cardView.GetCardData();

        bool success = _effectProcessor.Process(card, target);
        if (!success) return;

        _model.UseCard(card);
        _view.SpawnHand(_model.CurrentHand);
        RefreshHUD();

        // 카드 사용 후 전투 종료 체크
        CheckBattleEnd();
    }
    private void OnEndTurnPressed()
    {
        _turnSystem.ChangeTurn(_turnSystem.monsterState);
    }
    
    public void OnMonsterTurnEnd()
    {
        CheckBattleEnd();
    }

    private void RefreshHUD()
    {
        _hud.UpdateDeckInfo(
            _model.Deck.drawPile.Count,
            _model.Deck.discardPile.Count,
            _model.Deck.exhaustPile.Count
        );
        // 에너지 갱신 추가
        _hud.UpdateEnergy(_model.CurrentEnergy, _model.MaxEnergy);
    }

    
    private void CheckBattleEnd()
    {
        BattleResult result = _model.CheckBattleResult();

        if (result == BattleResult.Victory)
        {
            Debug.Log("전투 승리!");
            _hud.SetEndTurnInteractable(false); // 버튼 비활성화
            OnVictory();
        }
        else if (result == BattleResult.Defeat)
        {
            Debug.Log("전투 패배!");
            _hud.SetEndTurnInteractable(false);
            OnDefeat();
        }
    }

    private void OnVictory()
    {
        // 추후 보상 화면 연결
        Debug.Log("보상 화면으로 이동");
    }

    private void OnDefeat()
    {
        // 추후 게임오버 화면 연결
        Debug.Log("게임오버");
    }
}