using UnityEngine;

public class ShootingTest : MonoBehaviour{
    public GameObject ProjectilePrefab;
    public float Speed;
    public ShootingType ShootingType;
    public AnimationCurve SpeedCurve;

    public Transform start, end;

    
    public ShootingBehavior shoot;
    private ProjectileInfo projectile;
    private void Start() {
        projectile = new ProjectileInfo(ProjectilePrefab, ShootingType, Speed, SpeedCurve);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            shoot.Shoot(start.position,end.position, projectile);
        }
    }
}
