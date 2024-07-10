using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Base class for all units
/// </summary>
public class UnitBase : MonoBehaviour {
    /// <summary>
    /// The point form where the unit shoots
    /// </summary>
    [SerializeField] private Transform _shootingPoint;
    /// <summary>
    /// Will be triggered when the health changes 
    /// </summary>
    /// <param name="int">New current health value</param>
    /// <param name="int">The amount by which the health changed</param>
    private Action<int, int> _onHealthChange;

    /// <summary>
    /// The Starting health of a unit
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
    public ProjectileInfo Projectile { get; private set; }
    /// <summary>
    /// The amount off projectiles that are shot before reloading
    /// </summary>
    public float BurstSize { get; private set; }
    /// <summary>
    /// Multiplies the Attack speed between each shot within a burst volley
    /// </summary>
    public float BurstInBetweenShotsMultiplier { get; private set; }
    /// <summary>
    /// Multiplies the Attack speed after the burst is over
    /// </summary>    
    public float BurstReloadMultiplier { get; private set; }
    /// <summary>
    /// The current target of the units basic attacks
    /// </summary>
    public UnitBase AttackTarget { get; private set; }

    /// <summary>
    /// The current amount of hp
    /// </summary>
    private int _currentHealth;

    /// <summary>
    /// The current amount of attacks against the current target
    /// </summary>
    private int _attackAgainstTargets;

    /// <summary>
    /// The current instance of the <see cref="DelayedActionHandler"/>
    /// </summary>
    private DelayedActionHandler _delayedActionHandler;

    /// <summary>
    /// The current instance of the <see cref="ShootingBehavior"/>
    /// </summary>
    private ShootingBehavior _shootingBehavior;

    /// <summary>
    /// The key of the current shooting coroutine
    /// </summary>
    private string _currentShootingKey;

    /// <summary>
    /// Reference to the current instance of the unit manager
    /// </summary>
    private UnitManager _unitManager;
    private void Start() {
        _unitManager = GameManager.Instance.UnitManager;
    }

    /// <summary>
    /// Translates unit properties form scriptable unit object to unit script
    /// </summary>
    /// <typeparam name="T">Type of scriptable unit</typeparam>
    /// <param name="unit">Scriptable unit</param>
    protected virtual void InitUnit(ScriptableUnit unit) {
        MaxHealth = unit.MaxHealth;
        Power = unit.Power;
        AttackSpeed = unit.AttackSpeed;
        Range = unit.Range;
        AttackTargetFaction = unit.AttackTargets;
        AttackTargetingStrategy = TargetingStrategy.FromType(unit.AttackTargetingStrategy);
        Projectile = unit.Projectile.GetInfo();
        CheckForShooting();
    }

    /// <summary>
    /// Changes the units health
    /// </summary>
    /// <param name="healthChange"></param>
    protected virtual void ChangeHealth(int healthChange) {
        _currentHealth += healthChange;
        if(_currentHealth <= 0) {
            Destroy(gameObject);
        }
        _onHealthChange.Invoke(_currentHealth, healthChange);
    }

    /// <summary>
    /// Checks if there is a target to be shot and then shoots that target 
    /// </summary>
    private void CheckForShooting(string _ = "") {
        var newTarget = _unitManager.GetTarget(this, AttackTargetFaction);
        var time = 0f;
        if(newTarget != AttackTarget) {
            AttackTarget = newTarget;
            _attackAgainstTargets = 0;
        } else {
            _attackAgainstTargets++;
        }
        if(newTarget != null) {
            if(_attackAgainstTargets > BurstSize) {
                time = BurstReloadMultiplier * AttackSpeed;
            } else {
                time = BurstInBetweenShotsMultiplier * AttackSpeed;
            }
            _shootingBehavior.Shoot(_shootingPoint.position, newTarget.transform.position, Projectile);
        } else {
            time = Time.fixedDeltaTime;
        }
        _currentShootingKey = _delayedActionHandler.CallAfterSeconds(CheckForShooting, time);
    }

    /// <summary>
    /// This be notified when the health units health changes
    /// </summary>
    /// <param name="listener">The method that will be called</param>
    /// <param name="Action int 1">New current health value</param>
    /// <param name="Action int 2">Amount by which the health changed</param>
    public void AddHealthChangeListener(Action<int, int> listener) {
        _onHealthChange += listener;
    }

    /// <summary>
    /// Removes the given method form the health change notification
    /// </summary>
    /// <param name="listener">The method that will be removed</param>
    public void RemoveHealthChangeListener(Action<int, int> listener) {
        _onHealthChange += listener;
    }
}
