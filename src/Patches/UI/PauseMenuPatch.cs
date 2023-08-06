using HarmonyLib;
using UnityEngine;
using PegTheStreamer.Behaviours;
using PeglinUI.SettingsMenu.Options;
using TMPro;

namespace PegTheStreamer.Patches {
    [HarmonyPatch(typeof(PauseMenu))]
    class PauseMenuPatch {
        // Patching options inside PauseMenu, because at this point OptionsMenu is not active
        // and we can attach components to it, and their OnEnable function won't be called
        [HarmonyPatch("Awake")]
        public static void Postfix(PauseMenu __instance) {
            EnsureOptions(__instance.transform.Find("NewOptionsPanel").gameObject);
        }

        public static void EnsureOptions(GameObject optionsMenu) {
            if (optionsMenu?.GetComponent(typeof(OptionsCertificate)) != null) return;
            optionsMenu.AddComponent(typeof(OptionsCertificate));

            // removing triggers from keyboard, so that you can actually type login and OAuth
            GameObject.Destroy(optionsMenu.transform.Find("ClickableButtonAccept").GetComponent(typeof(RewiredUIButtonTrigger)));
            GameObject.Destroy(optionsMenu.transform.Find("ClickableButtonApply").GetComponent(typeof(RewiredUIButtonTrigger)));
            GameObject.Destroy(optionsMenu.transform.Find("ClickableButtonCancel").GetComponent(typeof(RewiredUIButtonTrigger)));

            PTSSettingsManager.Instance.CreateUI();

            PTSSettingsManager.TwitchOAuth.UIText.inputType = TMP_InputField.InputType.Password;
        }
    }
}
