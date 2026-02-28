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

    private const int MaxHandSize = 9;   // 최대 9장

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

    // 카드 드로우
    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_currentHandViews.Count >= MaxHandSize)
                return;

            CardData card = RunManager.Instance.currentDeck.Draw();
            if (card == null) return;

            GameObject obj = PoolManager.Instance.Spawn(cardPrefab);
            obj.transform.SetParent(handArea, false);

            CardView view = obj.GetComponent<CardView>();
            view.Setup(card);

            _currentHandViews.Add(view);
        }
        
        RefreshHandLayout();
    }

    // 카드 사용
    public void OnCardUsed(CardView cardView)
    {
        RunManager.Instance.currentDeck.UseCard(cardView.GetCardData());
        _currentHandViews.Remove(cardView);

        PoolManager.Instance.Despawn(cardView.gameObject, cardPrefab);

        RefreshHandLayout();
    }

    // 🔥 핵심: 부채꼴 정렬
    private void RefreshHandLayout()
    {
        int count = _currentHandViews.Count;
        if (count == 0) return;

        float middleIndex = (count - 1) / 2f;   // 중앙 기준
        float spacing = 2f;                     // 카드 간 x 좌표 간격
        float angleStep = 10f;                   // 기울기
        float yStep = 0.5f;                       // y 축 내려가는 정도

        for (int i = 0; i < count; i++)
        {
            float offset = i - middleIndex;

            RectTransform rect = _currentHandViews[i].GetComponent<RectTransform>();

            float x = offset * spacing;
            float y = -Mathf.Abs(offset) * yStep;   // 양쪽으로 갈수록 조금씩 내려감
            float angle = -offset * angleStep;      // 좌우 반대로 기울이기

            rect.anchoredPosition = new Vector2(x, y);
            rect.localRotation = Quaternion.Euler(0, 0, angle);
        }
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
            if (monster.isDead) continue;

            yield return StartCoroutine(monster.Act());
        }

        _turnSystem.ChangeTurn(_turnSystem.playerState);
    }
}