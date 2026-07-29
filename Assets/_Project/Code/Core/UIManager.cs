using TMPro;
using UnityEngine;

namespace _Project.Code.Core
{
    public class UIManager : MonoBehaviour
    {
        [Header("Screens / Panels")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private GameObject hud; // the gameplay HUD container (timer + scores)

        [Header("HUD References")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text player1Text;
        [SerializeField] private TMP_Text player2Text;

        [Header("Game Over")]
        [SerializeField] private TMP_Text resultText; // lives on the game over screen

        // ---- Screen switching ----

        public void ShowStartScreen()
        {
            SetActive(startScreen, true);
            SetActive(pauseScreen, false);
            SetActive(gameOverScreen, false);
            SetActive(hud, false);
        }

        public void ShowGameplay()
        {
            SetActive(startScreen, false);
            SetActive(pauseScreen, false);
            SetActive(gameOverScreen, false);
            SetActive(hud, true);
        }

        public void ShowPauseScreen()
        {
            // Overlay on top of the gameplay HUD.
            SetActive(pauseScreen, true);
        }

        public void ShowGameOverScreen(string message)
        {
            if (resultText != null)
                resultText.text = message;

            SetActive(hud, false);
            SetActive(gameOverScreen, true);
        }

        // ---- HUD updates ----

        public void UpdateTimer(float timeRemaining)
        {
            if (timerText != null)
                timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
        }

        public void UpdateScore(int player1Score, int player2Score)
        {
            if (player1Text != null)
                player1Text.text = $"Player 1: {player1Score}";

            if (player2Text != null)
                player2Text.text = $"Player 2: {player2Score}";
        }

        private static void SetActive(GameObject go, bool value)
        {
            if (go != null)
                go.SetActive(value);
        }
    }
}
