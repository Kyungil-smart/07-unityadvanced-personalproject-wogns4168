using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string _savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public bool HasSave()
    {
        return File.Exists(_savePath);
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.playerName = RunManager.Instance.PlayerName;
        data.currentHp = RunManager.Instance.CurrentHp;
        data.maxHp = RunManager.Instance.MaxHp;
        data.gold = RunManager.Instance.Gold;

        // 덱 저장 (카드 이름으로)
        data.deckCardNames = new List<string>();
        var deck = RunManager.Instance.currentDeck;
        foreach (var card in deck.drawPile) data.deckCardNames.Add(card.cardName);
        foreach (var card in deck.discardPile) data.deckCardNames.Add(card.cardName);
        foreach (var card in deck.exhaustPile) data.deckCardNames.Add(card.cardName);
        foreach (var card in deck.hand) data.deckCardNames.Add(card.cardName);

        // 맵 저장
        data.floors = new List<FloorSaveData>();
        foreach (var floor in MapManager.Instance.Floors)
        {
            FloorSaveData floorData = new FloorSaveData();
            floorData.nodes = new List<NodeSaveData>();
            foreach (var node in floor)
            {
                NodeSaveData nodeData = new NodeSaveData
                {
                    floor = node.Floor,
                    index = node.Index,
                    type = node.Type.ToString(),
                    isCleared = node.IsCleared,
                    isAccessible = node.IsAccessible,
                    nextNodeIndices = new List<int>()
                };
                foreach (var next in node.NextNodes)
                    nodeData.nextNodeIndices.Add(next.Index);
                floorData.nodes.Add(nodeData);
            }
            data.floors.Add(floorData);
        }

        // 현재 노드
        var currentNode = RunManager.Instance.CurrentMapNode;
        if (currentNode != null)
        {
            data.currentNodeFloor = currentNode.Floor;
            data.currentNodeIndex = currentNode.Index;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savePath, json);
        Debug.Log($"저장 완료: {_savePath}");
    }

    public void Load()
    {
        if (!HasSave()) { Debug.LogWarning("세이브 파일 없음"); return; }

        string json = File.ReadAllText(_savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // 기본 데이터 복원
        RunManager.Instance.SetPlayerName(data.playerName);
        RunManager.Instance.SavePlayerHp(data.currentHp, data.maxHp);
        RunManager.Instance.SetGold(data.gold);

        // 덱 복원
        RunManager.Instance.currentDeck = new Deck();
        foreach (var cardName in data.deckCardNames)
        {
            CardData card = RunManager.Instance.allCards.Find(c => c.cardName == cardName);
            if (card != null) RunManager.Instance.currentDeck.AddCard(card);
            else Debug.LogWarning($"카드 못 찾음: {cardName}");
        }

        // 맵 복원
        List<List<MapNode>> floors = new List<List<MapNode>>();
        for (int f = 0; f < data.floors.Count; f++)
        {
            List<MapNode> floorNodes = new List<MapNode>();
            foreach (var nodeData in data.floors[f].nodes)
            {
                MapNode node = new MapNode
                {
                    Floor = nodeData.floor,
                    Index = nodeData.index,
                    Type = (NodeType)System.Enum.Parse(typeof(NodeType), nodeData.type),
                    IsCleared = nodeData.isCleared,
                    IsAccessible = nodeData.isAccessible
                };
                floorNodes.Add(node);
            }
            floors.Add(floorNodes);
        }

        // NextNodes 연결
        for (int f = 0; f < data.floors.Count; f++)
        {
            for (int n = 0; n < data.floors[f].nodes.Count; n++)
            {
                foreach (var nextIndex in data.floors[f].nodes[n].nextNodeIndices)
                {
                    if (f + 1 < floors.Count && nextIndex < floors[f + 1].Count)
                        floors[f][n].NextNodes.Add(floors[f + 1][nextIndex]);
                }
            }
        }

        MapManager.Instance.LoadFloors(floors);

        // 현재 노드 복원
        if (data.currentNodeFloor >= 0 && data.currentNodeFloor < floors.Count)
        {
            MapNode currentNode = floors[data.currentNodeFloor][data.currentNodeIndex];
            RunManager.Instance.SetCurrentNode(currentNode);
        }

        Debug.Log("불러오기 완료!");
    }

    public void DeleteSave()
    {
        if (File.Exists(_savePath))
            File.Delete(_savePath);
    }
}