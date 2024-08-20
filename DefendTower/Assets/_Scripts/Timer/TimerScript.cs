using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Handles the power progression of the game
/// </summary>
public class TimerScript : MonoBehaviour {

    /// <summary>
    /// The timer display
    /// </summary>
    [SerializeField] private TMP_Text _timerText;
    /// <summary>
    /// Current power level
    /// </summary>
    [field: SerializeField] public int Level { get; private set; } = 1;

    /// <summary>
    /// Total time of the game
    /// </summary>
    private float _timeRemaining = 900f;
    /// <summary>
    /// first instance of new power level
    /// </summary>
    private float _nextLevelTime = 840f;

    /// <summary>
    /// Will be called if the power level changes
    /// </summary>
    private Action<int> _onPowerLevelChange;

    private void Start() {
        _onPowerLevelChange?.Invoke(1);
    }
    public void Update() {
        if(_timeRemaining > 0) {
            _timeRemaining -= Time.deltaTime;
            if(_timeRemaining <= _nextLevelTime) {
                Level++;
                _onPowerLevelChange?.Invoke(Level);
                _nextLevelTime -= 60f;
            }
            DisplayTime(_timeRemaining);
        } else {
            _timeRemaining = 0;
            DisplayTime(_timeRemaining);
            _onPowerLevelChange?.Invoke(16);
        }
    }

    /// <summary>
    /// Updates the displayed time
    /// </summary>
    /// <param name="timeToDisplay">The new time</param>
    public void DisplayTime(float timeToDisplay) {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
    /// <summary>
    /// Adds a listener to the power level change event
    /// </summary>
    /// <param name="listener">The listener that will be added</param>
    public void AddOnPowerLevelChangeListener(Action<int> listener) {
        _onPowerLevelChange += listener;
    }
    /// <summary>
    /// Removes a listener to the power level change event
    /// </summary>
    /// <param name="listener">The listener that will be removed</param>
    public void RemoveOnPowerLevelChangeListener(Action<int> listener) {
        _onPowerLevelChange -= listener;
    }
}
