using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "MapScene";

    public void OnGameStart()
    {
        // 1. 새로운 런 시작 (덱, 골드 초기화)
        if (RunManager.Instance != null)
        {
            RunManager.Instance.StartNewRun();
        }

        // 2. 새로운 맵 생성
        if (MapManager.Instance != null)
        {
            MapManager.Instance.GenerateMap();
        }

        // 3. 맵 씬으로 이동
        SceneManager.LoadScene(mapSceneName);
    }

    public void OnExitGame()
    {
        // 유니티 에디터에서 실행 중일 때
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // 실제 빌드된 게임(PC, 모바일 등)에서 실행 중일 때
                Application.Quit();
        #endif
    }
}