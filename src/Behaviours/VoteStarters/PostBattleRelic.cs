using PegTheStreamer.Behaviours.Settings;
using PegTheStreamer.Behaviours.VoteOptions;
using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteStarters {
    public class PostBattleRelic : VoteStarter {

        public RelicTypeId RelicType { get; set; }
        protected override bool StartVote() {
            if (!TwitchManager.Instance.EnsureConnected()) { return false; }

            Transform containerUp = transform.Find("Container/UpgradePanel/Image/RelicContainer");
            Transform containerDown = containerUp.Find("RelicContainer");

            VoteManager voteManager = VoteManager.Instance;
            voteManager.CleanVote();

            int i = 0;
            foreach (Transform child in containerDown) {
                if (child.gameObject.activeInHierarchy) {
                    PostBattleRelicVoteOption option = child.gameObject.AddComponent(typeof(PostBattleRelicVoteOption)) as PostBattleRelicVoteOption;
                    option.RelicIndex = i++;
                    voteManager.AddVoteOption(option);
                }
            }

            SettingBoolean targetSkipSetting;

            switch (RelicType) {
                default:
                case RelicTypeId.CHEST:
                    Debug.Log("Vote for chest relic");
                    targetSkipSetting = PTSSettingsManager.AllowSkipChest;
                    break;
                case RelicTypeId.ELITE:
                    Debug.Log("Vote for elite relic");
                    targetSkipSetting = PTSSettingsManager.AllowSkipElite;
                    break;
                case RelicTypeId.BOSS:
                    Debug.Log("Vote for boss relic");
                    targetSkipSetting = PTSSettingsManager.AllowSkipBoss;
                    break;
            }

            if (targetSkipSetting) {
                PostBattleSkipRelicVoteOption skipOption = containerUp.Find("SkipButton").gameObject.AddComponent(typeof(PostBattleSkipRelicVoteOption)) as PostBattleSkipRelicVoteOption;
                voteManager.AddVoteOption(skipOption);
            }

            voteManager.VoteTimerText = Utils.CreateVoteTimerText(transform.Find("Container"), new Vector3(15, 228, 0));
            voteManager.StartVote();
            return true;
        }

        public enum RelicTypeId {
            NONE,
            CHEST,
            ELITE,
            BOSS
        }
    }
}
