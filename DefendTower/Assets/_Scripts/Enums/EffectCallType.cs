/// <summary>
/// Determines to which event the effect listens 
/// </summary>
public enum EffectCallType {
    OneTimeApply,
    ChangeSpell,
    ChangeProjectile,
    ProjectileBeforeHit,
    ProjectileOnHit,
    ProjectileAfterHit,
    EnemyDeath,
    TowerDeath,
    OnDeath,
    GettingHit,
    TargetEnterRange,
    TargetLeavesRange,
    OnCast,
    SpellBeforeHit,
    SpellOnHit,
    SpellAfterHit,
}
