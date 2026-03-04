using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "MapScene";
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private TMP_InputField nameInputField;

    private void Start()
    {
        nameInputPanel.SetActive(false);
    }

    // Game Start 버튼
    public void OnGameStart()
    {
        nameInputPanel.SetActive(true);
    }

    // 확인 버튼
    public void OnConfirm()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
            playerName = "모험가";

        RunManager.Instance.SetPlayerName(playerName);
        RunManager.Instance.StartNewRun();
        MapManager.Instance.GenerateMap();
        SceneManager.LoadScene(mapSceneName);
    }

    // 취소 버튼
    public void OnCancel()
    {
        nameInputPanel.SetActive(false);
    }

    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}