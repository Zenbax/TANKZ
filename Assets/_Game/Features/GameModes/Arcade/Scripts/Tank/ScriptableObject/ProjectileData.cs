using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Game/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public GameObject prefab;
    public float damage;
    public float speed;
    public float lifetime;
    public bool explosive;
    [Header("Visuals")]
    public float projectileScale = 0.2f;
}