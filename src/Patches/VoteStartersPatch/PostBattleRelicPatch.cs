using HarmonyLib;
using PeglinUI.PostBattle;
using PegTheStreamer.Behaviours;
using PegTheStreamer.Behaviours.VoteStarters;
using Relics;

namespace PegTheStreamer.Patches.VoteStarters {
    [HarmonyPatch(typeof(BattleUpgradeCanvas))]
    class PostBattleRelicPatch {
        [HarmonyPatch("SetupRelicGrant")]
        [HarmonyPostfix]
        public static void SetupRelicGrant(BattleUpgradeCanvas __instance, RelicRarity rarity, bool isTreasure = false) {
            if (!PTSSettingsManager.EnableMod) { return; }
            VoteManager.Instance.InterruptVote();
            PostBattleRelic.RelicTypeId relicType = PostBattleRelic.RelicTypeId.NONE;
            if (isTreasure && PTSSettingsManager.EnableVoteChest) {
                relicType = PostBattleRelic.RelicTypeId.CHEST;
            }
            if (!isTreasure && rarity == RelicRarity.RARE && PTSSettingsManager.EnableVoteElite) {
                relicType = PostBattleRelic.RelicTypeId.ELITE;
            }
            if (!isTreasure && rarity == RelicRarity.BOSS && PTSSettingsManager.EnableVoteBoss) {
                relicType = PostBattleRelic.RelicTypeId.BOSS;
            }
            if (relicType != PostBattleRelic.RelicTypeId.NONE) {
                PostBattleRelic comp = __instance.gameObject.AddComponent(typeof(PostBattleRelic)) as PostBattleRelic;
                comp.RelicType = relicType;
            }
        }
    }
}
