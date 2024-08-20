using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Info of current data of instantiated projectile
/// </summary>
public class ProjectileInfo {
    /// <summary>
    /// All effect that will be triggered the moment before the on hit effect will be applied
    /// </summary>
    private List<ScriptableAbilityEffect> OnBeforeHitEffect { get; set; } = new();

    /// <summary>
    /// All effect that will be triggered the moment a unit has been shot
    /// </summary>
    private List<ScriptableAbilityEffect> OnHitEffect { get; set; } = new();
    /// <summary>
    /// all effects that will be triggered the moment after the on hit effect have been applied
    /// </summary>
    private List<ScriptableAbilityEffect> OnAfterHitEffect { get; set; } = new();

    /// <summary>
    /// Will be triggered when a unit is hit
    /// </summary>
    private Action _onTargetHit;

    /// <summary>
    /// All the effects currently on the projectile
    /// </summary>
    public readonly List<ScriptableAbilityEffect> Effects = new();
    /// <summary> 
    /// Initializes a new instance of the <see cref="ProjectileInfo"/> class.
    /// </summary>
    public ProjectileInfo(ScriptableProjectile projectile, UnitBase unitBase) {
        ProjectilePrefab = projectile.Prefab;
        ShootingType = projectile.ShootingType;
        Speed = projectile.Speed;
        SpeedCurve = projectile.SpeedCurve;
        Origin = unitBase;
        PiercingAmount = projectile.PiercingTargets;
        TargetFaction = unitBase.AttackTargetFaction;
    }
    /// <summary>
    /// The origin of the projectile
    /// </summary>
    public UnitBase Origin { get; set; }

    /// <summary>
    /// Projectile object that will be spawned
    /// </summary>
    public GameObject ProjectilePrefab;

    /// <summary>
    /// Speed of the projectile.
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// Type of shooting associated with the projectile.
    /// </summary>
    public ShootingType ShootingType { get; set; }

    /// <summary>
    /// Animation curve for the projectile's speed.
    /// </summary>
    public AnimationCurve SpeedCurve { get; set; }

    /// <summary>
    /// Amount of targets can be hit before projectile is destroyed
    /// </summary>
    public int PiercingAmount { get; set; } = 0;

    /// <summary>
    /// The amount of targets the projectile has pierced
    /// </summary>
    public int PiercedTargets { get; set; }

    /// <summary>
    /// Which number of attack this is against the target  
    /// </summary>
    public int AttackAgainstTargets { get; set; }

    /// <summary>
    /// Is added to the damage 
    /// </summary>
    public int DamageBonus { get; set; }

    /// <summary>
    /// Represents how many times a spell has been casted without regenerating mana. Is used to determine multicasts of the same spell
    /// </summary>
    public int SpellNumber { get; set; } = 0;
    public UnitBase Target { get; set; }

    public Faction TargetFaction { get; set; }

    /// <summary>
    /// Handles the projectile when it reaches its destination
    /// </summary>
    /// <param name="shotProjectile">The instance of the projectile</param>
    public void ReachedDestination(GameObject shotProjectile) {
        EffectCaster.CastEffect(OnBeforeHitEffect.Concat(OnHitEffect).Concat(OnAfterHitEffect), this, Origin, Target);
        if(PiercedTargets >= PiercingAmount || PiercingAmount < 0) {
            UnityEngine.Object.Destroy(shotProjectile);
        }
    }
    /// <summary>
    /// Adds an effect to the On Before Hit Effect list
    /// </summary>
    /// <param name="effect">The effect that will be added</param>
    public void AddOnBeforeHitEffect(ScriptableAbilityEffect effect) {
        OnBeforeHitEffect.Add(effect);
        Effects.Add(effect);
    }
    /// <summary>
    /// Adds an effect to the On Hit Effect list
    /// </summary>
    /// <param name="effect">The effect that will be added</param>
    public void AddOnHitEffect(ScriptableAbilityEffect effect) {
        OnHitEffect.Add(effect);
        Effects.Add(effect);
    }
    /// <summary>
    /// Adds an effect to the On After hit Effect list
    /// </summary>
    /// <param name="effect">The effect that will be added</param>
    public void AddOnAfterHitEffect(ScriptableAbilityEffect effect) {
        OnAfterHitEffect.Add(effect);
        Effects.Add(effect);
    }
}
