using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all units
/// </summary>
public class UnitBase : MonoBehaviour {
    /// <summary>
    /// The Starting health of an unit
    /// </summary>
    public int MaxHealth { get; private set; }
    /// <summary>
    /// Represents the damage a unit will do
    /// </summary>
    public int Power { get; private set; }
    /// <summary>
    /// The amount of attacks a unit will do per second
    /// </summary>
    public float AttackSpeed { get; private set; }
    /// <summary>
    /// The distance the unit can shoot
    /// </summary>
    public float Range { get; private set; }
    /// <summary>
    /// The target faction of basic attacks
    /// </summary>
    public Faction AttackTargetFaction { get; private set; }
    /// <summary>
    /// How the unit will choose it's target for basic attacks
    /// </summary>
    public TargetingStrategy AttackTargetingStrategy { get; private set; }
    /// <summary>
    /// The projectile the units is shooting
    /// </summary>
    public ScriptableProjectile Projectile { get; private set; }
    /// <summary>
    /// The amount off projectiles that are shot before reloading
    /// </summary>
    public float BurstSize { get; private set; }
    /// <summary>
    /// Multiplies the Attack speed between each shot within a burst volley
    /// </summary>
    public float InBetweenBurstShotsMultiplier { get; private set; }
    /// <summary>
    /// Multiplies the Attack speed after the burst is over
    /// </summary>    
    public float BurstReloadMultiplier { get; private set; }

    /// <summary>
    /// The current target of the units basic attacks
    /// </summary>
    public UnitBase AttackTarget { get; private set; }

    /// <summary>
    /// Translates unit properties form scriptable unit object to unit script
    /// </summary>
    /// <typeparam name="T">Type of scriptable unit</typeparam>
    /// <param name="unit">Scriptable unit</param>
    public virtual void InitUnit(ScriptableUnit unit) {
        MaxHealth = unit.MaxHealth;
        Power = unit.Power;
        AttackSpeed = unit.AttackSpeed;
        Range = unit.Range;
        AttackTargetFaction = unit.AttackTargets;
        AttackTargetingStrategy = TargetingStrategy.FromType(unit.AttackTargetingStrategy);
        Projectile = unit.Projectile;
    }
}
