using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarSlider : MonoBehaviour
{
    [SerializeField] private GameObject sliderPrefab; // Slider + TMP Text
    [SerializeField] private Transform target;        // 발 밑 빈 오브젝트
    [SerializeField] private Canvas hpCanvas;         // HPBar용 Canvas
    [SerializeField] private Vector3 offset = new Vector3(0, -0.3f, 0);

    private Health _health;
    private Slider _slider;
    private TMP_Text _tmpText;

    private void Start()
    {
        _health = GetComponent<Health>();
        if (_health == null || sliderPrefab == null || target == null || hpCanvas == null) return;

        // HPBar 생성
        GameObject sliderInstance = Instantiate(sliderPrefab, hpCanvas.transform);
        RectTransform rt = sliderInstance.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;

        // 월드 좌표 → Canvas 로컬 좌표
        rt.localPosition = hpCanvas.transform.InverseTransformPoint(target.position + offset);

        _slider = sliderInstance.GetComponentInChildren<Slider>();
        _tmpText = sliderInstance.GetComponentInChildren<TMP_Text>();

        if (_slider != null)
        {
            _slider.maxValue = _health.maxHealth;
            _slider.value = _health.currentHealth;
        }

        if (_tmpText != null)
            _tmpText.text = $"{_health.currentHealth}/{_health.maxHealth}";

        _health.OnHealthChanged += UpdateBar;
    }

    private void UpdateBar(float current, float max)
    {
        if (_slider != null)
        {
            _slider.maxValue = max;
            _slider.value = current;
        }

        if (_tmpText != null)
            _tmpText.text = $"{current}/{max}";
    }

    private void OnDestroy()
    {
        if (_health != null)
            _health.OnHealthChanged -= UpdateBar;
    }
}