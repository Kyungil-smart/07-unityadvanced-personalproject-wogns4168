using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public BattleView battleView;
    public BattleHUD battleHUD;
    public BattleResultPanel resultPanel;
    public CardRewardPanel rewardPanel;
    public List<MonsterBase> monsters;

    private BattlePresenter _presenter;
    private BattleModel _model;

    private void Start()
    {
        Deck deckToUse = RunManager.Instance != null ? RunManager.Instance.currentDeck : null;

        if (deckToUse == null)
        {
            Debug.LogError("덱이 null");
            return;
        }

        // 덱 초기화 (손패/버림패 → 드로우파일로)
        deckToUse.ResetForBattle();

        Player player = FindAnyObjectByType<Player>();

        // 저장된 체력 복원
        if (RunManager.Instance.CurrentHp > 0)
            player.SetHealth(RunManager.Instance.CurrentHp, RunManager.Instance.MaxHp);

        _model = new BattleModel(monsters, deckToUse, player);
        BattleContext context = new BattleContext(player, monsters, _model);
        _presenter = new BattlePresenter(_model, battleView, battleHUD, context, resultPanel, rewardPanel);
    }

    private void Update()
    {
        _presenter?.Update();
    }

    private void OnDestroy()
    {
        _presenter?.OnDestroy();
    }
}