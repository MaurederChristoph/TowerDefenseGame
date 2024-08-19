using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Handles the game flow
/// </summary>
public class GameManager : Singleton<GameManager> {
	public GameState CurrentGameState { get; private set; }

	/// <summary>
	/// Reference to the current instance of the enemy manager
	/// </summary>
	[field: SerializeField] public EnemyManager EnemyManager { get; private set; }

	/// <summary>
	/// Reference to the current instance of the Delayed Action Handler
	/// </summary>
	[field: SerializeField] public DelayedActionHandler DelayedActionHandler { get; private set; }

	/// <summary>
	/// Reference to the current instance of the Unit Manager
	/// </summary>
	[field: SerializeField] public UnitManager UnitManager { get; private set; }

	/// <summary>
	/// Reference to the current instance of the Tower Manager
	/// </summary>
	[field: SerializeField] public TowerManager TowerManager { get; private set; }

	/// <summary>
	/// Reference to the current instance of the Input Manager 
	/// </summary>
	[field: SerializeField] public InputManager InputManager { get; private set; }

	/// <summary>
	/// Reference to the current instance of the Highlight Manager
	/// </summary>
	[field: SerializeField] public HighlightManager HighlightManager { get; private set; }

	/// <summary>
	/// Reference to the current instance of the abiltiy selection Manager
	/// </summary>
	[field: SerializeField] public AbilitySelectionManager AbilitySelectionManager { get; private set; }

	/// <summary>
	/// Reference to the current instance of the Shooting Behavior
	/// </summary>
	[field: SerializeField] public ShootingBehavior ShootingBehavior { get; private set; }

	[field: SerializeField] private TimerScript _timer;

	[field: SerializeField] private GameObject _winScreen;

	private bool _canWin = false;
	
	public int CurrentPowerLevel {
		get => _timer.Level;
	}

	private void Start() {
		ChangeState(GameState.SpawnMap);
		_timer.AddOnPowerLevelChangeListener(WinConCheck);
	}
	
	private void WinConCheck(int powerLevel) {
		if (powerLevel == 16) {
			_canWin = true;
		}
	}

	private void Update() {
		if (_canWin && EnemyManager.Enemies.Count == 0) {
			_winScreen.SetActive(true);
		}
	}

	/// <summary>
	/// Changes the current state of the game to the specified game state.
	/// </summary>
	/// <param name="gameState">The new state to change to.</param>
	public void ChangeState(GameState gameState) {
		switch(gameState) {
			case GameState.SpawnMap:
				break;
			case GameState.Combat:
				break;
			case GameState.Win:
				break;
			case GameState.Lose:
				break;
		}
	}

	public void AddPowerLevelChangeListener(Action<int> listener) {
		_timer.AddOnPowerLevelChangeListener(listener);
	}
	public void RemoveOnPowerLevelChangeListener(Action<int> listener) {
		_timer.RemoveOnPowerLevelChangeListener(listener);
	}
}
