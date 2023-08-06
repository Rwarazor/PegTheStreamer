using TMPro;
using UnityEngine;

namespace PegTheStreamer.Behaviours.Settings {
	public class SettingString : Setting<string> {
		private string _placeholderText;
		private Color _textColor;

		public TMP_InputField UIText;

		public SettingString(string prefsKey, string description, string defaultValue, string placeholder, Color color, Color textColor) : base(prefsKey, description, defaultValue, color) {
			_placeholderText = placeholder;
			_textColor = textColor;
		}

		public override void LoadValue() {
			this.Value = PlayerPrefs.GetString(_playerPrefsKey, _defaultValue);
		}

		public override void SaveValue() {
			PlayerPrefs.SetString(_playerPrefsKey, Value);
		}

		public override void AddApplyAction() {
			UIText.onDeselect.AddListener(ApplyAction);
			UIText.onSubmit.AddListener(ApplyAction);
		}

		public override void RemoveApplyAction() {
			UIText.onDeselect.RemoveListener(ApplyAction);
			UIText.onSubmit.RemoveListener(ApplyAction);
		}

		public override void ShowValue() {
			UIText.text = Value;
		}

		public override void CreateUI() {
			UIText = Utils.CreateInputFieldOption(_description, _placeholderText, _backgroundColor, _textColor);
		}
	}
}
