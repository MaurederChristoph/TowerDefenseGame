using UnityEngine;

public class ProjectileInfo {
    public ProjectileInfo(GameObject projectilePrefab, ShootingType shootingType, float speed, AnimationCurve speedCurve) {
        ProjectilePrefab = projectilePrefab;
        ShootingType = shootingType;
        Speed = speed;
        SpeedCurve = speedCurve;
    }

    public GameObject ProjectilePrefab { get; private set; }
    public float Speed { get; private set; }
	public ShootingType ShootingType { get; private set; }
	public AnimationCurve SpeedCurve { get; private set; }

    public void ReachedDestination(GameObject shotProjectile) {
        GameObject.Destroy(shotProjectile);
    }
}
