using PeglinUI.SettingsMenu.Options;
using System;
using UnityEngine;

namespace PegTheStreamer.Behaviours.Settings {
	using BooleanAction = CarouselOptionsBoolean.BooleanAction;
	public class SettingBoolean : Setting<bool> {
		CarouselOptionsBoolean UIOption;

		public SettingBoolean(string prefsKey, string description, bool defaultValue, Color color) : base(prefsKey, description, defaultValue, color) {
		}

		public override void LoadValue() {
			this.Value = PlayerPrefs.GetInt(_playerPrefsKey, _defaultValue ? 1 : 0) == 1;
		}

		public override void SaveValue() {
			PlayerPrefs.SetInt(_playerPrefsKey, Value ? 1 : 0);
		}

		public override void AddApplyAction() {
			UIOption.changeSettingAction = (BooleanAction)Delegate.Combine(UIOption.changeSettingAction, new BooleanAction(ApplyAction));
		}

		public override void RemoveApplyAction() {
			UIOption.changeSettingAction = (BooleanAction)Delegate.Remove(UIOption.changeSettingAction, new BooleanAction(ApplyAction));
		}

		public override void ShowValue() {
			UIOption.ShowOption(Value);
		}

		public override void CreateUI() {
			UIOption = Utils.CreateBinaryOption(_description, _backgroundColor);
		}
	}
}
