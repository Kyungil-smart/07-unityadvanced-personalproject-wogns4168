using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField] private List<MonsterBase> _monsters;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handArea;

    private List<CardView> _currentHandViews = new List<CardView>();
    private TurnSystem _turnSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _monsters = new List<MonsterBase>(
            FindObjectsByType<MonsterBase>(FindObjectsSortMode.None)
        );

        _turnSystem = new TurnSystem();
        _turnSystem.Init();
    }

    private void Start()
    {
        DrawCards(5);
    }

    private void Update()
    {
        _turnSystem.Update();
    }

    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CardData card = RunManager.Instance.currentDeck.Draw();
            if (card == null) return;

            GameObject obj = PoolManager.Instance.Spawn(cardPrefab);
            obj.transform.SetParent(handArea, false);

            CardView view = obj.GetComponent<CardView>();
            view.Setup(card);

            _currentHandViews.Add(view);
        }
    }

    public void OnCardUsed(CardView cardView)
    {
        RunManager.Instance.currentDeck.UseCard(cardView.GetCardData());
        _currentHandViews.Remove(cardView);

        PoolManager.Instance.Despawn(cardView.gameObject, cardPrefab);
    }

    public void OnClickEndPlayerTurn()
    {
        _turnSystem.ChangeTurn(_turnSystem.monsterState);
    }

    public void MonsterTurnStart()
    {
        StartCoroutine(MonsterTurnRoutine());
    }

    private IEnumerator MonsterTurnRoutine()
    {
        foreach (var monster in _monsters)
        {
            if (monster == null) continue;
            if (monster.isDead) continue; // Health에 있다 가정

            yield return StartCoroutine(monster.Act());
        }

        _turnSystem.ChangeTurn(_turnSystem.playerState);
    }
}