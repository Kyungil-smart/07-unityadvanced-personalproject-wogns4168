using System.Collections.Generic;

[System.Serializable]
public class NodeSaveData
{
    public int floor;
    public int index;
    public string type;
    public bool isCleared;
    public bool isAccessible;
    public List<int> nextNodeIndices;
}