using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class ButtonColor : MonoBehaviour {
	private Button _button;
	private bool _colorButton;
	[SerializeField] private float _saturation = 0.3f;
	[SerializeField] private bool _colorOnActive = false;
	private void Start() {
		_button = GetComponent<Button>();
	}
	public void Update() {
		var buttonColors = _button.colors;
		Color.RGBToHSV(_colorOnActive ? buttonColors.normalColor : buttonColors.disabledColor, out var h, out var s, out var v);
		s = _saturation;
		v = 1;
		if (h + (1f / 360f) > 1) {
			h = (1f / 360f);
		} else {
			h += (1f / 360f);
		}
		if (_colorOnActive) {
			buttonColors.normalColor = Color.HSVToRGB(h, s, v);
		} else {
			buttonColors.disabledColor = Color.HSVToRGB(h, s, v);
		}
		_button.colors = buttonColors;
	}
}
