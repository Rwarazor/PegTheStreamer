using TMPro;
using UnityEngine;

namespace PegTheStreamer {
    public static partial class Utils {
        public static TextMeshProUGUI CreateText(Transform parent, string text, Vector3 localPosition) {
            GameObject obj = new GameObject("Text");
            TextMeshProUGUI tmpro = obj.AddComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
            // SetParent after add component, to make sure TextMeshProUGUI.Awake runs and default settings
            // are applied so that our own settings won't be overwritten once we enable text object
            obj.transform.SetParent(parent, false);
            obj.transform.localPosition = localPosition;
            obj.transform.localScale = Vector3.one;
            tmpro.text = text;
            return tmpro;
        }

        public static TextMeshProUGUI CreateVoteTimerText(Transform parent, Vector3 localPosition) {
            var ans = CreateText(parent, "VOTE NOW: 99s left", localPosition);
            ans.enabled = false;
            return ans;
        }
    }
}
