using System.Collections.Generic;

public class MapNode
{
    public int Floor { get; set; }
    public int Index { get; set; }
    public NodeType Type { get; set; }
    public bool IsCleared { get; set; }
    public bool IsAccessible { get; set; }
    public List<MapNode> NextNodes { get; set; } = new List<MapNode>();
}