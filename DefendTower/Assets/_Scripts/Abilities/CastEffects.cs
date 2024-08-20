using System.Collections.Generic;

/// <summary>
/// Distributes the effected based on <see cref="TargetingType"/> of the given effect
/// </summary>
public static class EffectCaster {

    /// <summary>
    /// Calls ApplyEffect for the given effect 
    /// </summary>
    /// <param name="effects">The effects that will be applied</param>
    /// <param name="info">Info of the projectile that changes are applied to if an effect changes it</param>
    /// <param name="origin">The origin of the effect</param>
    /// <param name="target">The target of the effect</param>
    public static void CastEffect(IEnumerable<ScriptableAbilityEffect> effects, ProjectileInfo info, UnitBase origin,
        UnitBase target) {
        foreach(var effect in effects) {
            switch(effect.TargetingType) {
                case TargetingType.Target:
                    effect.ApplyEffect(info, origin, target);
                    break;
                case TargetingType.Self:
                    effect.ApplyEffect(info, origin, origin);
                    break;
            }
        }
    }
}
