using UnityEngine;

/// <summary>
/// Info of current data of instantiated projectile
/// </summary>
public class ProjectileInfo {
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectileInfo"/> class.
    /// </summary>
    /// <param name="projectilePrefab">The prefab of the projectile.</param>
    /// <param name="shootingType">The type of shooting associated with the projectile.</param>
    /// <param name="speed">The speed of the projectile.</param>
    /// <param name="speedCurve">The animation curve for the projectile's speed.</param>
    public ProjectileInfo(GameObject projectilePrefab, ShootingType shootingType, float speed, AnimationCurve speedCurve) {
        ProjectilePrefab = projectilePrefab;
        ShootingType = shootingType;
        Speed = speed;
        SpeedCurve = speedCurve;
    }

    /// <summary>
    /// Projectile object that will be spawned
    /// </summary>
    public GameObject ProjectilePrefab { get; private set; }

    /// <summary>
    /// Speed of the projectile.
    /// </summary>
    public float Speed { get; private set; }

    /// <summary>
    /// Type of shooting associated with the projectile.
    /// </summary>
    public ShootingType ShootingType { get; private set; }

    /// <summary>
    /// Animation curve for the projectile's speed.
    /// </summary>
    public AnimationCurve SpeedCurve { get; private set; }

    /// <summary>
    /// Handles the projectile when it reaches its destination
    /// </summary>
    /// <param name="shotProjectile">The instance of the projectile</param>
    public void ReachedDestination(GameObject shotProjectile) {
        GameObject.Destroy(shotProjectile);
    }
}