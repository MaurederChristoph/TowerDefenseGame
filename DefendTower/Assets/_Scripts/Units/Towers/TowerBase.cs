using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base class for all towers 
/// </summary>
public class TowerBase : UnitBase {
    /// <summary>
    /// The effects that will be applied on effect
    /// </summary>
    private List<ScriptableAbilityEffect> OnCastEffect { get; set; } = new();

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
    public float SpellSize { get; private set; } = 1;

    /// <summary>
    /// The amount of damage a spell will do
    /// </summary>
    private float _spellPower;

    /// <summary>
    /// The amount of damage a spell will do
    /// </summary>
    public float SpellPower
    {
        get => _spellPower + (int)StatChanges.GetStatChange(Stats, StatType.Int);
        private set => _spellPower = value;
    }

    /// <summary>
    /// How much mana is needed to create cast a spell
    /// </summary>
    private int _maxMana;

    /// <summary>
    /// How much mana is needed to create cast a spell
    /// </summary>
    public int MaxMana
    {
        get => _maxMana + (int)StatChanges.GetStatChange(Stats, StatType.Cha);
        private set => _maxMana = value;
    }

    /// <summary>
    /// How much mana the tower generates per second
    /// </summary>
    private float _manaRegeneration;

    /// <summary>
    /// How much mana the tower generates per second
    /// </summary>
    public float ManaRegeneration
    {
        get => _manaRegeneration + (int)StatChanges.GetStatChange(Stats, StatType.Wis);
        private set => _manaRegeneration = value;
    }

    /// <summary>
    /// How Long it takes for the tower to cast its spell
    /// </summary>
    public float CastingSpeed { get; private set; }
    /// <summary>
    /// The current mana a unit has
    /// </summary>
    private float _currentMana;

    /// <summary>
    /// The key of the current mana gain coroutine
    /// </summary>
    private string _manaRegenKey;

    /// <summary>
    /// The mana bar of the unit
    /// </summary>
    private InfoBar _manaBar;

    /// <summary>
    /// The base heath regeneration
    /// </summary>
    private float _hpReg;

    /// <summary>
    /// The total base regeneration
    /// </summary>
    public float HpReg
    {
        get => (_hpReg + (int)(StatChanges.GetStatChange(Stats, StatType.Con) / 100) * _healthRegModifier);
        private set => _hpReg = value;
    }

    /// <summary>
    /// If the tower is currently gaining mana
    /// </summary>
    private bool _isCurrentlyGainingMana = false;

    /// <summary>
    /// The prefab of the big spell
    /// </summary>
    private GameObject _bigSpellPrefab;
    /// <summary>
    /// THe profab of the normal spell
    /// </summary>
    private GameObject _normalSpellPrefab;
    /// <summary>
    /// The current health regeneration
    /// </summary>
    private float _healthRegModifier = 1;
    /// <summary>
    /// If the tower is currently casting a spell
    /// </summary>
    private bool _currentlyCasting = false;

    /// <summary>
    /// All tower in range
    /// </summary>
    private readonly List<UnitBase> _towerInRange = new();

    /// <summary>
    /// Translates tower properties form scriptable tower object to tower script
    /// </summary>
    /// <param name="unit">Scriptable unit</param>
    public override void InitUnit(ScriptableUnit unit) {
        base.InitUnit(unit);
        GetComponent<CircleCollider2D>().radius = Range;
        var tower = (ScriptableTower)unit;
        MaxMana = tower.MaxMana;
        _currentMana = 0;
        _manaBar = GetComponents<InfoBar>().First(b => b.BarType == BarType.ManaBar);
        _manaBar.UpdateMaxValue(MaxMana, _currentMana);
        SpellProjectile = tower.SpellProjectile;
        SpellProjectileInfo = new ProjectileInfo(tower.SpellProjectile, this);
        DistributeEffects(SpellProjectile.Effects);
        SpellTargetingStrategy = TargetingStrategy.FromType(tower.SpellTargetingStrategy);
        ManaRegeneration = tower.ManaRegeneration;
        CastingSpeed = tower.CastingSpeed;
        SpellTargets = tower.SpellTargets;
        _bigSpellPrefab = tower.BigSpellPrefab;
        _normalSpellPrefab = SpellProjectileInfo.ProjectilePrefab;
        HpReg = tower.HeathRegeneration;
        PassiveRegeneration();
        StartManaGain();
    }

    /// <summary>
    /// Changes the area the spells effects creatures
    /// </summary>
    /// <param name="value">The amount the spell will be multiplied</param>
    public void ChangeSpellSize(float value) {
        if(SpellSize * value > 0.1f) {
            SpellSize *= value;
        } else {
            SpellSize = 0.1f;
        }
        SpellProjectileInfo.ProjectilePrefab = SpellSize > 1 ? _bigSpellPrefab : _normalSpellPrefab;
    }

