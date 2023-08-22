using HarmonyLib;
using PeglinUI;
using PegTheStreamer.Behaviours;

namespace PegTheStreamer.Patches {
    [HarmonyPatch(typeof(GameInit))]
    class ClearStartingDeck {
        [HarmonyPatch("Start")]
        public static void Prefix(GameInit __instance, LoadMapData ___LoadData) {
            if (___LoadData.NewGame && PTSSettingsManager.EnableMod && PTSSettingsManager.DoSlayTheStreamerStart) {
                StaticGameData.StartingOrbs = null;
                STSStartManager.Instance.state = STSStartManager.STSState.SHOULD_START;
            } else {
                STSStartManager.Instance.state = STSStartManager.STSState.DISABLED;
            }
        }
    }

    [HarmonyPatch(typeof(BattleController))]
    class StopBattleUntilDeckChosen {
        [HarmonyPatch("Update")]
        public static bool Prefix() {
            if (STSStartManager.Instance.state == STSStartManager.STSState.SHOULD_START || STSStartManager.Instance.state == STSStartManager.STSState.STARTED) {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(InventoryViewController))]
    class UseInventoryViewAsTemplate {
        [HarmonyPatch("OnEnable")]
        public static void Postfix() {
            if (STSStartManager.Instance.state == STSStartManager.STSState.SHOULD_START) {
                STSStartManager.Instance.SetupStartingDeckChoice();
            }
        }
    }
}
