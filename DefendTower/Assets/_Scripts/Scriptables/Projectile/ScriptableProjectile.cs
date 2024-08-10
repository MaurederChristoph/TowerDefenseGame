using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data storage for projectiles
/// </summary>
[CreateAssetMenu(menuName = "Projectile", fileName = "Projectile")]
public class ScriptableProjectile : ScriptableObject {
    [Tooltip("Instance of the prefab")]
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [Tooltip("The why the projectile is shot")]
    [field: SerializeField] public ShootingType ShootingType { get; private set; } = ShootingType.Path;
    [Tooltip("Travel speed of the projectile")]
    [field: SerializeField] public float Speed { get; private set; } = 10;
    [Tooltip("Speed multiplier based on travel distance")]
    [field: SerializeField] public AnimationCurve SpeedCurve { get; private set; }
    [Tooltip("Amount of targets the projectile flies throw before disappearing")]
    [field: SerializeField] public int PiercingTargets { get; private set; } = 0;
    [Tooltip("The effects that are triggered when the projectile hits its target")]
    [field: SerializeField] public List<ScriptableAbilityEffect> Effects { get; private set; } = new();
}
