using UnityEngine;

/// <summary>
/// Changes the shooting behaviour of the spell
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change Shooting Type", fileName = "Change Shooting Type")]
public class ChangeShootingType : ScriptableAbilityEffect {
    [field: SerializeField] public ShootingType ShootingType { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(target is TowerBase tower) {
            tower.ChangeShootingType(ShootingType);
        }
    }
}
