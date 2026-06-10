using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMasher : MonoBehaviour
{
    [SerializeField] private int playerID;
    [SerializeField] private Key mashButton;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        if (Keyboard.current[mashButton].wasPressedThisFrame)
        {
            //Debug.Log($"Player {playerID} pressed {mashButton}");
            
            gameManager.RegisterPress(playerID);
        }
    }
}
