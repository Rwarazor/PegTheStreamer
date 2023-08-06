using UnityEngine;
using UnityEngine.UI;

namespace PegTheStreamer.Behaviours.VoteOptions {
    class PostBattleRelicVoteOption : VoteOption {

        public int RelicIndex { get; set; }

        public override void Choose() {
            Utils.EnsureGameUnpaused();
            Utils.EnsureInventoryClosed();
            Utils.PressButton(this.gameObject);
            Transform trans = GameObject.Find("BattleUpgradesCanvas").transform.Find("Container/UpgradeConfirmationPanel Relic/Options And Navigation/ButtonContainer");
            foreach (Transform child in trans) {
                if (child.Find("UpgradeButton") != null) {
                    (child.Find("UpgradeButton").GetComponent(typeof(Button)) as Button).Invoke("Press", 0f);
                }
            }
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
            this._voteCountText = Utils.CreateText(gameObject.transform, "chest relic", new Vector3(44, -93, 0));
        }
    }
}
