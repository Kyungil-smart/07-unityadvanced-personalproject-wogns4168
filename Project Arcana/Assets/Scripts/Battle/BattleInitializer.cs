using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public BattleView battleView;
    public BattleHUD battleHUD;
    public BattleResultPanel resultPanel;
    public CardRewardPanel rewardPanel;
    private List<MonsterBase> monsters;

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

        deckToUse.ResetForBattle();

        Player player = FindAnyObjectByType<Player>();

        if (RunManager.Instance.CurrentHp > 0)
            player.SetHealth(RunManager.Instance.CurrentHp, RunManager.Instance.MaxHp);

        RunManager.Instance.SavePlayerHp(player.currentHealth, player.maxHealth);

        // 몬스터 스폰
        MonsterSpawner spawner = FindAnyObjectByType<MonsterSpawner>();
        NodeType nodeType = RunManager.Instance.CurrentMapNode?.Type ?? NodeType.Battle;
        int floor = RunManager.Instance.CurrentMapNode?.Floor ?? 0;
        monsters = spawner.SpawnMonsters(nodeType, floor);

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