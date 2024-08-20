using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Applies a number off effect to all targets in range
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/EffectInRange", fileName = "Effect In Range")]
public class EffectInRange : ScriptableAbilityEffect {
    [field: SerializeField] public float Radius { get; private set; }
    [field: SerializeField] public Faction TargetFaction { get; private set; }
    [field: SerializeField] public List<ScriptableAbilityEffect> Effects { get; private set; }

    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(target == null || origin == null) return;
        List<UnitBase> targets = new();
        targets = GameManager.Instance.UnitManager.GetTargetsWithinRange(TargetFaction == origin.Faction ? origin : target,
            TargetFaction, Radius);
        targets.Remove(target);
        targets.Remove(origin);
        foreach(var t in targets) {
            if(t is null) continue;
            EffectCaster.CastEffect(Effects, projectile, origin, t);
        }
    }
}
