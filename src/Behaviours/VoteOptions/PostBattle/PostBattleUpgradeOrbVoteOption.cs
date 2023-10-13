using PeglinUI.PostBattle;
using UnityEngine;
using UnityEngine.UI;

namespace PegTheStreamer.Behaviours.VoteOptions {
    public class PostBattleUpgradeOrbVoteOption : BattleUpgradeVoteOption {
        public int OrbIndex { get; set; }
        public override void Choose() {
            Utils.EnsureGameUnpaused();
            Utils.EnsureInventoryClosed();
            Utils.PressButton(this.gameObject);
            Transform trans = GameObject.Find("BattleUpgradesCanvas").transform.Find("Container/UpgradeConfirmationPanel OrbUpgrade/Options And Navigation/ButtonContainer");
            foreach (Transform child in trans) {
                if (child.Find("UpgradeButton") != null) {
                    (child.Find("UpgradeButton").GetComponent(typeof(Button)) as Button).Invoke("Press", 0f);
                }
            }
            NextVote();
        }

        public override int GetByTypeNamingPriority() {
            return 0;
        }

        public override int GetInTypeNamingPriority() {
            return OrbIndex;
        }

        public override string GetOptionTextForChat() {
            return (gameObject.GetComponent(typeof(UpgradeOption)) as UpgradeOption).attack.Name;
        }

        protected override void SetupCountText() {
            this._voteCountText = Utils.CreateText(gameObject.transform, "postbattle upgrade orb", new Vector3(51, -57, 0));
        }
    }
}
