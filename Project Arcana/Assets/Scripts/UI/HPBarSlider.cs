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
    private PlayerHealth _playerHealth;
    private Slider _slider;
    private TMP_Text _tmpText;
    private GameObject _sliderInstance;
    private RectTransform _sliderRect;

    private GameObject _shieldGroup;
    private TMP_Text _shieldText;

    private void Start()
    {
        _health = GetComponent<Health>();
        _playerHealth = GetComponent<PlayerHealth>();
        
        // hpCanvas 자동 찾기
        if (hpCanvas == null)
            hpCanvas = GameObject.Find("HpCanvas")?.GetComponent<Canvas>();

        if (_health == null || sliderPrefab == null || target == null || hpCanvas == null) return;

        _sliderInstance = Instantiate(sliderPrefab, hpCanvas.transform);
        _sliderRect = _sliderInstance.GetComponent<RectTransform>();
        _sliderRect.localScale = Vector3.one;
        _sliderRect.localPosition = hpCanvas.transform.InverseTransformPoint(target.position + offset);

        _slider = _sliderInstance.GetComponentInChildren<Slider>();
        _tmpText = _sliderInstance.GetComponentInChildren<TMP_Text>();

        // ShieldGroup 찾기
        Transform shieldTransform = _sliderInstance.transform.Find("ShieldGroup");
        if (shieldTransform != null)
        {
            _shieldGroup = shieldTransform.gameObject;
            _shieldText = _shieldGroup.GetComponentInChildren<TMP_Text>();
            _shieldGroup.SetActive(false);
        }

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

        if (_playerHealth != null)
            _playerHealth.OnShieldChanged += UpdateShieldUI;
    }

    private void LateUpdate()
    {
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

    private void UpdateShieldUI(float shieldAmount)
    {
        if (_shieldGroup == null || _shieldText == null) return;

        if (shieldAmount > 0)
        {
            _shieldGroup.SetActive(true);
            _shieldText.text = shieldAmount.ToString();
        }
        else
        {
            _shieldGroup.SetActive(false);
        }
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
        if (_playerHealth != null)
            _playerHealth.OnShieldChanged -= UpdateShieldUI;

        if (_sliderInstance != null)
            Destroy(_sliderInstance);
    }
}