using UnityEngine;
/// <summary>
/// Casts the spell multiple times
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Multicast")]
public class Multicast : ScriptableAbilityEffect {
    [field: SerializeField] public EffectAmount CastAmount { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase _) {
        if (projectile.SpellNumber >= CastAmount.GetIntValue(origin)) return;
        projectile.SpellNumber++;
        ((TowerBase)origin).CheckSpellCast(true);
    }
}
