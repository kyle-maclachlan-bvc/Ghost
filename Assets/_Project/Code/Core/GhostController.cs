using UnityEngine;

namespace _Project.Code.Core
{
    public class GhostController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Sprite neutralSprite;
        [SerializeField] private Sprite hungrySprite;
        [SerializeField] private Sprite angrySprite;
    
        [SerializeField] private float maxOffset = 4f;
        [SerializeField] private float moveSpeed = 8f;

        private int difference;

        private Vector3 _startingPosition;
        private float targetX;

        private void Start()
        {
            _startingPosition = transform.position;
        }

        private void Update()
        {
            Vector3 targetPosition = new Vector3(targetX,  transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }

        public void UpdateGhostPosition(int player1Score, int player2Score)
        {
            difference = player1Score - player2Score;

            float offsetPerPress = 0.15f;
        
            float offset = difference * offsetPerPress;
        
            offset = Mathf.Clamp(offset, -maxOffset, maxOffset);
            targetX = _startingPosition.x + offset;
        
            UpdateGhostSprite(player1Score, player2Score);
        }

        private void UpdateGhostSprite(int player1Score, int player2Score)
        {
            difference = Mathf.Abs(player1Score - player2Score);

            if (difference >= 15)
                spriteRenderer.sprite = angrySprite;
            else if (difference >= 5)
                spriteRenderer.sprite = hungrySprite;
            else
                spriteRenderer.sprite = neutralSprite;
        }
    }
}
