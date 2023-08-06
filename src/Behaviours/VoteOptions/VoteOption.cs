using PegTheStreamer.Behaviours.VoteStarters;
using System;
using TMPro;
using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteOptions {
    public abstract class VoteOption : MonoBehaviour, IComparable<VoteOption> {

        public void Awake() {
            SetupCountText();
            HideCountText();
        }

        public int Votes { get; set; }

        public void SetCountText(string str) {
            _voteCountText.text = str;
        }
        public abstract void Choose();

        public abstract string GetOptionTextForChat();

        public void ShowCountText() {
            if (_voteCountText) {
                _voteCountText.enabled = true;
            }
        }

        public void HideCountText() {
            if (_voteCountText) {
                _voteCountText.enabled = false;
            }
        }

        protected abstract void SetupCountText();

        public abstract int GetByTypeNamingPriority();
        public abstract int GetInTypeNamingPriority();

        public int CompareTo(VoteOption other) {
            if (GetByTypeNamingPriority() < other.GetByTypeNamingPriority()) { return -1; }
            if (GetByTypeNamingPriority() > other.GetByTypeNamingPriority()) { return 1; }
            return GetInTypeNamingPriority().CompareTo(other.GetInTypeNamingPriority());
        }

        protected TextMeshProUGUI _voteCountText;
    }

    public abstract class BattleUpgradeVoteOption : VoteOption {

        public PostBattleUpgrades voteStarter;

        protected void NextVote() {
            voteStarter.TimeOut = 2;
            voteStarter.ShouldStartVote = true;
        }
    }
}
