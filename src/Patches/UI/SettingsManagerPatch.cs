using HarmonyLib;
using PegTheStreamer.Behaviours;

namespace PegTheStreamer.Patches {
    [HarmonyPatch(typeof(SettingsManager))]
    class SettingsManagerPatch {
        [HarmonyPatch("ApplySettings")]
        [HarmonyPostfix]
        public static void ApplySettingsPostfix(SettingsManager __instance) {
            PTSSettingsManager.Instance.ApplySettings();
        }

        [HarmonyPatch("RevertSettings")]
        [HarmonyPrefix]
        public static void RevertSettingsPrefix(SettingsManager __instance) {
            PTSSettingsManager.Instance.RevertSettings();
        }
    }
}
