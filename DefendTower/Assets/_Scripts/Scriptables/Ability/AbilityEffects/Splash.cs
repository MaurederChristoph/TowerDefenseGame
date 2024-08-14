using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Applies all before hit and on hit effects of the projectile to targets in range
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Splash", fileName = "Splash")]
public class Splash : ScriptableAbilityEffect {
	[field: SerializeField] public float Radius { get; private set; }
	public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
		List<UnitBase> targets = GameManager.Instance.UnitManager.GetTargetsWithinRange(projectile.Target, target.Faction, Radius);
		Debug.Log(targets.Count);
		targets.Remove(target);
		foreach(var t in targets) {
			foreach(var effect in projectile.Effects.Where(e =>
				        e.CallType is
					        EffectCallType.ProjectileOnHit or
					        EffectCallType.ProjectileBeforeHit or
					        EffectCallType.SpellOnHit or
					        EffectCallType.SpellBeforeHit
			        )) {
				effect.ApplyEffect(projectile, origin, t);
			}
		}
	}
}
