using UnityEngine;

public interface IProjectileSpell
{
    public (Vector3 position, Quaternion rotation) GetLaunchPlatform();
    public void ApplyEnemy(Enemy enemy, Projectile projectile);
}