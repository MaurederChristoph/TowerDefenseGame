using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// Changes the targeting of the spell
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change Targeting", fileName = "Change Targeting")]
public class ChangeTargeting : ScriptableAbilityEffect {
    [field: SerializeField] public Faction Faction { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(target is TowerBase tower) {
            tower.ChangeSpellTargeting(Faction);
        }
    }
}
