using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy", fileName = "Enemy")]
public class ScriptableEnemy : ScriptableUnit {
    [Tooltip("The time it takes a unit to complete the track")]
    [field: SerializeField] public float Time { get; private set; } = 20f;

    [Tooltip("What type of enemy this is")]
    [field: SerializeField] public EnemyType EnemyType { get; private set; }
}
