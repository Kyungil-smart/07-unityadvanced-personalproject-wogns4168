using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("맵 설정")]
    [SerializeField] private int totalFloors = 10;
    [SerializeField] private int maxNodesPerFloor = 3;

    [Header("씬 이름")]
    [SerializeField] private string battleScene = "BattleScene";
    [SerializeField] private string eventScene = "EventScene";
    [SerializeField] private string shopScene = "ShopScene";
    [SerializeField] private string mapScene = "MapScene";

    public List<List<MapNode>> Floors { get; private set; }
    public MapNode CurrentNode { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GenerateMap()
    {
        Floors = new List<List<MapNode>>();

        for (int floor = 0; floor < totalFloors; floor++)
        {
            List<MapNode> floorNodes = new List<MapNode>();
            int count = (floor == totalFloors - 1) ? 1 : Random.Range(1, maxNodesPerFloor + 1);

            for (int i = 0; i < count; i++)
            {
                MapNode node = new MapNode
                {
                    Floor = floor,
                    Index = i,
                    Type = GetNodeType(floor),
                    IsCleared = false,
                    IsAccessible = floor == 0
                };
                floorNodes.Add(node);
            }
            Floors.Add(floorNodes);
        }

        ConnectNodes();
    }

    private NodeType GetNodeType(int floor)
    {
        // 보스는 마지막 층 고정
        if (floor == totalFloors - 1) return NodeType.Boss;

        // 초반 (0~2층): 전투 70%, 이벤트 20%, 상점 10%
        if (floor <= 2)
            return GetWeightedRandom(new[] { NodeType.Battle, NodeType.Event, NodeType.Shop },
                                     new[] { 70, 20, 10 });

        // 중반 (3~6층): 전투 50%, 엘리트 20%, 이벤트 20%, 상점 10%
        if (floor <= 6)
            return GetWeightedRandom(new[] { NodeType.Battle, NodeType.Elite, NodeType.Event, NodeType.Shop },
                                     new[] { 50, 20, 20, 10 });

        // 후반 (7~8층): 전투 40%, 엘리트 30%, 이벤트 20%, 상점 10%
        return GetWeightedRandom(new[] { NodeType.Battle, NodeType.Elite, NodeType.Event, NodeType.Shop },
                                 new[] { 40, 30, 20, 10 });
    }

    private NodeType GetWeightedRandom(NodeType[] types, int[] weights)
    {
        int total = 0;
        foreach (var w in weights) total += w;

        int rand = Random.Range(0, total);
        int cumulative = 0;

        for (int i = 0; i < types.Length; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative) return types[i];
        }

        return NodeType.Battle;
    }

    private void ConnectNodes()
    {
        for (int floor = 0; floor < Floors.Count - 1; floor++)
        {
            List<MapNode> current = Floors[floor];
            List<MapNode> next = Floors[floor + 1];

            // 다음 층 노드마다 부모 1개만 연결
            List<MapNode> shuffledNext = new List<MapNode>(next);
            Shuffle(shuffledNext);

            // 현재 층 노드가 최소 1개 연결 보장
            for (int i = 0; i < next.Count; i++)
            {
                MapNode parent = current[i % current.Count];
                if (!parent.NextNodes.Contains(shuffledNext[i]))
                    parent.NextNodes.Add(shuffledNext[i]);
            }

            // 연결 안 된 현재 층 노드 → 랜덤 다음 노드 연결
            foreach (var node in current)
            {
                if (node.NextNodes.Count == 0)
                    node.NextNodes.Add(next[Random.Range(0, next.Count)]);
            }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void SelectNode(MapNode node)
    {
        CurrentNode = node;
        RunManager.Instance.SetCurrentNode(node);

        switch (node.Type)
        {
            case NodeType.Battle:
            case NodeType.Elite:
            case NodeType.Boss:
                SceneManager.LoadScene(battleScene);
                break;
            case NodeType.Event:
                SceneManager.LoadScene(eventScene);
                break;
            case NodeType.Shop:
                SceneManager.LoadScene(shopScene);
                break;
        }
    }

    public void OnNodeCleared()
    {
        if (CurrentNode == null) return;
        CurrentNode.IsCleared = true;

        foreach (var next in CurrentNode.NextNodes)
            next.IsAccessible = true;

        SceneManager.LoadScene(mapScene);
    }
}