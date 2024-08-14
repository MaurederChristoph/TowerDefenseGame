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
		var value = CalculateHealthChange(HealthChange.GetIntValue(origin), HealthChangeType);
		var healthChange = value < 0 ? Mathf.Min(value - projectile.DamageBonus, 0) : Mathf.Max(value - projectile.DamageBonus, 0);
		target.ChangeHealth(healthChange, origin);
	}
}
