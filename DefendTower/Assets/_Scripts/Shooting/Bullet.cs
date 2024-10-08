using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the bullet if bullet coalition is enabled
/// </summary>
public class Bullet : MonoBehaviour {
	private bool _canCollide = false;
	private Faction _faction;
	private Action<ProjectileInfo, GameObject> _onCollisionCall;
	private ProjectileInfo _projectileInfo;
	private readonly List<UnitBase> _targets = new();
	
	/// <summary>
	/// Enables the bullet to collide with units
	/// </summary>
	/// <param name="faction">What faction the bullet will collide with</param>
	/// <param name="reachedDestination">What function will be called when the bullet reaches the destination</param>
	/// <param name="projectileInfo">"What type of bullet this is"</param>
	public void EnableCollider(Faction faction, Action<ProjectileInfo, GameObject> reachedDestination, ProjectileInfo projectileInfo) {
		_canCollide = true;
		_faction = faction;
		_projectileInfo = projectileInfo;
		_onCollisionCall += reachedDestination;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!_canCollide || !other.TryGetComponent<UnitBase>(out var unit) || unit.Faction != _faction || _targets.Contains(unit)) return;
		_targets.Add(unit);
		_projectileInfo.Target = unit;
		_projectileInfo.PiercedTargets++;
		_onCollisionCall.Invoke(_projectileInfo, gameObject);
	}
}
