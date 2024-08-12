using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base class for all units
/// </summary>
public class UnitBase : MonoBehaviour {
	/// <summary>
	/// The point form where the unit shoots
	/// </summary>
	[SerializeField] private Transform shootingPoint;

	/// <summary>
	/// Triggered when the health changes 
	/// </summary>
	/// <param name="int">Current health value</param>
	/// <param name="int">The amount by which the health changed</param>
	private Action<int, int> _onHealthChange;

	/// <summary>
	/// Triggered when enemy enters the range of the tower
	/// </summary>
	private Action<UnitBase> _onEnemyEntersRange;

	/// <summary>
	/// Triggered when enemy leaves the range of the tower
	/// </summary>
	private Action<UnitBase> _onEnemyLeavesRange;

	private Action<UnitBase> _onDeath;

	private List<ScriptableAbilityEffect> OnEnemyLeaveRangeEffect { get; set; } = new();
	private List<ScriptableAbilityEffect> OnEnemyEnterRangeEffect { get; set; } = new();
	private List<ScriptableAbilityEffect> OnGettingHitEffect { get; set; } = new();
	private List<ScriptableAbilityEffect> OnDeathEffect { get; set; } = new();
	private List<ScriptableAbilityEffect> OnTowerDeathEffect { get; set; } = new();
	private List<ScriptableAbilityEffect> OnEnemyDeathEffect { get; set; } = new();

	/// <summary>
	/// Faction the unit belongs to
	/// </summary>
	public Faction Faction { get; private set; }

	/// <summary>
	/// The Starting health of a unit
	/// </summary>
	private int _maxHealth;

	/// <summary>
	/// The Starting health of a unit
	/// </summary>
	public int MaxHealth {
		get => _maxHealth + (int)StatChanges.GetStatChange(Stats, StatType.Con);
		private set => _maxHealth = value;
	}

	/// <summary>
	/// Represents the damage a unit will do
	/// </summary>
	public int Power {
		get => _power + (int)StatChanges.GetStatChange(Stats, StatType.Str);
		private set => _power = value;
	}

	/// <summary>
	/// Multiples the attack speed
	/// </summary>
	public float AttackSpeedMultiplier { get; private set; } = 1;

	/// <summary>
	/// Attack speed of the unit
	/// </summary>
	private float _attackSpeed;

	/// <summary>
	/// Calculated Attack Speed with AttackSpeedMultiplier 
	/// </summary>
	public float TotalAttackSpeed {
		get => _attackSpeed * AttackSpeedMultiplier + StatChanges.GetStatChange(Stats, StatType.Dex);
		private set => _attackSpeed = value;
	}

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
	public ProjectileInfo BaseProjectile { get; private set; }

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
	public int CurrentHealth { get; private set; }

	/// <summary>
	/// Number of attacks against the current target
	/// </summary>
	private int _attacksAgainstTarget;

	/// <summary>
	/// Reference to the current instance of the <see cref="GameManager"/>
	/// </summary>
	private GameManager _gameManager;

	/// <summary>
	/// Reference to the current instance of the <see cref="UnitManager"/>
	/// </summary>
	private UnitManager _unitManager;

	/// <summary>
	/// The current instance of the <see cref="DelayedActionHandler"/>
	/// </summary>
	private DelayedActionHandler _delayedActionHandler;

	/// <summary>
	/// The current instance of the <see cref="ShootingBehavior"/>
	/// </summary>
	protected ShootingBehavior ShootingBehavior;

	/// <summary>
	/// The key of the current shooting coroutine
	/// </summary>
	private string _currentShootingKey;

	/// <summary>
	/// How often the target is vulnerable  
	/// </summary>
	private int _vulnerableAmount = 0;

	/// <summary>
	/// How often the target it slowed
	/// </summary>
	private int _speedChangeTimes = 0;

	/// <summary>
	/// The time the unit was created
	/// </summary>
	public float CreationTime { get; private set; }

	/// <summary>
	/// Reference to the scriptable unit
	/// </summary>
	private ScriptableUnit _scriptableUnit;

	/// <summary>
	/// Current true targeting strategy. Used as a reference to reset  
	/// </summary>
	private TargetingStrategy _savedTargetingStrategy;

	/// <summary>
	/// The current effects of the projectile
	/// </summary>
	private List<ScriptableAbilityEffect> _projectileEffects;

	/// <summary>
	/// The stats of the tower
	/// </summary>
	public Stats Stats { get; private set; }

	/// <summary>
	/// The stats that are changed temporary and displayed in the UI
	/// </summary>
	public List<Stats> TempStats { get; private set; }

