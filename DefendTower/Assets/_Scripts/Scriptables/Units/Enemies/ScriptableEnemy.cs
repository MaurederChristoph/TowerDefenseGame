using UnityEngine;

/// <summary>
/// Data Storage for all enemies    
/// </summary>
[CreateAssetMenu(menuName = "Enemy", fileName = "Enemy")]
public class ScriptableEnemy : ScriptableUnit {
    /// <summary>
    /// The time it takes a unit to complete the track
    /// </summary>
    [Tooltip("The time it takes a unit to complete the track")]
    [field: SerializeField] public float Time { get; private set; } = 20f;
    
    /// <summary>
    /// What type of enemy this is
    /// </summary>
    [Tooltip("What type of enemy this is")]
    [field: SerializeField] public EnemyType EnemyType { get; private set; }
}