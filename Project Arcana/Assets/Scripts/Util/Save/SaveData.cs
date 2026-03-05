using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string playerName;
    public float currentHp;
    public float maxHp;
    public int gold;
    public List<string> deckCardNames; // CardData 이름으로 저장
    public List<FloorSaveData> floors; // 맵 진행상황
    public int currentNodeFloor;
    public int currentNodeIndex;
}