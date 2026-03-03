using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarSlider : MonoBehaviour
{
    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private Transform target;
    [SerializeField] private Canvas hpCanvas;
    [SerializeField] private Vector3 offset = new Vector3(0, -0.3f, 0);

    private Health _health;
    private Slider _slider;
    private TMP_Text _tmpText;
    private GameObject _sliderInstance;
    private RectTransform _sliderRect; // 추가

    private void Start()
    {
        _health = GetComponent<Health>();
        if (_health == null || sliderPrefab == null || target == null || hpCanvas == null) return;

        _sliderInstance = Instantiate(sliderPrefab, hpCanvas.transform);
        _sliderRect = _sliderInstance.GetComponent<RectTransform>(); // 추가
        _sliderRect.localScale = Vector3.one;
        _sliderRect.localPosition = hpCanvas.transform.InverseTransformPoint(target.position + offset); // 원래 방식 유지

        _slider = _sliderInstance.GetComponentInChildren<Slider>();
        _tmpText = _sliderInstance.GetComponentInChildren<TMP_Text>();

        if (_slider != null)
        {
            _slider.interactable = false;
            _slider.maxValue = _health.maxHealth;
            _slider.value = _health.currentHealth;
        }

        if (_tmpText != null)
            _tmpText.text = $"{_health.currentHealth}/{_health.maxHealth}";

        _health.OnHealthChanged += UpdateBar;
        _health.OnHealthChanged += CheckDead;
    }

    private void LateUpdate()
    {
        // 매 프레임 위치 갱신 (원래 방식 그대로)
        if (_sliderInstance == null || target == null) return;
        _sliderRect.localPosition = hpCanvas.transform.InverseTransformPoint(target.position + offset);
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

    private void CheckDead(float current, float max)
    {
        if (current <= 0 && _sliderInstance != null)
        {
            Destroy(_sliderInstance);
            _sliderInstance = null;
        }
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnHealthChanged -= UpdateBar;
            _health.OnHealthChanged -= CheckDead;
        }
        if (_sliderInstance != null)
            Destroy(_sliderInstance);
    }
}