using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text continueButtonText;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowVictory(System.Action onContinue)
    {
        panel.SetActive(true);
        resultText.text = "Victory!";
        continueButtonText.text = "계속하기";
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => onContinue?.Invoke());
    }

    public void ShowDefeat(System.Action onContinue)
    {
        panel.SetActive(true);
        resultText.text = "Defeat...";
        continueButtonText.text = "처음으로";
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => onContinue?.Invoke());
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}