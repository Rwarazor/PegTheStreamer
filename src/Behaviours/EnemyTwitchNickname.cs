using TMPro;
using UnityEngine;

namespace PegTheStreamer.Behaviours {
    class EnemyTwitchNickname : MonoBehaviour {

        protected bool _holdingName = false;
        protected string _name;
        protected TextMeshProUGUI _text;

        protected virtual void Awake() {
            if (TwitchNamesPoolManager.Instance.TryFetchName(out _name)) {
                _holdingName = true;
                _text = Utils.CreateText(this.transform, _name, new Vector3(0, -0.075f, 0));

                _text.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                _text.verticalAlignment = TMPro.VerticalAlignmentOptions.Geometry;
                _text.fontSizeMin = 0.4f;
                _text.fontSizeMax = 0.6f;
                _text.maxVisibleLines = 1;
                _text.overflowMode = TMPro.TextOverflowModes.Truncate;
                _text.enableAutoSizing = true;

                // Setting text width to a little less than healthbar width
                Vector2 newSizeDelta = (_text.gameObject.transform as RectTransform).sizeDelta;
                newSizeDelta.x = (this.gameObject.transform as RectTransform).sizeDelta.x - 0.2f;
                (_text.gameObject.transform as RectTransform).sizeDelta = newSizeDelta;

                _text.color = new Color(1, 1, 0.8f, 1);
            }
        }

        void OnDisable() {
            if (_holdingName) {
                TwitchNamesPoolManager.Instance.FreeName(_name);
            }
        }
    }

    class BossTwitchNickname : EnemyTwitchNickname {
        protected override void Awake() {
            base.Awake();
            if (_holdingName) {
                _text.transform.localPosition = new Vector3(0.5f, 1.23f, 0);
                _text.fontSizeMin = 0.6f;
                _text.fontSizeMax = 0.8f;
            }
        }
    }
}
