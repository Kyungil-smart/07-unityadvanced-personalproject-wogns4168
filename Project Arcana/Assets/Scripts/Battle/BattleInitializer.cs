using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    public BattleView battleView;
    public Deck deck; // 테스트용 덱
    public List<MonsterBase> monsters; // 테스트용 몬스터

    private BattlePresenter _presenter;
    private BattleModel _model;

    private void Start()
    {
        Deck deckToUse = (RunManager.Instance != null) 
            ? RunManager.Instance.currentDeck 
            : deck;

        if (deckToUse == null)
        {
            Debug.LogError("덱이 null입니다! RunManager 또는 deck 필드를 확인하세요.");
            return;
        }

        _model = new BattleModel(monsters, deckToUse);
        _presenter = new BattlePresenter(_model, battleView);
    }

    private void Update()
    {
        _presenter.Update();
    }

    private void OnDestroy()
    {
        _presenter.OnDestroy();
    }
}