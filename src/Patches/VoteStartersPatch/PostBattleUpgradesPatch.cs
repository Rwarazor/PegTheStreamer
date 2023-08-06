using Currency;
using HarmonyLib;
using PeglinUI.PostBattle;
using PegTheStreamer.Behaviours;
using PegTheStreamer.Behaviours.VoteStarters;
using Relics;
using UnityEngine;

namespace PegTheStreamer.Patches.VoteStarters {

    [HarmonyPatch(typeof(BattleUpgradeCanvas))]
    class PostBattleUpgradesPatch {
        [HarmonyPatch("SetUpPostBattleOptions")]
        [HarmonyPostfix]
        public static void PostBattleUpgrades(BattleUpgradeCanvas __instance, DeckManager ____deckManager) {
            if (PTSSettingsManager.EnableMod && PTSSettingsManager.EnableVotePostbattle) {
                (__instance.gameObject.AddComponent(typeof(PostBattleUpgrades)) as PostBattleUpgrades).deckManager = ____deckManager;
            }
        }
    }

    [HarmonyPatch(typeof(CurrencyManager))]
    class PostBattleUpgradesInterruptsPatch {
        [HarmonyPatch("RemoveGold")]
        [HarmonyPrefix]
        public static void RemoveGold(BattleUpgradeCanvas __instance) {
            VoteManager.Instance.InterruptVote();
        }
    }
}
