using UnityEngine;

public class DestructibleWall : MonoBehaviour, IDamageable
{
    public float health = 50f;
    public GameObject destructionEffect; // Particle effect prefab

    public void TakeDamage(float amount)
    {
        Debug.Log("HIT");
        health -= amount;
        if (health <= 0)
        {
            DestroyWall();
        }
    }

    private void DestroyWall()
    {
        if (destructionEffect != null)
        {
            Debug.Log("HIT-DD");
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }
        Debug.Log("HIT-DEAD");
        Debug.Log("HIT-helth: " + health);
        Destroy(transform.root.gameObject);
    }
}