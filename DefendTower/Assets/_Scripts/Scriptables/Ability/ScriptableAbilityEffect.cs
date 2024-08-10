using System;
using UnityEngine;

/// <summary>
/// The base class of all ability effects
/// </summary>
public abstract class ScriptableAbilityEffect : ScriptableObject {
    [Tooltip("When the effect triggered")]
    [field: SerializeField] public EffectCallType CallType { get; private set; }

    [Tooltip("What the effect targets")]
    [field: SerializeField] public TargetingType TargetingType { get; private set; }

    /// <summary>
    /// Applies the effect of a projectile to a target
    /// </summary>
    /// <param name="projectile">All infos for the projectile</param>
    /// <param name="origin">Unit that shot the projectile</param>
    /// <param name="target">Receiver of the effect</param>
    public abstract void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target);

    /// <summary>
    /// Calculate the health changed based on <see cref="HealthEffectType"/>
    /// </summary>
    /// <param name="value">The value that will be transformed</param>
    /// <param name="healthChangeType">What type of health effect will be applied</param>
    /// <returns>the Changed value</returns>
    protected static int CalculateHealthChange(int value, HealthEffectType healthChangeType) {
        switch(healthChangeType) {
            case HealthEffectType.Damage:
                value = Math.Abs(value) * -1;
                break;
            case HealthEffectType.Health:
                value = Math.Abs(value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return value;
    }
}
