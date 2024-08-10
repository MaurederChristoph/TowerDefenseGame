using System.Linq;
using UnityEngine;

/// <summary>
/// Deals extra damage every X hits
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Damage after X hits amounts", fileName = "Damage After X Hits Amount")]
public class DamageAfterXHitsAmount : ScriptableAbilityEffect {

    [Tooltip("The damage the unit will receive after X hits")]
    [field: SerializeField] public EffectAmount Damage { get; private set; }
    [Tooltip("After how many hit the target will get damage")]
    [field: SerializeField] private int _damageAfterHits;
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(projectile.AttackAgainstTargets % _damageAfterHits != 0) {
            return;
        }
        var healthChange = projectile.Effects.FirstOrDefault(e => e is ChangeHealth) as ChangeHealth;
        if(healthChange == null) { return; }
        var value = CalculateHealthChange(Mathf.RoundToInt(Damage.GetIntValue(origin)), healthChange.HealthChangeType);
        target.ChangeHealth(value);
    }
}
