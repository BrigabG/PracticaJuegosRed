using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private CanvasGroup winnerPanel;
    [SerializeField] private TextMeshProUGUI winnerText;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        HideWinner();
    }

    public void ShowWinner(string winnerName)
    {
        winnerText.text = $"{winnerName} wins!";
        winnerPanel.alpha = 1;
        winnerPanel.interactable = true;
        winnerPanel.blocksRaycasts = true;
    }

    public void HideWinner()
    {
        winnerPanel.alpha = 0;
        winnerPanel.interactable = false;
        winnerPanel.blocksRaycasts = false;
    }
}