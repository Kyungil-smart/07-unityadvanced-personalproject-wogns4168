using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public BattleView battleView;
    public BattleHUD battleHUD;
    public BattleResultPanel resultPanel;
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

        Player player = FindAnyObjectByType<Player>();
        _model = new BattleModel(monsters, deckToUse, player);
        BattleContext context = new BattleContext(player, monsters, _model);
        _presenter = new BattlePresenter(_model, battleView, battleHUD, context, resultPanel);
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