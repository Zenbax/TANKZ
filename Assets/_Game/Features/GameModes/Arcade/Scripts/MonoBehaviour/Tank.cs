namespace _Game.Features.GameModes.Test.Scripts.Tank.MonoBehaviour
{
    using UnityEngine;

    public class Tank : MonoBehaviour, IDamageable
    {
        [Header("Tank Settings")]
        public float maxHealth = 100f;
        private float currentHealth;
        private int playerNumber;

        private GameManager gameManager;

        public void Init(GameManager gm, int playerNum)
        {
            gameManager = gm;
            playerNumber = playerNum;
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            Debug.Log("dt: " + amount);
            currentHealth -= amount;
            gameManager.UpdateHealth(playerNumber, Mathf.Max(currentHealth, 0));

            if (currentHealth <= 0)
            {
                gameManager.OnPlayerDeath(playerNumber);
                Destroy(gameObject);
            }
        }
    }
}