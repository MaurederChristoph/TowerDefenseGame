using UnityEngine;
/// <summary>
/// Changes the attack speed of the target
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change AttackSpeed Multiplier", fileName = "ChangeAttackSpeedMultiplier")]
public class ChangeAttackSpeedMultiplier : ScriptableAbilityEffect {

    [Tooltip("The amount by which the attack speed will be changed")]
    [field: SerializeField] public EffectAmount AttackSpeedChange { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        target.ChangeAttackSpeedMultiplier(AttackSpeedChange.GetFloatValue(origin));
    }
}
