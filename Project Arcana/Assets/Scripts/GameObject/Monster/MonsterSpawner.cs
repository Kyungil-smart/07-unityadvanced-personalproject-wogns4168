using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] monsterPrefabs; // Inspector에서 6개 연결

    private List<MonsterData> _monsterTable = new List<MonsterData>();

    public List<MonsterBase> SpawnMonsters(NodeType nodeType, int floor)
    {
        LoadCSV();

        List<MonsterData> pool = GetMonsterPool(nodeType);
        List<MonsterData> selected = SelectMonsters(pool, nodeType, floor);
        List<MonsterBase> spawned = new List<MonsterBase>();

        for (int i = 0; i < selected.Count; i++)
        {
            GameObject prefab = GetPrefab(selected[i].prefabName);
            if (prefab == null) continue;

            GameObject obj = Instantiate(prefab, spawnPoints[i].position, Quaternion.identity);
            MonsterBase monster = obj.GetComponent<MonsterBase>();

            // 층수별 스탯 증가
            float hp = selected[i].baseHp * (1f + floor * 0.2f);
            float attack = selected[i].baseAttack * (1f + floor * 0.1f);

            // 정수처리
            hp = Mathf.Round(hp);
            attack = Mathf.Round(attack);
            
            monster.SetStats(hp, attack);
            

            spawned.Add(monster);
        }

        return spawned;
    }

    private void LoadCSV()
    {
        _monsterTable.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "monsters.csv");
        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] col = lines[i].Split(',');
            _monsterTable.Add(new MonsterData
            {
                name = col[0],
                tier = col[1],
                baseHp = float.Parse(col[2]),
                baseAttack = float.Parse(col[3]),
                prefabName = col[4]
            });
        }
    }

    private List<MonsterData> GetMonsterPool(NodeType nodeType)
    {
        string tier = nodeType switch
        {
            NodeType.Elite => "Elite",
            NodeType.Boss => "Boss",
            _ => "Normal"
        };

        return _monsterTable.FindAll(m => m.tier == tier);
    }

    private List<MonsterData> SelectMonsters(List<MonsterData> pool, NodeType nodeType, int floor)
    {
        List<MonsterData> selected = new List<MonsterData>();

        if (nodeType == NodeType.Boss)
        {
            selected.Add(pool[0]);
        }
        else if (nodeType == NodeType.Elite)
        {
            int count = Random.Range(1, 3); // 1~2마리
            List<MonsterData> shuffled = new List<MonsterData>(pool);
            Shuffle(shuffled);
            for (int i = 0; i < count; i++)
                selected.Add(shuffled[i % shuffled.Count]);
        }
        else
        {
            int count;
            if (floor == 0)
            {
                count = 1; // 1층 무조건 1마리
            }
            else if (floor <= 3)
            {
                // 2~4층: 1마리 60%, 2마리 40%
                count = Random.Range(0, 100) < 60 ? 1 : 2;
            }
            else if (floor <= 6)
            {
                // 5~7층: 1마리 30%, 2마리 50%, 3마리 20%
                int rand = Random.Range(0, 100);
                count = rand < 30 ? 1 : rand < 80 ? 2 : 3;
            }
            else
            {
                // 8~9층: 2마리 40%, 3마리 60%
                count = Random.Range(0, 100) < 40 ? 2 : 3;
            }

            List<MonsterData> shuffled = new List<MonsterData>(pool);
            Shuffle(shuffled);
            for (int i = 0; i < count; i++)
                selected.Add(shuffled[i % shuffled.Count]);
        }

        return selected;
    }

    private GameObject GetPrefab(string prefabName)
    {
        foreach (var prefab in monsterPrefabs)
            if (prefab.name == prefabName) return prefab;
        return null;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}