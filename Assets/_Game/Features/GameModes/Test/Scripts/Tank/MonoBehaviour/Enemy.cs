using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Destroy(gameObject);
    }
}