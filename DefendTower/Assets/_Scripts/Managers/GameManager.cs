using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Handles the game flow
/// </summary>
public class GameManager : Singleton<GameManager> {
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

    /// <summary>
    /// Reference to the current instance of the timer
    /// </summary>
    [field: SerializeField] private TimerScript _timer;

    /// <summary>
    /// Reference to the win screen gameobject 
    /// </summary>
    [field: SerializeField] private GameObject _winScreen;

    /// <summary>
    /// Represents if the game can be ended
    /// </summary>
    private bool _canWin = false;

    /// <summary>
    /// The current power level of the game
    /// </summary>
    public int CurrentPowerLevel => _timer.Level;

    private void Start() {
        _timer.AddOnPowerLevelChangeListener(WinConCheck);
        Screen.SetResolution(1920, 1080, true);
    }

    /// <summary>
    /// Checks if the power level is appropriate to be able to win
    /// </summary>
    /// <param name="powerLevel">The current power level</param>
    private void WinConCheck(int powerLevel) {
        if(powerLevel == 16) {
            _canWin = true;
        }
    }

    private void Update() {
        if(_canWin && EnemyManager.Enemies.Count == 0) {
            _winScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Adds a listener to the event that is called when the power level changes
    /// </summary>
    /// <param name="listener">The listener that will be added</param>
    public void AddPowerLevelChangeListener(Action<int> listener) {
        _timer.AddOnPowerLevelChangeListener(listener);
    }
    /// <summary>
    /// Removes a listener to the event that is called when the power level changes
    /// </summary>
    /// <param name="listener">The listener that will be removed</param>
    public void RemoveOnPowerLevelChangeListener(Action<int> listener) {
        _timer.RemoveOnPowerLevelChangeListener(listener);
    }
}
