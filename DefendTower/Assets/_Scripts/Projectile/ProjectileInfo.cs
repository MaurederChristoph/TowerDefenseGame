using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Info of current data of instantiated projectile
/// </summary>
public class ProjectileInfo {
    private List<ScriptableAbilityEffect> OnBeforeHitEffect { get; set; } = new();
    private List<ScriptableAbilityEffect> OnHitEffect { get; set; } = new();
    private List<ScriptableAbilityEffect> OnAfterHitEffect { get; set; } = new();
    private Action _onTargetChange;
    private Action _onTargetHit;

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
    }
    public UnitBase Origin { get; set; }

    /// <summary>
    /// Projectile object that will be spawned
    /// </summary>
    public readonly GameObject ProjectilePrefab;

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
    public int PiercingTargets { get; set; } = 0;

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
    public int SpellNumber { get; set; }
    public UnitBase Target { get; set; }

    /// <summary>
    /// Handles the projectile when it reaches its destination
    /// </summary>
    /// <param name="shotProjectile">The instance of the projectile</param>
    public void ReachedDestination(GameObject shotProjectile) {
        EffectCaster.CastEffect(OnBeforeHitEffect.Concat(OnHitEffect).Concat(OnAfterHitEffect), this, Origin, Target);
        UnityEngine.Object.Destroy(shotProjectile);
    }

    /// <summary>
    /// Triggered when a new target is selected
    /// </summary>
    public void HandleNewTarget() {
        _onTargetChange?.Invoke();
    }

    /// <summary>
    /// Adds method that will be notified when a new target is chosen
    /// </summary>
    /// <param name="listener">The method that will be notified</param>
    public void AddNewTargetListener(Action listener) {
        _onTargetChange += listener;
    }

    /// <summary>
    /// Removes a method form the notification list when a new target is selected
    /// </summary>
    /// <param name="listener">The method that will be removed</param>
    public void RemoveNewTargetListener(Action listener) {
        _onTargetChange -= listener;
    }
    public void AddOnBeforeHitEffect(ScriptableAbilityEffect effect) {
        OnBeforeHitEffect.Add(effect);
        Effects.Add(effect);
    }
    public void AddOnHitEffect(ScriptableAbilityEffect effect) {
        OnHitEffect.Add(effect);
        Effects.Add(effect);
    }
    public void AddOnAfterHitEffect(ScriptableAbilityEffect effect) {
        OnAfterHitEffect.Add(effect);
        Effects.Add(effect);
    }
}
