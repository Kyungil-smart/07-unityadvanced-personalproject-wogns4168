using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RectTransform iconTransform; // 크기 조절을 위해 RectTransform으로 받음

    [Header("Text Color Settings")]
    [SerializeField] private Color normalTextColor = new Color(0.8f, 0.2f, 0.2f, 1f); 
    [SerializeField] private Color hoverTextColor = Color.white;  
    [SerializeField] private Color pressedTextColor = Color.grey; 

    [Header("Scale Settings")]
    [SerializeField] private float hoverScale = 1.15f;  // 마우스 올렸을 때 크기 (1.15배)
    [SerializeField] private float pressedScale = 0.95f; // 클릭했을 때 크기 (0.95배)

    private Vector3 _originalTextScale;
    private Vector3 _originalIconScale;

    void Awake()
    {
        if (textMeshPro != null) _originalTextScale = textMeshPro.transform.localScale;
        if (iconTransform != null) _originalIconScale = iconTransform.localScale;
    }

    void Start()
    {
        ResetToNormal();
    }

    // 마우스 올렸을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetFeedback(hoverTextColor, hoverScale);
    }

    // 마우스 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetToNormal();
    }

    // 클릭했을 때
    public void OnPointerDown(PointerEventData eventData)
    {
        SetFeedback(pressedTextColor, pressedScale);
    }

    // 클릭 뗐을 때
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            SetFeedback(hoverTextColor, hoverScale);
        else
            ResetToNormal();
    }

    private void SetFeedback(Color txtColor, float scale)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = txtColor;
            textMeshPro.transform.localScale = _originalTextScale * scale;
        }

        if (iconTransform != null)
        {
            iconTransform.localScale = _originalIconScale * scale;
        }
    }

    private void ResetToNormal()
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = normalTextColor;
            textMeshPro.transform.localScale = _originalTextScale;
        }

        if (iconTransform != null)
        {
            iconTransform.localScale = _originalIconScale;
        }
    }
}