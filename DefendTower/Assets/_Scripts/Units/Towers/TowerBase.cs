using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all towers 
/// </summary>
public class TowerBase : UnitBase {
    private List<ScriptableAbilityEffect> OnCastEffect { get; set; } = new();
    private List<ScriptableAbilityEffect> OnBeforeSpellHitEffect { get; set; } = new();
    private List<ScriptableAbilityEffect> OnSpellHitEffect { get; set; } = new();
    private List<ScriptableAbilityEffect> OnAfterSpellHitEffect { get; set; } = new();

    /// <summary>
    /// The target faction of spells
    /// </summary>
    public Faction SpellTargets { get; private set; } = Faction.Enemy;
    /// <summary>
    /// The base spell the tower is casting
    /// </summary>
    public ScriptableProjectile SpellProjectile { get; private set; }
    /// <summary>
    /// Modified spell the tower is casting
    /// </summary>
    public ProjectileInfo SpellProjectileInfo { get; private set; }
    /// <summary>
    /// How the unit will choose its target for spells
    /// </summary>
    public TargetingStrategy SpellTargetingStrategy { get; private set; }
    /// <summary>
    /// The size multiplier of the spell
    /// </summary>
    public float SpellSize { get; private set; }
    /// <summary>
    /// How much mana is needed to create cast a spell
    /// </summary>
    public int MaxMana { get; private set; }
    /// <summary>
    /// How much mana the tower generates per second
    /// </summary>
    public float ManaRegeneration { get; private set; }
    /// <summary>
    /// How Long it takes for the tower to cast its spell
    /// </summary>
    public float CastingSpeed { get; private set; }
    /// <summary>
    /// The current mana a unit has
    /// </summary>
    private int _currentMana;

    /// <summary>
    /// Translates tower properties form scriptable tower object to tower script
    /// </summary>
    /// <param name="unit">Scriptable unit</param>
    public override void InitUnit(ScriptableUnit unit) {
        base.InitUnit(unit);
        var tower = (ScriptableTower)unit;
        SpellProjectile = tower.SpellProjectile;
        SpellTargetingStrategy = TargetingStrategy.FromType(tower.SpellTargetingStrategy);
        MaxMana = tower.MaxMana;
        ManaRegeneration = tower.ManaRegeneration;
        CastingSpeed = tower.CastingSpeed;
        SpellTargets = tower.SpellTargets;
    }

    /// <summary>
    /// Changes the area the spells effects creatures
    /// </summary>
    /// <param name="value">The amount the spell will be increased</param>
    public void ChangeSpellSize(float value) {
        if(SpellSize + value > 0.1f) {
            SpellSize += value;
        } else {
            SpellSize = 0.1f;
        }
    }

    /// <summary>
    /// Changes the faction the spell targets 
    /// </summary>
    /// <param name="targetFaction">The new faction the tower targets</param>
    public void ChangeSpellTargeting(Faction targetFaction) {
        SpellTargets = targetFaction;
    }

    /// <summary>
    /// Cast the current spell of the unit
    /// </summary>
    public void CastSpell() {
        throw new NotImplementedException();
        EffectCaster.CastEffect(OnCastEffect, SpellProjectileInfo, this, AttackTarget);
    }

    /// <summary>
    /// Changes the casting speed of a spell
    /// </summary>
    /// <param name="speedChange"></param>
    public void ChangeCastingSpeed(float speedChange) {
        CastingSpeed += speedChange;
    }

    /// <summary>
    /// Increases the current amount of mana
    /// </summary>
    /// <param name="manaIncreases">The mana by which the current mana is increased</param>
    public void IncreaseMana(int manaIncreases) {
        _currentMana += manaIncreases;
        if(_currentMana < MaxMana) {
            return;
        }
        _currentMana = 0;
        CastSpell();
    }

    /// <summary>
    /// Changes the shooting type of the projectile
    /// </summary>
    /// <param name="shootingType">New trajectory of the projectile</param>
    public void ChangeShootingType(ShootingType shootingType) {
        SpellProjectileInfo.ShootingType = shootingType;
    }

    public void AddOnCastEffect(ScriptableAbilityEffect effect) {
        OnCastEffect.Add(effect);
    }
    public void AddOnBeforeSpellHitEffect(ScriptableAbilityEffect effect) {
        OnBeforeSpellHitEffect.Add(effect);
    }
    public void AddOnSpellHitEffect(ScriptableAbilityEffect effect) {
        OnSpellHitEffect.Add(effect);
    }
    public void AddOnAfterSpellHitEffect(ScriptableAbilityEffect effect) {
        OnAfterSpellHitEffect.Add(effect);
    }
}
