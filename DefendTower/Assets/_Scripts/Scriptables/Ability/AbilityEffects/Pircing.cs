using UnityEngine;
/// <summary>
/// The amount of targets the projectile can hit before being destroyed
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Piercing",fileName = "Piercing")]
public class Piercing : ScriptableAbilityEffect {
    
    [Tooltip("Increases the amount of targets a projectile can hit")]
    [field: SerializeField] public EffectAmount PircingAmount { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        var value = PircingAmount.GetIntValue(origin);
        projectile.PiercingTargets =+ value;
    }
}
