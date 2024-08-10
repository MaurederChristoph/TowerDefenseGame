using System;
using UnityEngine;
/// <summary>
/// Changes the burst behaviour of a unit
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change Burst Behaviour", fileName = "Change Burst Behaviour")]
public class ChangeBurstBehaviour : ScriptableAbilityEffect {
    [Tooltip("The amount of arrows in a burst volley")]
    [field: SerializeField] public EffectAmount Amount { get; protected set; }
    [Tooltip("The amount that will be added to the reload multiplier")]
    [field: SerializeField] public EffectAmount ReloadMultiplier { get; protected set; }
    [Tooltip("The amount that will be added to the in between shot multiplier")]
    [field: SerializeField] public EffectAmount InBetweenShotMultiplier { get; protected set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        target.ChangeBurstAmount(Amount.GetIntValue(origin) - 1);
        target.ChangeBurstReloadSpeed(ReloadMultiplier.GetFloatValue(origin));
        target.ChangeBurstInBetweenShotsMultiplier(InBetweenShotMultiplier.GetFloatValue(origin));
    }
}
