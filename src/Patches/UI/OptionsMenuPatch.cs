using HarmonyLib;
using PeglinUI.SettingsMenu;
using PegTheStreamer.Behaviours;

namespace PegTheStreamer.Patches {
    [HarmonyPatch(typeof(OptionsMenu))]
    class OptionsMenuPatch {
        [HarmonyPatch("OnEnable")]
        [HarmonyPostfix]
        public static void OnEnable() {
            PTSSettingsManager.Instance.ShowSettings();
            PTSSettingsManager.Instance.AddApplyActions();
        }

        [HarmonyPatch("OnDisable")]
        [HarmonyPrefix]
        public static void OnDisable() {
            PTSSettingsManager.Instance.RemoveApplyActions();
        }
    }
}