	private bool _isQuitting = false;

	private readonly List<UnitBase> _targetList = new();

	private int _power;

	private void Awake() {
		_gameManager = GameManager.Instance;
		_unitManager = _gameManager.UnitManager;
		_delayedActionHandler = _gameManager.DelayedActionHandler;
		ShootingBehavior = _gameManager.ShootingBehavior;
	}

	private void OnApplicationQuit() => _isQuitting = true;

	/// <summary>
	/// Translates unit properties form scriptable unit object to unit script
	/// </summary>
	/// <param name="unit">Scriptable unit</param>
	public virtual void InitUnit(ScriptableUnit unit) {
		Stats = new Stats();
		MaxHealth = unit.MaxHealth;
		CurrentHealth = MaxHealth;
		Power = unit.Power;
		TotalAttackSpeed = unit.AttackSpeed;
		Range = unit.Range;
		AttackTargetFaction = unit.AttackTargets;
		AttackTargetingStrategy = TargetingStrategy.FromType(unit.AttackTargetingStrategy);
		BaseProjectile = new ProjectileInfo(unit.Projectile, this);
		DistributeEffects(unit.Projectile.Effects);
		Faction = unit.Faction;
		BurstSize = unit.BurstSize;
		BurstInBetweenShotsMultiplier = unit.BurstInBetweenShotsMultiplier;
		BurstReloadMultiplier = unit.BurstReloadMultiplier;
		CreationTime = Time.time;
		_speedChangeTimes = 0;
		CheckForShooting();
	}


	/// <summary>
	/// Changes the units health
	/// </summary>
	/// <param name="healthChange">The amount the health will be changed</param>
	/// <param name="origin">The origin of the effect</param>
	public virtual void ChangeHealth(int healthChange, UnitBase origin) {
		if (_vulnerableAmount > 0) {
			healthChange = Mathf.FloorToInt(healthChange * 1.5f);
		}
		if (origin != this) {
			EffectCaster.CastEffect(OnGettingHitEffect, BaseProjectile, this, origin);
		}
		CurrentHealth += healthChange;
		if (CurrentHealth <= 0 && this != null) {
			TryDestroy();
		}
		_onHealthChange?.Invoke(CurrentHealth, healthChange);
	}

