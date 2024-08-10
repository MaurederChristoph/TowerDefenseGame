using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Adds a health changes a unit based on the <see cref="ChangeHealth"/>
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Health Modifier", fileName = "Health Modifier")]
public class HealthModifier : ScriptableAbilityEffect {
    [Tooltip("The amount the health will be multiplied with")]
    [field: SerializeField] public EffectAmount HealthChangeMultiplier { get; private set; }

    [Tooltip("Changes the health change effect")]
    [field: SerializeField] public HealthEffectModifier ModifierType { get; private set; }
    [Tooltip("The percentage that needs to be meet for this effect to work")]
    [field: SerializeField] public float Percentage { get; private set; }


    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        var healthChange = projectile.Effects.FirstOrDefault(e => e is ChangeHealth) as ChangeHealth;
        if(healthChange == null) { return; }
        var loseHealth = healthChange.HealthChangeType == HealthEffectType.Damage ? -1 : 1;
        var value = CalculateHealthChange(healthChange.HealthChange.GetIntValue(origin), healthChange.HealthChangeType);
        switch(ModifierType) {
            case HealthEffectModifier.Below:
                if(target.CurrentHealth < target.MaxHealth * Percentage) {
                    target.ChangeHealth(Mathf.RoundToInt(value * HealthChangeMultiplier.GetFloatValue(origin) * loseHealth));
                }
                break;
            case HealthEffectModifier.Above:
                if(target.CurrentHealth > target.MaxHealth * Percentage) {
                    target.ChangeHealth(Mathf.RoundToInt(value * HealthChangeMultiplier.GetFloatValue(origin) * loseHealth));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
