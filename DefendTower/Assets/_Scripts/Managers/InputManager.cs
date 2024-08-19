using System;
using UnityEngine;
using UnityEngine.UI;
public class InputManager : MonoBehaviour {
	[SerializeField] private Button _towerSpawnButton;
	private GameManager _gameManager;
	private TowerManager _towerManager;
	private HighlightManager _highlightManager;
	private GridPosInfo _currentHoveredTileInfo;
	private bool _placingTower;
	private Vector3 MousePos => Input.mousePosition;

	private int _towersAvailable = 0;
	private void Start() {
		_gameManager = GameManager.Instance;
		_towerManager = _gameManager.TowerManager;
		_highlightManager = _gameManager.HighlightManager;
		_gameManager.AddPowerLevelChangeListener(CheckForNewTower);
	}
	private void CheckForNewTower(int i) {
		if (i > 10) return;
		_towersAvailable++;
		_towerSpawnButton.interactable = true;
	}

	private void FixedUpdate() {
		if (_placingTower) {
			_currentHoveredTileInfo = _highlightManager.CheckCurrentMouseHover(MousePos);
		}
	}

	private void Update() {
		if (_placingTower && Input.GetKeyDown(KeyCode.Mouse0)) {
			if (_currentHoveredTileInfo is not null && _towerManager.SpawnTower(_currentHoveredTileInfo)) {
				_highlightManager.DeleteLastHighlight();
				_placingTower = false;
				_towersAvailable--;
				if (_towersAvailable == 0) {
					_towerSpawnButton.interactable = false;
				}
			}
		}
	}

	public void MakeTowerPlayable() {
		_placingTower = true;
	}
}