	private void TryDestroy() {
		EffectCaster.CastEffect(OnDeathEffect, BaseProjectile, this, this);
		if (CurrentHealth <= 0 && this != null) {
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Checks if there is a target to be shot and then triggers the shooting of that target 
	/// </summary>
	private void CheckForShooting(string _ = "") {
		var newTarget = _unitManager.GetTarget(this, AttackTargetFaction);
		float time;
		if (newTarget != AttackTarget) {
			AttackTarget = newTarget;
			_attacksAgainstTarget = 1;
		} else {
			_attacksAgainstTarget++;
		}
		if (newTarget != null) {
			if (_attacksAgainstTarget % BurstSize == 0 && _attacksAgainstTarget != 0) {
				time = (1 / TotalAttackSpeed) * BurstReloadMultiplier;
			} else {
				time = (1 / TotalAttackSpeed) * BurstInBetweenShotsMultiplier;
			}
			BaseProjectile.AttackAgainstTargets = _attacksAgainstTarget;
			ShootingBehavior.Shoot(shootingPoint.position, newTarget, BaseProjectile);
		} else {
			time = Time.fixedDeltaTime;
		}
		_currentShootingKey = _delayedActionHandler.CallAfterSeconds(CheckForShooting, time);
	}

	/// <summary>
	/// Reset the unit 
	/// </summary>
	public void ResetUnit() {
		_delayedActionHandler.StopDelayedAction(_currentShootingKey);
		InitUnit(_scriptableUnit);
	}

	/// <summary>
	/// Change the targeting strategy but saving the original strategy  
	/// </summary>
	/// <param name="targetingStrategy">The new targeting strategy</param>
	public void ChangeTargetingStrategyTemporary(TargetingStrategy targetingStrategy) {
		AttackTargetingStrategy = targetingStrategy;
	}

	/// <summary>
	/// Change the targeting strategy and change the saved targeting strategy to the new targeting strategy 
	/// </summary>
	/// <param name="targetingStrategy"></param>
	public void ChangeTargetingStrategyPermanent(TargetingStrategy targetingStrategy) {
		AttackTargetingStrategy = targetingStrategy;
		_savedTargetingStrategy = targetingStrategy;
	}

	/// <summary>
	/// Change the targeting strategy to the saved one
	/// </summary>
	public void ResetTargetingStrategy() {
		AttackTargetingStrategy = _savedTargetingStrategy;
	}

	/// <summary>
	/// Add one vulnerable stack
	/// </summary>
	public void AddVulnerable(string _ = "") => _vulnerableAmount++;

	/// <summary>
	/// Remove one vulnerable stack
	/// </summary>
	public void RemoveVulnerable(string _ = "") => _vulnerableAmount--;

	/// <summary>
	/// Changes the multiplier that the attack speed is multiplied with
	/// </summary>
	/// <param name="value">The amount that will be added</param>
	public void ChangeAttackSpeedMultiplier(float value) {
		if (AttackSpeedMultiplier + value > 0.1f) {
			AttackSpeedMultiplier += value;
		} else {
			AttackSpeedMultiplier = 0.1f;
		}
	}

	/// <summary>
	/// Changes the shoots per burst volley
	/// </summary>
	/// <param name="value">The amount by which the burst size will be changed</param>
	public void ChangeBurstAmount(int value) {
		if (BurstSize + value >= 1) {
			BurstSize += value;
		} else {
			BurstSize = 1;
		}
	}

	/// <summary>
	/// Changes duration for how long it takes to reload after a shot burst volley
	/// </summary>
	/// <param name="value">The value that will be added to the reload multiplier</param>
	public void ChangeBurstReloadSpeed(float value) {
		if (BurstReloadMultiplier + value >= 0.1f) {
			BurstReloadMultiplier += value;
		} else {
			BurstReloadMultiplier = 0.1f;
		}
	}

	/// <summary>
	/// Changes the duration of how long it takes each shot in a burst to follow the next shot
	/// </summary>
	/// <param name="value">The value that will be added to the burst in between shots multiplier</param>
	public void ChangeBurstInBetweenShotsMultiplier(float value) {
		if (BurstInBetweenShotsMultiplier + value >= 0.1f) {
			BurstInBetweenShotsMultiplier += value;
		} else {
			BurstInBetweenShotsMultiplier = 0.1f;
		}
	}
	/// <summary>
	/// Change the target of the current attacks
	/// </summary>
	/// <param name="target">New target for attack</param>
	public void ChangeCurrentAttackTarget(UnitBase target) {
		AttackTarget = target;
	}

	/// <summary>
	/// Change the targets speed to be slowed
	/// </summary>
	public void ApplySpeedPenalty() {
		if (this is not EnemyBase enemy) {
			return;
		}
		_speedChangeTimes++;
		enemy.ChangeSpeedPenalty();
	}


	/// <summary>
	/// Resets the speed to the original speed
	/// </summary>
	public void ResetSpeed() {
		if (this is EnemyBase enemy && _speedChangeTimes == 0) {
			enemy.ChangeSpeedPenalty();
		}
	}
	public void DistributeEffects(List<ScriptableAbilityEffect> projectileEffects, UnitBase origin = null) {
		foreach(var effect in projectileEffects) {
			DistributeEffect(effect, origin);
		}
	}
	public void DistributeEffect(ScriptableAbilityEffect effect, UnitBase origin = null) {
		switch(effect.CallType) {
			case EffectCallType.OneTimeApply:
				if (origin != null) {
					effect.ApplyEffect(BaseProjectile, origin, this);
				}
				break;
			case EffectCallType.ChangeSpell:
				if (origin != null) {
					effect.ApplyEffect(((TowerBase)this).SpellProjectileInfo, origin, this);
				}
				break;
			case EffectCallType.ChangeProjectile:
				if (origin != null) {
					effect.ApplyEffect(BaseProjectile, origin, this);
				}
				break;
			case EffectCallType.ProjectileBeforeHit:
				BaseProjectile.AddOnBeforeHitEffect(effect);
				break;
			case EffectCallType.ProjectileOnHit:
				BaseProjectile.AddOnHitEffect(effect);
				break;
			case EffectCallType.ProjectileAfterHit:
				BaseProjectile.AddOnAfterHitEffect(effect);
				break;
			case EffectCallType.EnemyDeath:
				AddOnEnemyDeathEffect(effect);
				break;
			case EffectCallType.TowerDeath:
				AddOnTowerDeathEffect(effect);
				break;
			case EffectCallType.OnDeath:
				AddOnDeathEffect(effect);
				break;
			case EffectCallType.GettingHit:
				AddOnGettingHitEffect(effect);
				break;
			case EffectCallType.TargetEnterRange:
				AddOnEnemyEnterRangeEffect(effect);
				break;
			case EffectCallType.TargetLeavesRange:
				AddOnEnemyLeaveRangeEffect(effect);
				break;
			case EffectCallType.OnCast:
				(this as TowerBase)?.AddOnCastEffect(effect);
				break;
			case EffectCallType.SpellBeforeHit:
				(this as TowerBase)?.AddOnBeforeSpellHitEffect(effect);
				break;
			case EffectCallType.SpellOnHit:
				(this as TowerBase)?.AddOnSpellHitEffect(effect);
				break;
			case EffectCallType.SpellAfterHit:
				(this as TowerBase)?.AddOnAfterSpellHitEffect(effect);
				break;
		}
	}


	#region Event Handlers

	/// <summary>
	/// Add method to be notified when the health units health changes
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

	/// <summary>
	/// Add method to be notified when enemy enters tower range
	/// </summary>
	/// <param name="listener">The method that will be called</param>
	public void AddOnEnemyEnterRangeListener(Action<UnitBase> listener) {
		_onEnemyEntersRange += listener;
	}

	/// <summary>
	/// Removes the given method form the enemy enter tower range notification
	/// </summary>
	/// <param name="listener">The method that will be removed</param>
	public void RemoveOnEnemyEnterRangeListener(Action<UnitBase> listener) {
		_onEnemyEntersRange -= listener;
	}

	/// <summary>
	/// Add method to be notified when enemy leaves tower range
	/// </summary>
	/// <param name="listener">The method that will be called</param>
	public void AddOnEnemyLeaveRangeListener(Action<UnitBase> listener) {
		_onEnemyLeavesRange += listener;
	}

	/// <summary>
	/// Removes the given method form the enemy leaves tower range notification
	/// </summary>
	/// <param name="listener">The method that will be removed</param>
	public void RemoveOnEnemyLeaveRangeListener(Action<UnitBase> listener) {
		_onEnemyLeavesRange -= listener;
	}
	public void AddOnEnemyLeaveRangeEffect(ScriptableAbilityEffect effect) {
		OnEnemyLeaveRangeEffect.Add(effect);
	}
	public void AddOnEnemyEnterRangeEffect(ScriptableAbilityEffect effect) {
		OnEnemyEnterRangeEffect.Add(effect);
	}
	public void AddOnGettingHitEffect(ScriptableAbilityEffect effect) {
		OnGettingHitEffect.Add(effect);
	}
	public void AddOnDeathEffect(ScriptableAbilityEffect effect) {
		OnDeathEffect.Add(effect);
	}
	public void AddOnTowerDeathEffect(ScriptableAbilityEffect effect) {
		OnTowerDeathEffect.Add(effect);
	}
	public void AddOnEnemyDeathEffect(ScriptableAbilityEffect effect) {
		OnEnemyDeathEffect.Add(effect);
	}

	public void AddOnDeathListener(Action<UnitBase> listener) {
		_onDeath += listener;
	}
	public void RemoveOnDeathListener(Action<UnitBase> listener) {
		_onDeath -= listener;
	}

	#endregion

	private void OnDestroy() {
		if (_isQuitting) {
			return;
		}
		if (this is EnemyBase enemy) {
			GameManager.Instance.EnemyManager.RemoveEnemy(enemy);
		}
		if (this is TowerBase tower) {
			GameManager.Instance.TowerManager.RemoveTower(tower);
		}
		foreach(var target in _targetList.ToList()) {
			_targetList.Remove(target);
			EffectCaster.CastEffect(OnEnemyLeaveRangeEffect, BaseProjectile, this, target);
		}
		_onDeath?.Invoke(this);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		var target = other.GetComponent<UnitBase>();
		target.AddOnDeathListener(HandleUnitDeath);
		if (!other.CompareTag(AttackTargetFaction.ToString())) return;
		_targetList.Add(target);
		EffectCaster.CastEffect(OnEnemyEnterRangeEffect, BaseProjectile, this, target);
	}
	private void OnTriggerExit2D(Collider2D other) {
		var target = other.GetComponent<UnitBase>();
		target.RemoveOnDeathListener(HandleUnitDeath);
		if (!other.CompareTag(AttackTargetFaction.ToString())) return;
		_targetList.Remove(target);
		EffectCaster.CastEffect(OnEnemyLeaveRangeEffect, BaseProjectile, this, target);
	}

	private void HandleUnitDeath(UnitBase unit) {
		if (unit is EnemyBase) {
			EffectCaster.CastEffect(OnEnemyDeathEffect, BaseProjectile, this, unit);
		} else {
			EffectCaster.CastEffect(OnTowerDeathEffect, BaseProjectile, this, unit);
		}
	}
}
