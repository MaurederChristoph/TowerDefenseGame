using System;
using System.Collections.Generic;
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
		IEnumerable<ScriptableAbilityEffect> healthChanges = projectile.Effects.Where(e => e is ChangeHealth);
		ChangeHealth healthChange = null;
		var value = 0;
		foreach(var scriptableAbilityEffect in healthChanges) {
			healthChange = (ChangeHealth)scriptableAbilityEffect;
			value += CalculateHealthChange(healthChange.HealthChange.GetIntValue(origin), healthChange.HealthChangeType);
		}
		if (healthChange == null) { return; }
		switch(ModifierType) {
			case HealthEffectModifier.Below:
				if (target.CurrentHealth < target.MaxHealth * (Percentage / 100)) {
					target.ChangeHealth(Mathf.RoundToInt(value * HealthChangeMultiplier.GetFloatValue(origin)),
						origin);
				}
				break;
			case HealthEffectModifier.Above:
				if (target.CurrentHealth > target.MaxHealth * (Percentage / 100)) {
					target.ChangeHealth(Mathf.RoundToInt(value * HealthChangeMultiplier.GetFloatValue(origin)),
						origin);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

	}
}
