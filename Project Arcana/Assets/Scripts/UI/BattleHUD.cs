using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [Header("Deck Info")]
    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private TMP_Text discardCountText;
    [SerializeField] private TMP_Text exhaustCountText;

    [Header("Energy")]
    [SerializeField] private TMP_Text energyText;

    [Header("Turn")]
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TMP_Text endTurnButtonText;

    public void UpdateDeckInfo(int draw, int discard, int exhaust)
    {
        deckCountText.text = $"{draw}";
        discardCountText.text = $"{discard}";
        exhaustCountText.text = $"{exhaust}";
    }

    public void UpdateEnergy(int current, int max)
    {
        energyText.text = $"{current}/{max}";
    }

    public void SetEndTurnInteractable(bool interactable)
    {
        endTurnButton.interactable = interactable;
    }

    public void SetPlayerTurn()
    {
        endTurnButtonText.text = "턴 종료";
        SetEndTurnInteractable(true);
    }

    public void SetEnemyTurn()
    {
        endTurnButtonText.text = "상대 턴";
        SetEndTurnInteractable(false);
    }

    public void SetEndTurnCallback(System.Action callback)
    {
        endTurnButton.onClick.RemoveAllListeners();
        endTurnButton.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlayButtonPressSFX(); // 추가

            foreach (var dragArrow in FindObjectsByType<CardDragArrow>(FindObjectsSortMode.None))
            {
                if (dragArrow.CardView.IsSelected)
                {
                    dragArrow.Deselect();
                    break;
                }
            }
            callback();
        });
    }
}