using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Ui References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text player1Text;
    [SerializeField] private TMP_Text player2Text;
    [SerializeField] private TMP_Text resultText;

    private void Start()
    {
        resultText.text = "";
    }

    public void UpdateTimer(float timeRemaining)
    {
        timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
    }

    public void UpdateScore(int player1Score, int player2Score)
    {
        Debug.Log($"Updating Scores: P1={player1Score}, P2={player2Score}");
        
        player1Text.text = $"Player 1: {player1Score}";
        player2Text.text = $"Player 2: {player2Score}";
    }

    public void ShowResult(string message)
    {
        resultText.text = message;
    }

}
