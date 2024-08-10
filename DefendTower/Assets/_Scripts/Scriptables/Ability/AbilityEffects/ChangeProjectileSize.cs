using UnityEngine;

/// <summary>
/// Changes the size of the spell
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change Size", fileName = "Change Size")]
public class ChangeProjectileSize : ScriptableAbilityEffect {

    [Tooltip("The amount by which the spell size will be changed")]
    [field: SerializeField] public EffectAmount SpellSizeChange { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(target is TowerBase tower) {
            tower.ChangeSpellSize(SpellSizeChange.GetFloatValue(origin));
        }
    }
}
