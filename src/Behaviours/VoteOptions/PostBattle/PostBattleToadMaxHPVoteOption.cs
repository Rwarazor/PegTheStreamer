using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteOptions {
    public class PostBattleToadMaxHPVoteOption : BattleUpgradeVoteOption {
        public override void Choose() {
            Utils.EnsureGameUnpaused();
            Utils.EnsureInventoryClosed();
            Utils.EnsurePostBattleUpgradesMainWindowOpen();
            Utils.PressButton(this.gameObject);
            NextVote();
            this.voteStarter.didToad = true;
        }

        public override int GetByTypeNamingPriority() {
            return 4;
        }

        public override int GetInTypeNamingPriority() {
            return 0;
        }

        public override string GetOptionTextForChat() {
            return "+Max HP";
        }

        protected override void SetupCountText() {
            this._voteCountText = Utils.CreateText(gameObject.transform, "postbattle toad max hp", new Vector3(313, -45, 0));
            this._voteCountText.fontSize = 28;
        }
    }
}
