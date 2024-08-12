using System;
using UnityEngine;
public class InputManager : MonoBehaviour {
    private GameManager _gameManager;
    private TowerManager _towerManager;
    private HighlightManager _highlightManager;
    private EnemyManager _enemyManager;

    private GridPosInfo _currentHoveredTileInfo;
    private bool _placingTower = false;

    public UnitBase SelectedTower { get; private set; }

    private Vector3 MousePos => Input.mousePosition;
    private void Start() {
        _gameManager = GameManager.Instance;
        _towerManager = _gameManager.TowerManager;
        _highlightManager = _gameManager.HighlightManager;
        _enemyManager = _gameManager.EnemyManager;
    }

    private void FixedUpdate() {
        if(_placingTower) {
            _currentHoveredTileInfo = _highlightManager.CheckCurrentMouseHover(MousePos);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)) {
            _placingTower = !_placingTower;
            _highlightManager.DeleteLastHighlight();
        }
        if(_placingTower && Input.GetKeyDown(KeyCode.Mouse0)) {
            _towerManager.SpawnTower(_currentHoveredTileInfo);
        }
        if(Input.GetKeyDown(KeyCode.P)) {
            _enemyManager.SpawnEnemy(EnemyType.Skeleton);
        }
    }
}
