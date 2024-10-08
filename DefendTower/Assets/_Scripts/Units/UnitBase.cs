using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base class for all units
/// </summary>
public class UnitBase : MonoBehaviour {
	/// <summary>
	/// The point form where the unit shoots
	/// </summary>
	[SerializeField] protected Transform shootingPoint;

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

	/// <summary>
	/// Triggered when the unit dies
	/// </summary>
	private Action<UnitBase> _onDeath;

	/// <summary>
	/// Triggered when a new target is acquired
	/// </summary>
	private Action<UnitBase> _onNewTarget;

	/// <summary>
	/// Effects applied when an enemy leaves the range
	/// </summary>
	private List<ScriptableAbilityEffect> OnEnemyLeaveRangeEffect { get; set; } = new();

	/// <summary>
	/// Effects applied when an enemy enters the range
	/// </summary>
	private List<ScriptableAbilityEffect> OnEnemyEnterRangeEffect { get; set; } = new();

	/// <summary>
	/// Effects applied when the unit gets hit
	/// </summary>
	private List<ScriptableAbilityEffect> OnGettingHitEffect { get; set; } = new();

	/// <summary>
	/// Effects applied when the unit dies
	/// </summary>
	private List<ScriptableAbilityEffect> OnDeathEffect { get; set; } = new();

	/// <summary>
	/// Effects applied when the tower dies
	/// </summary>
	private List<ScriptableAbilityEffect> OnTowerDeathEffect { get; set; } = new();

	/// <summary>
	/// Effects applied when the enemy dies
	/// </summary>
	private List<ScriptableAbilityEffect> OnEnemyDeathEffect { get; set; } = new();


	/// <summary>
	/// Faction the unit belongs to
	/// </summary>
	public Faction Faction { get; private set; }

	/// <summary>
	/// The Starting health of a unit
	/// </summary>
	public int MaxHealth { get; private set; }

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
		get => (_attackSpeed + StatChanges.GetStatChange(Stats, StatType.Dex)) * AttackSpeedMultiplier;
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
	protected UnitManager _unitManager;

	/// <summary>
	/// The current instance of the <see cref="DelayedActionHandler"/>
	/// </summary>
	protected DelayedActionHandler _delayedActionHandler;

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

	private InfoBar _infoBar;

	private bool _isShooting = false;
	private bool _canOverHeal = false;
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
	/// <param name="unit">Scriptable unit object</param>
	/// <param name="stats">The starting stats of the unit</param>
	public virtual void InitUnit(ScriptableUnit unit) {
		Stats = new Stats();
		Stats.InitStats();
		_scriptableUnit = unit;
		var conBonus = (int)StatChanges.GetStatChange(Stats, StatType.Con);
		CurrentHealth = unit.MaxHealth + conBonus;
		MaxHealth = unit.MaxHealth + conBonus;
		_infoBar = GetComponents<InfoBar>().First(b => b.BarType == BarType.HeathBar);
		_infoBar.UpdateMaxValue(MaxHealth, CurrentHealth);
		Stats.Con.AddStatChangeListener(UpdateMaxHealth);
		TotalAttackSpeed = unit.AttackSpeed;
		Range = unit.Range;
		AttackTargetFaction = unit.AttackTargets;
		AttackTargetingStrategy = TargetingStrategy.FromType(unit.AttackTargetingStrategy);
		_savedTargetingStrategy = AttackTargetingStrategy;
		BaseProjectile = new ProjectileInfo(unit.Projectile, this);
		DistributeEffects(unit.Projectile.Effects);
		Faction = unit.Faction;
		BurstSize = unit.BurstSize;
		BurstInBetweenShotsMultiplier = unit.BurstInBetweenShotsMultiplier;
		BurstReloadMultiplier = unit.BurstReloadMultiplier;
		CreationTime = Time.time;
		StartBasicAttackShooting();
	}


	/// <summary>
	/// Changes the units health
	/// </summary>
	/// <param name="healthChange">The amount the health will be changed</param>
	/// <param name="origin">The origin of the effect</param>
	public void ChangeHealth(int healthChange, UnitBase origin) {
		if (_vulnerableAmount > 0) {
			healthChange = Mathf.FloorToInt(healthChange * 1.3f);
		}
		if (origin != this) {
			EffectCaster.CastEffect(OnGettingHitEffect, BaseProjectile, this, origin);
		}
		if (CurrentHealth + healthChange > MaxHealth && !_canOverHeal) {
			CurrentHealth = MaxHealth;
		} else {
			CurrentHealth += healthChange;
			if (CurrentHealth > MaxHealth) {
				MaxHealth = CurrentHealth;
				_infoBar.UpdateMaxValue(MaxHealth, CurrentHealth);
			}
		}
		_infoBar.UpdateCurrentValue(CurrentHealth);
		if (CurrentHealth <= 0 && this != null) {
			TryDestroy();
		}
		_onHealthChange?.Invoke(CurrentHealth, healthChange);
	}

