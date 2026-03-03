using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public BattleView battleView;
    public BattleHUD battleHUD;
    public List<MonsterBase> monsters;

    private BattlePresenter _presenter;
    private BattleModel _model;

    private void Start()
    {
        Deck deckToUse = RunManager.Instance != null ? RunManager.Instance.currentDeck : null;

        if (deckToUse == null)
        {
            Debug.LogError("덱이 null입니다! RunManager를 확인하세요.");
            return;
        }

        _model = new BattleModel(monsters, deckToUse);
        _presenter = new BattlePresenter(_model, battleView, battleHUD);
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