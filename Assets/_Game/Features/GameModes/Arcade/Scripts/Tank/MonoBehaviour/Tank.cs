namespace _Game.Features.GameModes.Test.Scripts.Tank.MonoBehaviour
{
    using UnityEngine;

    public class Tank : MonoBehaviour, IDamageable
    {
        [Header("Tank Settings")]
        public float maxHealth = 100f;
        private float currentHealth;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
            {
                DestroyTank();
            }
        }

        private void DestroyTank()
        {
            Debug.Log("Tank destroyed!");
            Destroy(gameObject);
        }
    }
}