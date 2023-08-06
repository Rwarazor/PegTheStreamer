using PegTheStreamer.Behaviours.VoteOptions;
using UnityEngine;
using UnityEngine.UI;

namespace PegTheStreamer.Behaviours.VoteStarters {
    public class PostBattleOrbUpgrade : VoteStarter {

        public PostBattleUpgrades parentVoteStarter;
        public DeckManager deckManager;
        protected override bool StartVote() {
            if (!TwitchManager.Instance.EnsureConnected()) { return false; }

            Transform container = transform.Find("Container/UpgradePanel/Image/BallInventoryView/Scroll View/Viewport/OrbGrid");

            if (deckManager.GetUpgradeableOrbs().Count == 1) {
                PostBattleUpgradeOrbVoteOption option = container.GetChild(0).gameObject.AddComponent(typeof(PostBattleUpgradeOrbVoteOption)) as PostBattleUpgradeOrbVoteOption;
                option.OrbIndex = 0;
                option.voteStarter = parentVoteStarter;
                option.Choose();
                return true;
            }

            VoteManager voteManager = VoteManager.Instance;
            voteManager.CleanVote();

            int i = 0;
            foreach (Transform child in container) {
                if (child.gameObject.activeInHierarchy) {
                    PostBattleUpgradeOrbVoteOption option = child.gameObject.AddComponent(typeof(PostBattleUpgradeOrbVoteOption)) as PostBattleUpgradeOrbVoteOption;
                    option.OrbIndex = i++;
                    option.voteStarter = parentVoteStarter;
                    voteManager.AddVoteOption(option);
                }
            }

            voteManager.VoteTimerText = Utils.CreateVoteTimerText(gameObject.transform.Find("Container"), new Vector3(15, 228, 0));

            // disable back button, so nothing blows up (it will blow up if this inventory is closed)
            (transform.Find("Container/UpgradePanel/Image/BallInventoryView/CloseButton").GetComponent(typeof(Button)) as MonoBehaviour).enabled = false;

            voteManager.ChangeOddity();
            voteManager.StartVote();

            return true;
        }
    }
}
