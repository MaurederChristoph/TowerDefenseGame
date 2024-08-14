using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour {
	[SerializeField] private TMP_Text _infoNumber;
	[SerializeField] private Slider _slider;
	[field:SerializeField] public BarType BarType { get; private set; }

	private int _maxValue;
	public void UpdateMaxValue(int newValueHeath, float currentValue) {
		_maxValue = newValueHeath;
		_slider.maxValue = _maxValue;
		UpdateCurrentValue(currentValue);
	}

	public void UpdateCurrentValue(float currentValue) {
		_slider.value = (int)currentValue;
		if (_infoNumber == null) return;
		_infoNumber.text = $"{(int)currentValue}/{_maxValue}";
	}
}
