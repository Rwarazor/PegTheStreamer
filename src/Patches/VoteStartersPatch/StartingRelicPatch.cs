using HarmonyLib;
using PegTheStreamer.Behaviours;

namespace PegTheStreamer.Patches.VoteStartersPatch {
    [HarmonyPatch(typeof(GameInit))]
    class StartingRelicPatch {
        [HarmonyPatch("Start")]
        public static void Postfix(GameInit __instance) {
            if (!PTSSettingsManager.EnableMod || !PTSSettingsManager.EnableVoteStarting) { return; }
            __instance.gameObject.AddComponent(typeof(Behaviours.VoteStarters.StartingRelic));
        }

        [HarmonyPatch("AddRelic")]
        [HarmonyPrefix]
        public static void PickInterrupt(GameInit __instance) {
            VoteManager.Instance.InterruptVote();
        }

        [HarmonyPatch("SkipRelic")]
        [HarmonyPrefix]
        public static void SkipInterrupt(GameInit __instance) {
            VoteManager.Instance.InterruptVote();
        }
    }
}
