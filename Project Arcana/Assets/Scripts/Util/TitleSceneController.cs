using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "MapScene";
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button loadButton;

    private void Start()
    {
        nameInputPanel.SetActive(false);
        
        if (loadButton != null)
            loadButton.interactable = SaveManager.Instance.HasSave();
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
            playerName = "";

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
    // 로드 버튼
    public void OnLoadGame()
    {
        SaveManager.Instance.Load();
        SceneManager.LoadScene("MapScene");
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