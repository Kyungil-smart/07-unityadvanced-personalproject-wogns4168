using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
{
    [SerializeField] private Button titleButton;

    private void Start()
    {
        titleButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("TitleScene");
        });
    }
}