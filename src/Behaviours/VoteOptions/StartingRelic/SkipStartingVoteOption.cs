using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteOptions {
    class SkipStartingVoteOption : VoteOption {
        public override void Choose() {
            GameInit comp = GameObject.Find("GameInit").GetComponent(typeof(GameInit)) as GameInit;
            comp.SkipRelic();
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
