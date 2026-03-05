using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        panel.SetActive(false);
        settingsButton.onClick.AddListener(() =>
        {
            panel.SetActive(true);
            // 맵씬에서만 저장 버튼 활성화
            string sceneName = SceneManager.GetActiveScene().name;
            saveButton.interactable = sceneName == "MapScene";
        });
    
        closeButton.onClick.AddListener(() => panel.SetActive(false));
        saveButton.onClick.AddListener(OnSave);
        titleButton.onClick.AddListener(OnTitle);
        quitButton.onClick.AddListener(OnQuit);
    }

    private void OnSave()
    {
        SaveManager.Instance.Save();
        // 저장 완료 피드백
        Debug.Log("저장 완료!");
    }

    private void OnTitle()
    {
        RunManager.Instance.StartNewRun();
        SceneManager.LoadScene("TitleScene");
    }

    private void OnQuit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
}