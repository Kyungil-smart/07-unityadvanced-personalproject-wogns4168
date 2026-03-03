using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private RectTransform baseRect;
    [SerializeField] private bool startsActive = false;

    private RectTransform myRect;
    private RectTransform originRect;
    private Canvas canvas;
    private bool isActive;

    private void Awake()
    {
        myRect = (RectTransform)transform;
        canvas = GetComponentInParent<Canvas>();
        baseRect.pivot = new Vector2(0.5f, 0f);
        SetActive(startsActive);
    }

    private void Update()
    {
        if (!isActive || originRect == null) return;

        myRect.SetAsLastSibling();

        float scaleFactor = canvas.scaleFactor;
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        // 카드가 속한 Canvas의 카메라로 스크린 좌표 변환
        Canvas originCanvas = originRect.GetComponentInParent<Canvas>();
        Camera originCamera = originCanvas.renderMode == RenderMode.ScreenSpaceOverlay 
            ? null 
            : originCanvas.worldCamera;

        Vector2 originScreen = RectTransformUtility.WorldToScreenPoint(originCamera, originRect.position);
        Vector2 mouseScreen = Input.mousePosition;

        myRect.anchoredPosition = (originScreen - screenCenter) / scaleFactor;

        Vector2 dir = mouseScreen - originScreen;
        float length = dir.magnitude;

        float baseHeightPixels = baseRect.rect.height * scaleFactor;
        baseRect.localScale = new Vector3(1, length / baseHeightPixels, 1);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        baseRect.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void SetActive(bool b)
    {
        isActive = b;
        if (baseRect != null)
            baseRect.gameObject.SetActive(b);
    }

    public void Activate() => SetActive(true);
    public void Deactivate() => SetActive(false);

    public void SetupAndActivate(RectTransform origin)
    {
        originRect = origin;
        Activate();
    }
}