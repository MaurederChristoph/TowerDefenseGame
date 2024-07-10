using System;

/// <summary>
/// Handles the game flow
/// </summary>
public class GameManager : Singleton<GameManager> {
    public GameState CurrentGameState { get; private set; }

    /// <summary>
    /// Reference to the current instance of the enemy manager
    /// </summary>
    public EnemyManager EnemyManager { get; private set; }

    /// <summary>
    /// Reference to the current instance of the Delayed Action Handler
    /// </summary>
    public DelayedActionHandler DelayedActionHandler { get; private set; }

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

