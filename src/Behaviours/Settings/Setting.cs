using UnityEngine;

namespace PegTheStreamer.Behaviours.Settings {
	public abstract class SettingBase {
		public abstract void LoadValue();

		public abstract void SaveValue();

		public abstract void RevertValue();

		public abstract void InitializePreviousValue();

		public abstract void AddApplyAction();

		public abstract void RemoveApplyAction();

		public abstract void ShowValue();

		public abstract void CreateUI();
	}

	public abstract class Setting<T> : SettingBase {
		public T Value { get; set; }
		protected T _lastValue;

		protected T _defaultValue;
		protected string _playerPrefsKey;
		protected string _description;
		protected Color _backgroundColor;

		public static implicit operator T(Setting<T> obj) {
			return obj.Value;
		}

		protected Setting(string prefsKey, string description, T defaultValue, Color color) {
			_playerPrefsKey = prefsKey;
			_description = description;
			_defaultValue = defaultValue;
			_backgroundColor = color;
		}

		public override void RevertValue() {
			Value = _lastValue;
		}

		public override void InitializePreviousValue() {
			_lastValue = Value;
		}

		protected virtual void ApplyAction(T value) {
			Value = value;
		}
	}
}
