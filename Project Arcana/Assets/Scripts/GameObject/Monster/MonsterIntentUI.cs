using UnityEngine;
using TMPro;

public class MonsterIntentUI : MonoBehaviour
{
    [SerializeField] private GameObject intentPrefab; // 프리팹 연결
    [SerializeField] private Canvas hpCanvas;
    [SerializeField] private Vector3 offset = new Vector3(0, -2f, 0);
    [SerializeField] private Vector3 scale = new Vector3(-0.8f, -0.8f, 0);

    private MonsterBase _monster;
    private GameObject _instance;
    private RectTransform _rectTransform;

    private TMP_Text _intentText;
    private GameObject _poisonGroup;
    private TMP_Text _poisonText;
    private GameObject _weakGroup;
    private TMP_Text _weakText;
    private GameObject _breakGroup;
    private TMP_Text _breakText;

    private void Start()
    {
        _monster = GetComponent<MonsterBase>();
        
        if (hpCanvas == null)
            hpCanvas = GameObject.Find("HpCanvas")?.GetComponent<Canvas>();
        
        if (_monster == null || intentPrefab == null || hpCanvas == null) return;

        // HPBar처럼 Canvas에 자동 생성
        _instance = Instantiate(intentPrefab, hpCanvas.transform);
        _rectTransform = _instance.GetComponent<RectTransform>();
        _rectTransform.localScale = Vector3.one + scale;

        // 자식 참조
        _intentText = _instance.transform.Find("IntentGroup/IntentText")?.GetComponent<TMP_Text>();
        _poisonGroup = _instance.transform.Find("PoisonGroup")?.gameObject;
        _poisonText = _poisonGroup?.GetComponentInChildren<TMP_Text>();
        _weakGroup = _instance.transform.Find("WeakGroup")?.gameObject;
        _weakText = _weakGroup?.GetComponentInChildren<TMP_Text>();
        _breakGroup = _instance.transform.Find("BreakGroup")?.gameObject;
        _breakText = _breakGroup?.GetComponentInChildren<TMP_Text>();

        _poisonGroup?.SetActive(false);
        _weakGroup?.SetActive(false);
        _breakGroup?.SetActive(false);

        UpdateIntent();
    }

    private void LateUpdate()
    {
        if (_instance == null || _monster == null) return;
        _rectTransform.localPosition = hpCanvas.transform.InverseTransformPoint(_monster.transform.position + offset);
    }

    public void UpdateIntent()
    {
        if (_monster == null || _instance == null) return;

        if (_intentText != null)
            _intentText.text = $"{_monster.IntentDamage}";

        UpdateStatus("Poison", _poisonGroup, _poisonText);
        UpdateStatus("Weak", _weakGroup, _weakText);
        UpdateStatus("Break", _breakGroup, _breakText);
    }

    private void UpdateStatus(string statusName, GameObject group, TMP_Text text)
    {
        if (group == null || text == null) return;

        int stack = _monster.StatusManager.GetStack(statusName);
        group.SetActive(stack > 0);
        if (stack > 0) text.text = stack.ToString();
    }

    private void OnDestroy()
    {
        if (_instance != null)
            Destroy(_instance);
    }
    
    public void DestroyUI()
    {
        if (_instance != null)
        {
            Destroy(_instance);
            _instance = null;
        }
    }
}