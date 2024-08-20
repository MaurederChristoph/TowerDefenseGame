using System;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Handel user Input
/// </summary>
public class InputManager : MonoBehaviour {
    /// <summary>
    /// Reference to the button that shows if a tower can be placed
    /// </summary>
    [SerializeField] private Button _towerSpawnButton;
    /// <summary>
    /// Current Instance of the game manager
    /// </summary>
    private GameManager _gameManager;
    /// <summary>
    /// Current instance of the tower manager
    /// </summary>
    private TowerManager _towerManager;
    /// <summary>
    /// Current instance of the highlight manager
    /// </summary>
    private HighlightManager _highlightManager;
    /// <summary>
    /// Info of the currently highlighted tile
    /// </summary>
    private GridPosInfo _currentHoveredTileInfo;
    /// <summary>
    /// If the user is able to place a tower
    /// </summary>
    private bool _placingTower;
    /// <summary>
    /// The current position of the mouse
    /// </summary>
    private Vector3 MousePos => Input.mousePosition;
    /// <summary>
    /// How many tower the user can place
    /// </summary>
    private int _towersAvailable = 0;
    private void Start() {
        _gameManager = GameManager.Instance;
        _towerManager = _gameManager.TowerManager;
        _highlightManager = _gameManager.HighlightManager;
        _gameManager.AddPowerLevelChangeListener(CheckForNewTower);
    }
    /// <summary>
    /// Checks if with the current power level a new tower can be placed
    /// </summary>
    /// <param name="powerLevel">The current power level of tha game</param>
    private void CheckForNewTower(int powerLevel) {
        if(powerLevel > 10) return;
        _towersAvailable++;
        _towerSpawnButton.interactable = true;
    }

    private void FixedUpdate() {
        if(_placingTower) {
            _currentHoveredTileInfo = _highlightManager.CheckCurrentMouseHover(MousePos);
        }
    }

    private void Update() {
        if(_placingTower && Input.GetKeyDown(KeyCode.Mouse0)) {
            if(_currentHoveredTileInfo is not null && _towerManager.SpawnTower(_currentHoveredTileInfo)) {
                _highlightManager.DeleteLastHighlight();
                _placingTower = false;
                _towersAvailable--;
                if(_towersAvailable == 0) {
                    _towerSpawnButton.interactable = false;
                }
            }
        }
    }

    /// <summary>
    /// Make towers placeable
    /// </summary>
    public void MakeTowerPlayable() {
        _placingTower = true;
    }
}
