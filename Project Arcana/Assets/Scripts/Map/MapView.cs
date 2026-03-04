using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    [Header("노드 프리팹")]
    [SerializeField] private GameObject nodePrefab;

    [Header("맵 설정")]
    [SerializeField] private RectTransform content;
    private float floorHeight = 400f;
    private float nodeSpacing = 400f;

    private List<List<MapNodeView>> _nodeViews = new List<List<MapNodeView>>();

    private void Start()
    {
        if (MapManager.Instance.Floors == null)
            MapManager.Instance.GenerateMap();

        DrawMap();
    }

    private void DrawMap()
    {
        var floors = MapManager.Instance.Floors;

        // Content 높이 자동 설정
        content.sizeDelta = new Vector2(content.sizeDelta.x, floors.Count * floorHeight + 550f);

        for (int floor = 0; floor < floors.Count; floor++)
        {
            List<MapNodeView> floorViews = new List<MapNodeView>();
            List<MapNode> nodes = floors[floor];

            for (int i = 0; i < nodes.Count; i++)
            {
                GameObject obj = Instantiate(nodePrefab, content);
                RectTransform rect = obj.GetComponent<RectTransform>();

                // 위치 계산 (아래서 위로)
                float totalWidth = (nodes.Count - 1) * nodeSpacing;
                float x = -totalWidth / 2f + i * nodeSpacing;
                float y = floor * floorHeight + -1700f;
                rect.anchoredPosition = new Vector2(x, y);

                MapNodeView view = obj.GetComponent<MapNodeView>();
                view.Setup(nodes[i]);
                floorViews.Add(view);
            }
            _nodeViews.Add(floorViews);
        }

        DrawLines(floors);
    }
    

    private void DrawLines(List<List<MapNode>> floors)
    {
        for (int floor = 0; floor < floors.Count - 1; floor++)
        {
            foreach (var node in floors[floor])
            {
                foreach (var next in node.NextNodes)
                {
                    RectTransform from = _nodeViews[floor][node.Index].GetComponent<RectTransform>();
                    // next.Index가 다음층 _nodeViews 범위 안인지 체크
                    if (next.Index < _nodeViews[floor + 1].Count)
                    {
                        RectTransform to = _nodeViews[floor + 1][next.Index].GetComponent<RectTransform>();
                        DrawLine(from.anchoredPosition, to.anchoredPosition);
                    }
                }
            }
        }
    }

    private void DrawLine(Vector2 from, Vector2 to)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(content, false);
        lineObj.transform.SetSiblingIndex(1);

        Image line = lineObj.AddComponent<Image>();
        line.color = new Color(1f, 1f, 1f, 0.3f);

        RectTransform rect = lineObj.GetComponent<RectTransform>();
        Vector2 dir = to - from;
        rect.sizeDelta = new Vector2(dir.magnitude, 3f);
        rect.anchoredPosition = from + dir * 0.5f;
        rect.localRotation = Quaternion.FromToRotation(Vector3.right, dir);
    }
}