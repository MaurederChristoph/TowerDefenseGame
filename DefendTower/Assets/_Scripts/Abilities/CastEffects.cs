using System.Collections.Generic;

public static class EffectCaster {
    public static void CastEffect(IEnumerable<ScriptableAbilityEffect> effects, ProjectileInfo info, UnitBase origin, UnitBase target) {
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
