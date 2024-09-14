using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Represents a info bar 
/// </summary>
public class InfoBar : MonoBehaviour {
    /// <summary>
    /// Shows the info number 
    /// </summary>
    [SerializeField] private TMP_Text _infoNumber;
    /// <summary>
    /// Represents the slider
    /// </summary>
    [SerializeField] private Slider _slider;
    /// <summary>
    /// The type of bar it is
    /// </summary>
    [field: SerializeField] public BarType BarType { get; private set; }
    /// <summary>
    /// The max value of the slider 
    /// </summary>
    private int _maxValue;
    /// <summary>
    /// Updates the max value of the slider
    /// </summary>
    /// <param name="newValueHeath">The new Max Hp</param>
    /// <param name="currentValue">The new current HP</param>
    public void UpdateMaxValue(int newValueHeath, float currentValue) {
        _maxValue = newValueHeath;
        _slider.maxValue = _maxValue;
        UpdateCurrentValue(currentValue);
    }
    /// <summary>
    /// Updates the current Value of the slider
    /// </summary>
    /// <param name="currentValue">The new value</param>
    public void UpdateCurrentValue(float currentValue) {
        _slider.value = (int)currentValue;
        if(_infoNumber == null) return;
        _infoNumber.text = $"{(int)currentValue}/{_maxValue}";
    }
}
