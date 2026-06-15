using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 120f;
    [SerializeField] private int targetPresses = 100;

    [SerializeField] private UIManager uiManager;
    
    private float currentTime;

    private int player1Presses;
    private int player2Presses;

    private bool gameEnded;

    private void Start()
    {
        currentTime  = gameDuration;

        uiManager.UpdateTimer(currentTime);
        uiManager.UpdateScore(player1Presses, player2Presses);
        
    }

    private void Update()
    {
        if (gameEnded)
            return;
        
        currentTime -= Time.deltaTime;

        uiManager.UpdateTimer(currentTime);

        if (currentTime <= 0)
            BothPlayersLose();
    }

    public void RegisterPress(int playerID)
    {
        Debug.Log($"RegisterPress called by Player {playerID}");
        
        if (gameEnded)
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
        
        uiManager.UpdateScore(player1Presses, player2Presses);
    }

    private void PlayerWins(int winnerID)
    {
        gameEnded = true;

        int loserID = winnerID == 1 ? 2 : 1;

        uiManager.ShowResult($"PLAYER {winnerID} exorcized the Ghost!\nPLAYER {loserID} is Haunted!");
    }

    private void BothPlayersLose()
    {
        gameEnded = true;
        
        uiManager.ShowResult("Time's Up!\nThe Ghost Remains");
    }
}
