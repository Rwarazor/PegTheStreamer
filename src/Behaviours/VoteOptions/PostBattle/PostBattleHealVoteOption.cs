using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteOptions {
    public class PostBattleHealVoteOption : BattleUpgradeVoteOption {
        public override void Choose() {
            Utils.EnsureGameUnpaused();
            Utils.EnsureInventoryClosed();
            Utils.EnsurePostBattleUpgradesMainWindowOpen();
            Utils.PressButton(this.gameObject);
            NextVote();
            this.voteStarter.didHeal = true;
        }

        public override int GetByTypeNamingPriority() {
            return 3;
        }

        public override int GetInTypeNamingPriority() {
            return 0;
        }

        public override string GetOptionTextForChat() {
            return "Heal";
        }

        protected override void SetupCountText() {
            this._voteCountText = Utils.CreateText(gameObject.transform, "postbattle heal", new Vector3(270, -10, 0));
        }
    }
}
