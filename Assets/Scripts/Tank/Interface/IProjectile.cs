using UnityEngine;

public interface IProjectile
{
    void Launch(ProjectileData data, Vector3 direction);
}