	private void TryDestroy() {
		EffectCaster.CastEffect(OnDeathEffect, BaseProjectile, this, this);
		if (CurrentHealth <= 0 && this != null) {
			_onDeath?.Invoke(this);
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
			_onNewTarget?.Invoke(AttackTarget);
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
	/// Interrupts the current shooting coroutine
	/// </summary>
	protected void StopBasicAttackShooting() {
		_isShooting = false;
		_delayedActionHandler.StopDelayedAction(_currentShootingKey);
	}

	/// <summary>
	/// Starts the basic shooting cycle
	/// </summary>
	protected void StartBasicAttackShooting(string _ = "") {
		if (_isShooting) return;
		_isShooting = true;
		CheckForShooting();
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
	/// Distributes the input effects to the corresponding lists
	/// </summary>
	/// <param name="projectileEffects">The effects that will be distributed</param>
	/// <param name="origin">The origin of the effect</param>
	public void DistributeEffects(List<ScriptableAbilityEffect> projectileEffects, UnitBase origin = null) {
		foreach(var effect in projectileEffects) {
			DistributeEffect(effect, origin);
		}
	}

	/// <summary>
	/// Distributes the input effect to the corresponding lists  
	/// </summary>
	/// <param name="effect">The effect that will be distributed</param>
	/// <param name="origin">The origin of the effect</param>
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

	/// <summary>
	/// Sets the max hp of the unit
	/// </summary>
	/// <param name="value">New max hp</param>
	private void UpdateMaxHealth(int value) {
		var hpChange = (int)ResourceSystem.Instance.GetScriptableStatChanges().Con.Amount * value;
		MaxHealth += hpChange;
		CurrentHealth += hpChange;
		_infoBar.UpdateMaxValue(MaxHealth, CurrentHealth);
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
	/// Removes the given method from the enemy leaves tower range notification.
	/// </summary>
	/// <param name="listener">The method that will be removed.</param>
	public void RemoveOnEnemyLeaveRangeListener(Action<UnitBase> listener) {
		_onEnemyLeavesRange -= listener;
	}

	/// <summary>
	/// Adds an effect to be triggered when an enemy leaves the tower's range.
	/// </summary>
	/// <param name="effect">The effect that will be added.</param>
	public void AddOnEnemyLeaveRangeEffect(ScriptableAbilityEffect effect) {
		OnEnemyLeaveRangeEffect.Add(effect);
	}

	/// <summary>
	/// Adds an effect to be triggered when an enemy enters the tower's range.
	/// </summary>
	/// <param name="effect">The effect that will be added.</param>
	public void AddOnEnemyEnterRangeEffect(ScriptableAbilityEffect effect) {
		OnEnemyEnterRangeEffect.Add(effect);
	}

	/// <summary>
	/// Adds an effect to be triggered when the unit is hit.
	/// </summary>
	/// <param name="effect">The effect that will be added.</param>
	public void AddOnGettingHitEffect(ScriptableAbilityEffect effect) {
		OnGettingHitEffect.Add(effect);
	}

	/// <summary>
	/// Adds an effect to be triggered when the unit dies.
	/// </summary>
	/// <param name="effect">The effect that will be added.</param>
	public void AddOnDeathEffect(ScriptableAbilityEffect effect) {
		OnDeathEffect.Add(effect);
	}

	/// <summary>
	/// Adds an effect to be triggered when a tower dies.
	/// </summary>
	/// <param name="effect">The effect that will be added.</param>
	public void AddOnTowerDeathEffect(ScriptableAbilityEffect effect) {
		OnTowerDeathEffect.Add(effect);
	}

	/// <summary>
	/// Adds an effect to be triggered when an enemy dies.
	/// </summary>
	/// <param name="effect">The effect that will be added.</param>
	public void AddOnEnemyDeathEffect(ScriptableAbilityEffect effect) {
		OnEnemyDeathEffect.Add(effect);
	}

	/// <summary>
	/// Adds a listener for the unit's death event.
	/// </summary>
	/// <param name="listener">The method that will be called upon death.</param>
	public void AddOnDeathListener(Action<UnitBase> listener) {
		_onDeath += listener;
	}

	/// <summary>
	/// Removes a listener from the unit's death event.
	/// </summary>
	/// <param name="listener">The method that will be removed.</param>
	public void RemoveOnDeathListener(Action<UnitBase> listener) {
		_onDeath -= listener;
	}

	/// <summary>
	/// Adds a listener for when a new target is acquired.
	/// </summary>
	/// <param name="listener">The method that will be called when a new target is acquired.</param>
	public void AddNewTargetListener(Action<UnitBase> listener) {
		_onNewTarget += listener;
	}

	/// <summary>
	/// Removes a listener from the new target event.
	/// </summary>
	/// <param name="listener">The method that will be removed.</param>
	public void RemoveNewTargetListener(Action<UnitBase> listener) {
		_onNewTarget -= listener;
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
	}

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		var target = other.GetComponent<UnitBase>();
		if (target == null) return;
		target.AddOnDeathListener(HandleUnitDeath);
		if (!other.CompareTag(AttackTargetFaction.ToString())) return;
		_targetList.Add(target);
		_onEnemyEntersRange?.Invoke(target);
		EffectCaster.CastEffect(OnEnemyEnterRangeEffect, BaseProjectile, this, target);
	}
	protected void OnTriggerExit2D(Collider2D other) {
		var target = other.GetComponent<UnitBase>();
		if (target == null) return;
		target.RemoveOnDeathListener(HandleUnitDeath);
		if (!other.CompareTag(AttackTargetFaction.ToString())) return;
		_targetList.Remove(target);
		_onEnemyLeavesRange?.Invoke(target);
		EffectCaster.CastEffect(OnEnemyLeaveRangeEffect, BaseProjectile, this, target);
	}

	/// <summary>
	/// Checks if a units within the range of the unit dies
	/// </summary>
	/// <param name="unit">The unit that died</param>
	private void HandleUnitDeath(UnitBase unit) {
		if (unit is EnemyBase) {
			EffectCaster.CastEffect(OnEnemyDeathEffect, BaseProjectile, this, unit);
		} else {
			EffectCaster.CastEffect(OnTowerDeathEffect, BaseProjectile, this, unit);
		}
	}
}
