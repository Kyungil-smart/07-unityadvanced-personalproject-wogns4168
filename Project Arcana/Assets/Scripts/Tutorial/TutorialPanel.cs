using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text pageText;
    [SerializeField] private List<Sprite> tutorialImages;
    [SerializeField] private Image tutorialImage;

    private List<TutorialPage> _pages;
    private int _currentPage = 0;

    private void Start()
    {
        InitPages();

        // 첫 실행 여부 확인
        if (PlayerPrefs.GetInt("TutorialShown", 0) == 0)
        {
            Show();
        }
        else
        {
            panel.SetActive(false);
        }

        nextButton.onClick.AddListener(OnNext);
        prevButton.onClick.AddListener(OnPrev);
        closeButton.onClick.AddListener(OnClose);
    }

    private void InitPages()
    {
        _pages = new List<TutorialPage>
        {
            new TutorialPage
            {
                title = "카드 사용법",
                content = "카드를 마우스로 클릭하면 선택됩니다.\n클릭된 카드의 화살표를 통해 몬스터를 클릭해서 사용하세요.\n자신에게 사용하는 카드는 클릭만 해도 됩니다."
            },
            new TutorialPage
            {
                title = "에너지",
                content = "매 턴 에너지가 충전됩니다.\n카드마다 사용에 필요한 에너지가 다릅니다.\n에너지가 부족하면 카드를 사용할 수 없습니다."
            },
            new TutorialPage
            {
                title = "턴 종료",
                content = "우측 하단의 턴 종료 버튼을 누르면\n몬스터가 행동합니다.\n손패의 카드는 버림패로 이동합니다."
            },
            new TutorialPage
            {
                title = "덱 / 버림패 / 소멸",
                content = "덱: 앞으로 드로우할 카드 묶음\n버림패: 사용하거나 버려진 카드 묶음\n소멸: 해당 전투에서 사용 불가능한 카드\n 덱의 카드를 모두 사용하면 버림패를 다시 덱에 섞어서 넣습니다."
            },
            new TutorialPage
            {
                title = "덱 보기",
                content = "상단의 책 아이콘을 누르면\n현재 보유한 덱 전체를 확인할 수 있습니다."
            },
            new TutorialPage
            {
                title = "몬스터 의도",
                content = "몬스터 위에 표시된 숫자는\n다음 턴에 가할 공격력입니다.\n상태이상 아이콘도 함께 확인하세요."
            },
            new TutorialPage
            {
                title = "환경설정",
                content = "상단 우측의 톱니바퀴 버튼을 누르면\n저장, 타이틀로 돌아가기,\n게임 종료,\n 저장은 맵화면에서만 할 수 있습니다."
            }
        };
    }

    public void Show()
    {
        _currentPage = 0;
        panel.SetActive(true);
        UpdatePage();
    }

    private void UpdatePage()
    {
        titleText.text = _pages[_currentPage].title;
        contentText.text = _pages[_currentPage].content;
        pageText.text = $"{_currentPage + 1} / {_pages.Count}";

        // 이미지 교체
        if (tutorialImage != null && _currentPage < tutorialImages.Count)
            tutorialImage.sprite = tutorialImages[_currentPage];

        prevButton.interactable = _currentPage > 0;
        nextButton.gameObject.SetActive(_currentPage < _pages.Count - 1);
        closeButton.gameObject.SetActive(_currentPage == _pages.Count - 1);
    }

    private void OnNext()
    {
        if (_currentPage < _pages.Count - 1)
        {
            _currentPage++;
            UpdatePage();
        }
    }

    private void OnPrev()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            UpdatePage();
        }
    }

    private void OnClose()
    {
        PlayerPrefs.SetInt("TutorialShown", 1);
        PlayerPrefs.Save();
        panel.SetActive(false);
    }
}
