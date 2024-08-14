using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base class for all towers 
/// </summary>
public class TowerBase : UnitBase {
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
	public float SpellPower {
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
	public int MaxMana {
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
	public float ManaRegeneration {
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

	private bool _isCurrentlyGainingMana = false;

	private GameObject _bigSpellPrefab;
	private GameObject _normalSpellPrefab;
	
	/// <summary>
	/// Translates tower properties form scriptable tower object to tower script
	/// </summary>
	/// <param name="unit">Scriptable unit</param>
	public override void InitUnit(ScriptableUnit unit, Stats stats = null) {
		base.InitUnit(unit, stats);
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
		StartManaGain();
	}

	/// <summary>
	/// Changes the area the spells effects creatures
	/// </summary>
	/// <param name="value">The amount the spell will be multiplied</param>
	public void ChangeSpellSize(float value) {
		if (SpellSize * value > 0.1f) {
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
	public void ChangeSpellTargeting(Faction targetFaction) {
		SpellTargets = targetFaction;
	}

	private bool _currentylCasting = false;
	/// <summary>
	/// Cast the current spell of the unit
	/// </summary>
	public void CheckSpellCast(bool secondCast = false) {
		if (_unitManager.GetTarget(this, SpellTargets) is null) {
			_currentylCasting = false;
			_delayedActionHandler.CallAfterSeconds(StartBasicAttackShooting, 0.2f);
			_delayedActionHandler.CallAfterSeconds(StartManaGain, 0.2f);
			_currentMana = 0;
			_manaBar.UpdateCurrentValue(_currentMana);
			return;
		}
		_delayedActionHandler.CallAfterSeconds(CastSpell, CastingSpeed);
	}

	private void CastSpell(string _ = "") {
		_currentylCasting = false;
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
		if (_currentMana >= MaxMana && !_currentylCasting) {
			SpellProjectileInfo.SpellNumber = 0;
			_currentylCasting = true;
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

	public override void ResetUnit() {
		StopGainMana();
		base.ResetUnit();
	}

	private void ManaGain(string _ = "") {
		IncreaseMana(ManaRegeneration / 3);
		_manaRegenKey = _delayedActionHandler.CallAfterSeconds(ManaGain, 0.3f);
	}

	private void StopGainMana() {
		_delayedActionHandler.StopDelayedAction(_manaRegenKey);
		_isCurrentlyGainingMana = false;
	}

	private void StartManaGain(string _ = "") {
		if (_isCurrentlyGainingMana) return;
		_isCurrentlyGainingMana = true;
		ManaGain();
	}

	public void AddOnCastEffect(ScriptableAbilityEffect effect) {
		OnCastEffect.Add(effect);
	}
	public void AddOnBeforeSpellHitEffect(ScriptableAbilityEffect effect) {
		SpellProjectileInfo.AddOnBeforeHitEffect(effect);
	}
	public void AddOnSpellHitEffect(ScriptableAbilityEffect effect) {
		SpellProjectileInfo.AddOnHitEffect(effect);
	}
	public void AddOnAfterSpellHitEffect(ScriptableAbilityEffect effect) {
		SpellProjectileInfo.AddOnAfterHitEffect(effect);
	}
}
