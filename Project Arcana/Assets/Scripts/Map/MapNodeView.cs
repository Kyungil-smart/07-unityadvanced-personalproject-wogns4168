using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapNodeView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image nodeImage;
    [SerializeField] private Sprite[] nodeSprites; // 0=Battle, 1=Elite, 2=Shop, 3=Event, 4=Boss

    private MapNode _node;
    private Vector3 _originalScale;
    private bool _isPulsing;

    public void Setup(MapNode node)
    {
        _node = node;
        _originalScale = transform.localScale;

        int spriteIndex = (int)node.Type;
        if (nodeSprites != null && spriteIndex < nodeSprites.Length)
            nodeImage.sprite = nodeSprites[spriteIndex];

        // 상태에 따른 색상 분기
        if (node.IsCleared)
        {
            // 이미 클리어한 노드 (회색빛 혹은 반투명)
            nodeImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            _isPulsing = false;
        }
        else if (node.IsAccessible)
        {
            // 현재 갈 수 있는 노드 (밝게 표시 + 두근거림)
            nodeImage.color = Color.white;
            _isPulsing = true; 
        }
        else
        {
            // 갈 수 없는 노드 (어둡고 클릭 불가)
            nodeImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            _isPulsing = false;
        }
    }

    private void Update()
    {
        if (!_isPulsing) return;
        float scale = 1f + Mathf.Sin(Time.time * 3f) * 0.05f;
        transform.localScale = _originalScale * scale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_node == null || !_node.IsAccessible || _node.IsCleared) return;
        MapManager.Instance.SelectNode(_node);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_node == null || !_node.IsAccessible || _node.IsCleared) return;
        transform.localScale = _originalScale * 1.15f;
        _isPulsing = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_node == null || !_node.IsAccessible || _node.IsCleared) return;
        transform.localScale = _originalScale;
        _isPulsing = true;
    }
}