using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private RectTransform baseRect;
    [SerializeField] private Transform origin;
    [SerializeField] private bool startsActive = false;

    private RectTransform myRect;
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
        if (!isActive || origin == null) return;

        RectTransform canvasRect = (RectTransform)canvas.transform;
        RectTransform originRect = origin as RectTransform;

        Vector2 originLocal;
        Vector2 mouseLocal;

        if (originRect != null)
        {
            // UI RectTransform → Canvas 로컬 좌표 직접 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                RectTransformUtility.WorldToScreenPoint(null, originRect.position),
                null,
                out originLocal
            );
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Camera.main.WorldToScreenPoint(origin.position),
                null,
                out originLocal
            );
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null,
            out mouseLocal
        );

        Debug.Log($"originLocal: {originLocal}, mouseLocal: {mouseLocal}");

        myRect.localPosition = originLocal;

        Vector2 dir = mouseLocal - originLocal;
        float length = dir.magnitude / 100f;
        baseRect.localScale = new Vector3(1, Mathf.Max(length, 0.01f), 1);

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

    public void SetupAndActivate(Transform origin)
    {
        this.origin = origin;
        Activate();
    }
}