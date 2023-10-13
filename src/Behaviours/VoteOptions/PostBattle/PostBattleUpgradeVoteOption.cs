using PegTheStreamer.Behaviours.VoteStarters;
using UnityEngine;
using UnityEngine.UI;

namespace PegTheStreamer.Behaviours.VoteOptions {
    class PostBattleUpgradeVoteOption : VoteOption {
        public PostBattleUpgrades voteStarter;
        public DeckManager deckManager;

        public override void Choose() {
            Utils.EnsureGameUnpaused();
            Utils.EnsureInventoryClosed();
            Utils.PressButton(this.gameObject);
            var nextVoteStarter = GameObject.Find("BattleUpgradesCanvas").AddComponent(typeof(PostBattleOrbUpgrade)) as PostBattleOrbUpgrade;
            nextVoteStarter.parentVoteStarter = voteStarter;
            nextVoteStarter.deckManager = deckManager;
        }

        public override int GetByTypeNamingPriority() {
            return 2;
        }

        public override int GetInTypeNamingPriority() {
            return 0;
        }

        public override string GetOptionTextForChat() {
            return "Upgrade orb";
        }

        protected override void SetupCountText() {
            this._voteCountText = Utils.CreateText(gameObject.transform, "postbattle heal", new Vector3(65, -57, 0));
        }
    }
}
