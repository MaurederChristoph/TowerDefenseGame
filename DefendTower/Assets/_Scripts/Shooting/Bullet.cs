using System;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour {
	private bool _canCollide = false;
	private Faction _faction;
	private Action<ProjectileInfo, GameObject> _onCollisionCall;
	private ProjectileInfo _projectileInfo;
	private readonly List<UnitBase> _targets = new();
	
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
