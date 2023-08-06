using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteOptions {
    class PostBattleSkipRelicVoteOption : VoteOption {


        public override void Choose() {
            Utils.EnsureGameUnpaused();
            Utils.EnsureInventoryClosed();
            Utils.PressButton(this.gameObject);
        }

        public override int GetByTypeNamingPriority() {
            return 0;
        }

        public override int GetInTypeNamingPriority() {
            return 0;
        }

        public override string GetOptionTextForChat() {
            return "Skip";
        }

        protected override void SetupCountText() {
            this._voteCountText = Utils.CreateText(gameObject.transform, "skip relic", new Vector3(45, 22, 0));
        }
    }
}
