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

        // 타입에 맞는 스프라이트 설정
        int spriteIndex = (int)node.Type;
        if (nodeSprites != null && spriteIndex < nodeSprites.Length)
            nodeImage.sprite = nodeSprites[spriteIndex];

        if (!node.IsAccessible)
        {
            nodeImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            return;
        }

        if (node.IsCleared)
        {
            nodeImage.color = new Color(1f, 1f, 1f, 0.4f);
            return;
        }

        nodeImage.color = Color.white;
        _isPulsing = true;
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