    /// <summary>
    /// Changes the faction the spell targets 
    /// </summary>
    /// <param name="targetFaction">The new faction the tower targets</param>
    /// <param name="targetingStrategyType">The new strategy type</param>
    public void ChangeSpellTargeting(Faction targetFaction, TargetingStrategyType targetingStrategyType) {
        SpellTargets = targetFaction;
        SpellTargetingStrategy = TargetingStrategy.FromType(targetingStrategyType);
    }

    /// <summary>
    /// Cast the current spell of the unit
    /// </summary>
    public void CheckSpellCast(bool secondCast = false) {
        if(_unitManager.GetTarget(this, SpellTargets) is null) {
            _currentlyCasting = false;
            _delayedActionHandler.CallAfterSeconds(StartBasicAttackShooting, 0.2f);
            _delayedActionHandler.CallAfterSeconds(StartManaGain, 0.2f);
            _currentMana = 0;
            _manaBar.UpdateCurrentValue(_currentMana);
            return;
        }
        _delayedActionHandler.CallAfterSeconds(CastSpell, CastingSpeed);
    }

    /// <summary>
    /// Try and cast a spell
    /// </summary>
    private void CastSpell(string _ = "") {
        _currentlyCasting = false;
        _currentMana = 0;
        _manaBar.UpdateCurrentValue(_currentMana);
        var target = _unitManager.GetTarget(this, SpellTargets);
        EffectCaster.CastEffect(OnCastEffect, SpellProjectileInfo, this, target);
        SpellProjectileInfo.PiercedTargets = 0;
        ShootingBehavior.Shoot(shootingPoint.position, target, SpellProjectileInfo);
        _delayedActionHandler.CallAfterSeconds(StartBasicAttackShooting, 1f);
        _delayedActionHandler.CallAfterSeconds(StartManaGain, 1f);
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
    public void IncreaseMana(float manaIncreases) {
        _currentMana += manaIncreases;
        if(MaxMana - _currentMana < float.Epsilon && !_currentlyCasting) {
            SpellProjectileInfo.SpellNumber = 0;
            _currentlyCasting = true;
            StopGainMana();
            StopBasicAttackShooting();
            CheckSpellCast();
        }
        _manaBar.UpdateCurrentValue(_currentMana);
    }

    /// <summary>
    /// Changes the shooting type of the projectile
    /// </summary>
    /// <param name="shootingType">New trajectory of the projectile</param>
    public void ChangeShootingType(ShootingType shootingType) {
        SpellProjectileInfo.ShootingType = shootingType;
    }

    /// <summary>
    /// Gain mana
    /// </summary>
    private void ManaGain(string _ = "") {
        IncreaseMana(ManaRegeneration / 3);
        _manaRegenKey = _delayedActionHandler.CallAfterSeconds(ManaGain, 0.3f);
    }

    /// <summary>
    /// Stop the mana gain
    /// </summary>
    private void StopGainMana() {
        _delayedActionHandler.StopDelayedAction(_manaRegenKey);
        _isCurrentlyGainingMana = false;
    }

    /// <summary>
    /// Start the gaining mana
    /// </summary>
    private void StartManaGain(string _ = "") {
        if(_isCurrentlyGainingMana) return;
        _isCurrentlyGainingMana = true;
        ManaGain();
    }

    /// <summary>
    /// Adds heath of the mana regeneration
    /// </summary>
    private void PassiveRegeneration(string _ = "") {
        ChangeHealth((int)(HpReg / 2f), this);
        _delayedActionHandler.CallAfterSeconds(PassiveRegeneration, 0.5f);
    }

    /// <summary>
    /// Changes the health regeneration
    /// </summary>
    /// <param name="value">Value that will be added to the heath regeneration</param>
    public void ChangeHeathRegModifier(float value) {
        _healthRegModifier += value;
    }
    /// <summary>
    /// Adds an effect that will be called on cast
    /// </summary>
    /// <param name="effect"></param>
    public void AddOnCastEffect(ScriptableAbilityEffect effect) {
        OnCastEffect.Add(effect);
    }
    /// <summary>
    /// Adds an effect that will be called the moment before the spell on hit effects are triggered
    /// </summary>
    /// <param name="effect">The effect that will be added</param>
    public void AddOnBeforeSpellHitEffect(ScriptableAbilityEffect effect) {
        SpellProjectileInfo.AddOnBeforeHitEffect(effect);
    }
    /// <summary>
    /// Adds an effect that will be called the moment the spell hit
    /// </summary>
    /// <param name="effect">The effect that will be added</param>
    public void AddOnSpellHitEffect(ScriptableAbilityEffect effect) {
        SpellProjectileInfo.AddOnHitEffect(effect);
    }
    /// <summary>
    /// Adds an effect that will be called the moment after the spell on hit effects are triggered
    /// </summary>
    /// <param name="effect">The effect that will be added</param>
    public void AddOnAfterSpellHitEffect(ScriptableAbilityEffect effect) {
        SpellProjectileInfo.AddOnAfterHitEffect(effect);
    }
    protected override void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<TowerBase>(out var tower)) {
            _towerInRange.Add(tower);
        }
        base.OnTriggerEnter2D(other);
    }
}
