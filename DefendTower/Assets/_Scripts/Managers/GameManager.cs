using System;
using UnityEngine;

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

    /// <summary>
    /// Event triggered when the game state changes
    /// </summary>
    public Action<GameState> OnGameStateChange;

    private void Start() => ChangeState(GameState.SpawnMap);

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
        OnGameStateChange?.Invoke(gameState);
    }
}
