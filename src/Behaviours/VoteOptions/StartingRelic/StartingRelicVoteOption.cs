using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteOptions {
    class StartingRelicVoteOption : VoteOption {

        public int RelicIndex { get; set; }

        public override void Choose() {
            GameInit comp = GameObject.Find("GameInit").GetComponent(typeof(GameInit)) as GameInit;
            comp.ChooseRelic(RelicIndex);
            comp.AddRelic();
        }

        public override int GetByTypeNamingPriority() {
            return 1;
        }

        public override int GetInTypeNamingPriority() {
            return RelicIndex;
        }

        public override string GetOptionTextForChat() {
            return (transform.Find("Image").GetComponent(typeof(RelicIcon)) as RelicIcon).relic.englishDisplayName;
        }

        protected override void SetupCountText() {
            this._voteCountText = Utils.CreateText(gameObject.transform, "starting relic", new Vector3(44, -93, 0));
        }
    }
}
