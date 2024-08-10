using UnityEngine;
/// <summary>
/// Changes the Health of the target unit
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change Health", fileName = "Change Health")]
public class ChangeHealth : ScriptableAbilityEffect {
    [Tooltip("The amount the health will be changed")]
    [field: SerializeField] public EffectAmount HealthChange { get; private set; }
    [Tooltip("The type of heath change the unit will receive")]
    [field: SerializeField] public HealthEffectType HealthChangeType { get; private set; }


    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        var t = HealthChange.GetIntValue(origin);
        var value = CalculateHealthChange(t, HealthChangeType);
        target.ChangeHealth(value + projectile.DamageBonus);
    }
}
