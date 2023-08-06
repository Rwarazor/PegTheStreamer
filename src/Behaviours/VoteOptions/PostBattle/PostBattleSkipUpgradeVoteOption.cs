using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteOptions {
    public class PostBattleSkipUpgradeVoteOption : VoteOption {
        public override void Choose() {
            //(gameObject.GetComponent(typeof(UpgradeOption)) as UpgradeOption).OnClick();
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
            this._voteCountText = Utils.CreateText(gameObject.transform, "postbattle skip", new Vector3(170, -8, 0));
        }
    }
}
