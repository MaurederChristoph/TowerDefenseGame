using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TowerManager : MonoBehaviour {
    [Tooltip("Base tower that will be instantiated")]
    [SerializeField] private ScriptableTower _scriptableTower;

    [Tooltip("The parent gameboject where all towers will be spawned")]
    [SerializeField] private Transform parent;
    /// <summary>
    /// List of all currently alive towers
    /// <param name="Vector2">Tower position on a grid</param>
    /// <param name="TowerBase">The instance of the tower</param>
    /// </summary>
    private readonly Dictionary<Vector2, TowerBase> _towers = new();
    /// <summary>
    /// Returns a copy of all current enemies
    /// </summary>
    public List<TowerBase> Towers => _towers.Values.ToList();

    /// <summary>
    /// Instanciates a new tower 
    /// </summary>
    /// <param name="position">The position where the tower will be spawned</param>
    /// <param name="gridPosition">The position in the grid where the coordinates will be saved</param>
    public void SpawnTower(Vector3 position, Vector2 gridPosition) {
        if(_towers.ContainsKey(gridPosition)) { return; }
        var tower = Instantiate(_scriptableTower.UnitPrefab, position, Quaternion.identity, parent);
        _towers.Add(gridPosition, (TowerBase)tower);
    }
}
