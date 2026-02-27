using UnityEngine;
    public class Arrow : MonoBehaviour
    {
        // 화살표 시작점(카드 위치)
        public Transform Origin { get { return origin; } set { origin = value; } }

        [SerializeField] private float baseHeight = 50f;        // 화살표 기본 길이
        [SerializeField] private RectTransform baseRect;        // 화살표 이미지 RectTransform
        [SerializeField] private Transform origin;             // 시작 위치
        [SerializeField] private bool startsActive = false;    // 처음 활성화 여부

        private RectTransform myRect;
        private Canvas canvas;
        private Camera mainCamera;
        private bool isActive;

        private void Awake()
        {
            myRect = (RectTransform)transform;
            canvas = GetComponentInParent<Canvas>();
            mainCamera = Camera.main;
            SetActive(startsActive); // 초기 활성화 상태
        }

        private void Update()
        {
            if (!isActive || origin == null)
                return;

            Setup(); // 화살표 위치, 회전, 길이 갱신
        }

        // 화살표 위치, 회전, 길이 설정
        private void Setup()
        {
            // origin 월드 좌표 → 스크린 좌표
            Vector2 originPosOnScreen = mainCamera.WorldToScreenPoint(origin.position);

            // Canvas 좌표로 변환
            myRect.anchoredPosition = new Vector2(
                originPosOnScreen.x - Screen.width / 2,
                originPosOnScreen.y - Screen.height / 2
            ) / canvas.scaleFactor;

            // 마우스와 origin 벡터 계산
            Vector2 differenceToMouse = (Vector2)Input.mousePosition - originPosOnScreen;
            differenceToMouse.Scale(new Vector2(1f / myRect.localScale.x, 1f / myRect.localScale.y));

            // 화살표 회전
            transform.up = differenceToMouse;

            // 화살표 길이 조정
            baseRect.anchorMax = new Vector2(
                baseRect.anchorMax.x,
                differenceToMouse.magnitude / canvas.scaleFactor / baseHeight
            );
        }

        // 화살표 활성화 / 비활성화
        private void SetActive(bool b)
        {
            isActive = b;
            baseRect.gameObject.SetActive(b);
        }

        public void Activate() => SetActive(true);
        public void Deactivate() => SetActive(false);

        // origin 지정 후 활성화
        public void SetupAndActivate(Transform origin)
        {
            Origin = origin;
            Activate();
        }
    }