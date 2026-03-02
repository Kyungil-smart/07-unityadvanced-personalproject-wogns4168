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
    public void RefreshHandLayout()
    {
        int count = _currentHandViews.Count;
        if (count == 0) return;

        float middleIndex = (count - 1) / 2f;
        float spacing = 2f;
        float angleStep = 10f;
        float yStep = 0.5f;

        for (int i = 0; i < count; i++)
        {
            CardView card = _currentHandViews[i];
            RectTransform rect = card.GetComponent<RectTransform>();

            float offset = i - middleIndex;

            float x = offset * spacing;
            float y = -Mathf.Abs(offset) * yStep;
            float angle = -offset * angleStep;

            Vector2 finalPos = new Vector2(x, y);
            Quaternion finalRot = Quaternion.Euler(0, 0, angle);
            Vector3 finalScale = Vector3.one * 0.8f;

            // 🔥 Hover일 때만 살짝 위로 + 확대
            if (card.IsHover && !card.IsDragging)
            {
                finalPos += new Vector2(0, 3f);
                finalScale = Vector3.one;

                rect.SetAsLastSibling();
            }

            // 🔥 선택 고정
            if (card.IsSelected)
            {
                finalPos += new Vector2(0, 3f);
                finalScale = Vector3.one;

                rect.SetAsLastSibling();
            }

            // 🔥 드래그 중
            if (card.IsDragging)
            {
                rect.SetAsLastSibling();
            }

            rect.anchoredPosition = finalPos;
            rect.localRotation = finalRot;
            rect.localScale = finalScale;
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