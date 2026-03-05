using System.Collections.Generic;
using UnityEngine;

public class BattlePresenter
{
    private BattleModel _model;
    private BattleView _view;
    private TurnSystem _turnSystem;
    private BattleHUD _hud;
    private CardEffectProcessor _effectProcessor;
    private BattleResultPanel _resultPanel;
    private CardRewardPanel _rewardPanel;
    private BattleContext _context;
    private int _goldReward;

    public BattlePresenter(BattleModel model, BattleView view, BattleHUD hud, BattleContext context, BattleResultPanel resultPanel, CardRewardPanel rewardPanel)
    {
        _model = model;
        _view = view;
        _hud = hud;
        _resultPanel = resultPanel;
        _rewardPanel = rewardPanel;
        _context = context;
        _effectProcessor = new CardEffectProcessor(context);
        
        int floor = RunManager.Instance.CurrentMapNode?.Floor ?? 0;
        int monsterCount = _model.Monsters.Count;
        NodeType nodeType = RunManager.Instance.CurrentMapNode?.Type ?? NodeType.Battle;

        _goldReward = nodeType switch
        {
            NodeType.Elite => (25 + floor * 5) * monsterCount,
            NodeType.Boss => 100,
            _ => (10 + floor * 3) * monsterCount
        };

        _view.OnCardSelected += SelectCard;
        _view.OnCardUsed += UseCard;

        _hud.SetEndTurnCallback(OnEndTurnPressed);

        _turnSystem = new TurnSystem(_model, _hud, _view, this, context.Player);
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
        
        foreach (var monster in _model.Monsters)
            monster.GetComponent<MonsterIntentUI>()?.UpdateIntent();

        // 카드 사용 후 전투 종료 체크
        CheckBattleEnd();
    }
    private void OnEndTurnPressed()
    {
        _hud.SetEnemyTurn();
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
            RunManager.Instance.SavePlayerHp(
                _context.Player.currentHealth,
                _context.Player.maxHealth);
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
        _hud.SetEndTurnInteractable(false);
        _view.ClearHand();

        _resultPanel.ShowVictory(_goldReward,
            onGoldCollect: () => { },
            onCardReward: () =>
            {
                List<CardData> rewardCards = RunManager.Instance.GetRandomRewardCards();
                _rewardPanel.Show(rewardCards, () =>
                {
                    _resultPanel.HideAll(); // 카드 선택 후 dimPanel까지 끄기
                    MapManager.Instance.OnNodeCleared();
                });
            }
        );
    }

    private void OnDefeat()
    {
        _hud.SetEndTurnInteractable(false);
        _view.ClearHand();

        _resultPanel.ShowDefeat(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
    }
}