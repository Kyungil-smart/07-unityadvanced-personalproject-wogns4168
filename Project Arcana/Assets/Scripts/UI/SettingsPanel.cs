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

    [Header("볼륨")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        panel.SetActive(false);
        settingsButton.onClick.AddListener(() =>
        {
            panel.SetActive(true);
            string sceneName = SceneManager.GetActiveScene().name;
            saveButton.interactable = sceneName == "MapScene";

            // 슬라이더 현재 볼륨으로 초기화
            if (bgmSlider != null) bgmSlider.value = AudioManager.Instance.BGMVolume;
            if (sfxSlider != null) sfxSlider.value = AudioManager.Instance.SFXVolume;
        });

        closeButton.onClick.AddListener(() => panel.SetActive(false));
        saveButton.onClick.AddListener(OnSave);
        titleButton.onClick.AddListener(OnTitle);
        quitButton.onClick.AddListener(OnQuit);

        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGMVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }

    private void OnSave()
    {
        SaveManager.Instance.Save();
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