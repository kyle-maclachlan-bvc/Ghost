using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace _Project.Code.Core
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState { Start, Playing, Paused, GameOver }

        [Header("Game Settings")]
        [SerializeField] private float gameDuration = 120f;
        [SerializeField] private int targetPresses = 100;

        [SerializeField] private GhostController ghostController;

        [SerializeField] private UIManager uiManager;

        private float currentTime;

        private int player1Presses;
        private int player2Presses;

        public GameState State { get; private set; }

        private void Start()
        {
            // Begin on the title screen. Nothing runs until the player presses Start.
            Time.timeScale = 1f;
            State = GameState.Start;
            uiManager.ShowStartScreen();
        }

        private void Update()
        {
            // Esc toggles pause, but only while a round is actually running.
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (State == GameState.Playing)
                    PauseGame();
                else if (State == GameState.Paused)
                    ResumeGame();
            }

            if (State != GameState.Playing)
                return;

            currentTime -= Time.deltaTime;

            uiManager.UpdateTimer(currentTime);

            if (currentTime <= 0)
                BothPlayersLose();
        }

        // ---- Button-facing state transitions ----

        // Hook to the Start button (OnClick).
        public void StartGame()
        {
            player1Presses = 0;
            player2Presses = 0;
            currentTime = gameDuration;

            Time.timeScale = 1f;
            State = GameState.Playing;

            uiManager.ShowGameplay();
            uiManager.UpdateTimer(currentTime);
            uiManager.UpdateScore(player1Presses, player2Presses);
        }

        // Triggered by Esc.
        public void PauseGame()
        {
            State = GameState.Paused;
            Time.timeScale = 0f;
            uiManager.ShowPauseScreen();
        }

        // Hook to the Pause-screen Resume button (also triggered by Esc).
        public void ResumeGame()
        {
            State = GameState.Playing;
            Time.timeScale = 1f;
            uiManager.ShowGameplay();
        }

        // Hook to the Restart buttons (pause screen + game over screen).
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Hook to the Quit buttons.
        public void QuitGame()
        {
            Debug.Log("Quit pressed");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // ---- Gameplay ----

        public void RegisterPress(int playerID)
        {
            if (State != GameState.Playing)
                return;

            if (playerID == 1)
            {
                player1Presses++;

                if (player1Presses >= targetPresses)
                    PlayerWins(1);
            }
            else if (playerID == 2)
            {
                player2Presses++;

                if (player2Presses >= targetPresses)
                    PlayerWins(2);
            }

            ghostController.UpdateGhostPosition(player1Presses, player2Presses);

            uiManager.UpdateScore(player1Presses, player2Presses);
        }

        private void PlayerWins(int winnerID)
        {
            int loserID = winnerID == 1 ? 2 : 1;

            EndGame($"PLAYER {winnerID} exorcized the Ghost!\nPLAYER {loserID} is Haunted!");
        }

        private void BothPlayersLose()
        {
            EndGame("Time's Up!\nThe Ghost Remains");
        }

        private void EndGame(string message)
        {
            State = GameState.GameOver;
            uiManager.ShowGameOverScreen(message);
        }
    }
}
