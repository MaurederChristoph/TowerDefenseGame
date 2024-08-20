using UnityEngine;

/// <summary>
/// Changes the regeneration of a tower
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/RemoveHpCap", fileName = "RemoveHpCap")]
public class ChangeHeathReg : ScriptableAbilityEffect {
    [field: SerializeField] private EffectAmount _amount;
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        ((TowerBase)target).ChangeHeathRegModifier(_amount.GetFloatValue(origin));
    }
}
