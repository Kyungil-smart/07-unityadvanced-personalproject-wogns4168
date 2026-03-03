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
    private GameObject _sliderInstance; // 인스턴스 참조 보관

    private void Start()
    {
        _health = GetComponent<Health>();
        if (_health == null || sliderPrefab == null || target == null || hpCanvas == null) return;

        _sliderInstance = Instantiate(sliderPrefab, hpCanvas.transform);
        RectTransform rt = _sliderInstance.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;
        rt.localPosition = hpCanvas.transform.InverseTransformPoint(target.position + offset);

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
        _health.OnHealthChanged += CheckDead; // 사망 체크 추가
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
        // HP 0이 되면 HP바 삭제
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

        // 오브젝트 삭제 시 HP바도 같이 삭제
        if (_sliderInstance != null)
            Destroy(_sliderInstance);
    }
}