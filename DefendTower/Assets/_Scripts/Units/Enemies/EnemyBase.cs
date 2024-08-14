using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Base class for all enemies
/// </summary>
public class EnemyBase : UnitBase {
	/// <summary>
	/// The time it takes the enemy to complete the track
	/// </summary>
	public float Speed {
		get => _speed * _speedPenalty;
		private set {
			_speed = value;
			ApplySpeedChange();
		}
	}

	/// <summary>
	/// Multiply speed 
	/// </summary>
	public float _speedPenalty = 1f;

	/// <summary>
	/// Duration the unit takes ot compleet the track 
	/// </summary>
	private float _speed = 22;

	/// <summary>
	/// The enemy type this unit is
	/// </summary>
	public EnemyType EnemyType { get; private set; }

	/// <summary>
	/// Changes the speed to half the speed
	/// </summary>
	public void ChangeSpeedPenalty() {
		_speedPenalty = 0.5f;
		ApplySpeedChange();
	}

	/// <summary>
	/// Changes the speed penalty to 1
	/// </summary>
	public void ResetSpeedPenalty() {
		_speedPenalty = 1f;
		ApplySpeedChange();
	}

	/// <summary>
	/// Applies the speed to the spline animator
	/// </summary>
	public void ApplySpeedChange() {
		GetComponent<SplineAnimate>().Duration = Speed;
	}

	/// <summary>
	/// Translates enemy properties form scriptable enemy object to enemy script
	/// </summary>
	/// <typeparam name="T">Type of scriptable unit</typeparam>
	/// <param name="unit">Scriptable unit</param>
	public override void InitUnit(ScriptableUnit unit, Stats stats = null) {
		base.InitUnit(unit);
		var enemy = (ScriptableEnemy)unit;
		Speed = enemy.Speed * _speedPenalty;
		EnemyType = enemy.EnemyType;
	}
}
