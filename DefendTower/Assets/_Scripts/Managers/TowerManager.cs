using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
public class TowerManager : MonoBehaviour {
    [Tooltip("Base tower that will be instantiated")]
    [SerializeField] private ScriptableTower _scriptableTower;

    [Tooltip("The parent gameobject where all towers will be spawned")]
    [SerializeField] private Transform _parent;

    /// <summary>
    /// List of all currently alive towers
    /// <param name="Vector2">Tower position on a grid</param>
    /// <param name="TowerBase">The instance of the tower</param>
    /// </summary>
    private readonly Dictionary<Vector3, TowerBase> _towers = new();
    /// <summary>
    /// Returns a copy of all current enemies
    /// </summary>
    public List<TowerBase> Towers => _towers.Values.ToList();

    /// <summary>
    /// Instantiates a new tower 
    /// </summary>
    /// <param name="gridPos">The grid position of a tower</param>
    public void SpawnTower(GridPosInfo gridPos) {
        if(_towers.ContainsKey(gridPos.CurrentHoveredTile)) { return; }
        var tower = Instantiate(_scriptableTower.UnitPrefab, gridPos.CurrentHoveredTilePos, Quaternion.identity, _parent);
        _towers.Add(gridPos.CurrentHoveredTile, (TowerBase)tower);
        tower.InitUnit(_scriptableTower);
    }
    public void RemoveTower(TowerBase tower) {
        _towers.Remove(_towers.FirstOrDefault(t => t.Value == tower).Key);
    }
}