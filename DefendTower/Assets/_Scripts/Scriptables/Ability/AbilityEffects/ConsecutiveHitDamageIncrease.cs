using UnityEngine;
/// <summary>
/// Increases the damage with every consecutive hit on a target
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Consecutive Hit Damage Increase", fileName = "ConsecutiveHitDamageIncrease")]
public class ConsecutiveHitDamageIncrease : ScriptableAbilityEffect {

    [Tooltip("The amount of damage the unit will receive extra every turn")]
    [field: SerializeField] public EffectAmount Damage { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        projectile.DamageBonus += Damage.GetIntValue(origin);
        projectile.AddNewTargetListener(RemoveDamageBonus);
        return;

        void RemoveDamageBonus() {
            projectile.DamageBonus -= Damage.GetIntValue(origin);
            projectile.RemoveNewTargetListener(RemoveDamageBonus);
        }
    }
}
