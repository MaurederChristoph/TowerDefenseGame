using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles functions that concern all units
/// </summary>
public class UnitManager : MonoBehaviour {
    private EnemyManager _enemyManager;

    private void Start() {
        _enemyManager = GameManager.Instance.EnemyManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="currentTarget"></param>
    /// <returns></returns>
    public UnitBase GetTargeting(UnitBase origin, Faction toTarget) {
        IEnumerable<UnitBase> targets = null;
        switch(toTarget) {
            case Faction.Tower:
                targets = _enemyManager.Enemies;
                break;
            case Faction.Enemy:
                throw new NotImplementedException();
        }
       return origin.AttackTargetingStrategy.GetNextTarget(origin, targets);
    }
}

