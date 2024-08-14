using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// Slows down the unit when entering the range of the unit
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/DamageReduction", fileName = "Damage Reduction")]
public class DamageReduction : ScriptableAbilityEffect {
    [field: SerializeField] public EffectAmount ReductionAmount { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        target.BaseProjectile.DamageBonus -= ReductionAmount.GetIntValue(origin);
    }
}
