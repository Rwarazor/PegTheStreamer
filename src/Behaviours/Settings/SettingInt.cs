using PeglinUI.SettingsMenu.Options;
using System;
using UnityEngine;

namespace PegTheStreamer.Behaviours.Settings {
	using IntAction = CarouselOptionsInt.IntAction;
	public class SettingInt : Setting<int> {

		private int[] _values;

		CarouselOptionsInt UIOption;

		public SettingInt(string prefsKey, string description, int defaultValue, int[] values, Color color) : base(prefsKey, description, defaultValue, color) {
			_values = values;
		}

		public override void LoadValue() {
			this.Value = PlayerPrefs.GetInt(_playerPrefsKey, _defaultValue);
		}

		public override void SaveValue() {
			PlayerPrefs.SetInt(_playerPrefsKey, Value);
		}

		protected override void ApplyAction(int value) {
			Value = _values[value];
		}

		public override void AddApplyAction() {
			UIOption.changeSettingAction = (IntAction)Delegate.Combine(UIOption.changeSettingAction, new IntAction(ApplyAction));
		}

		public override void RemoveApplyAction() {
			UIOption.changeSettingAction = (IntAction)Delegate.Remove(UIOption.changeSettingAction, new IntAction(ApplyAction));
		}

		public override void ShowValue() {
			UIOption.ShowOption(Array.FindIndex(_values, val => val == Value));
		}

		public override void CreateUI() {
			string[] valuesStrings = new string[_values.Length];
			for (int i = 0; i < _values.Length; ++i) {
				valuesStrings[i] = _values[i].ToString();
			}

			UIOption = Utils.CreateIntOption(_description, _backgroundColor, valuesStrings);
		}
	}
}
