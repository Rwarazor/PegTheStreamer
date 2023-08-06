using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteStarters {
    public abstract class VoteStarter : MonoBehaviour {

        public bool ShouldStartVote { get { return _shouldStartVote; } set { _shouldStartVote = value; } }
        bool _shouldStartVote = true;

        private int framesPassed = 0;

        void LateUpdate() {
            if (ShouldStartVote && framesPassed > 0 && StartVote()) {
                ShouldStartVote = false;
            }
            ++framesPassed;
        }

        void OnDisable() {
            VoteManager.Instance.InterruptVote();
        }
        abstract protected bool StartVote();
    }
}
