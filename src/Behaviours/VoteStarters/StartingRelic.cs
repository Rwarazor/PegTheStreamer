using PegTheStreamer.Behaviours.VoteOptions;
using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteStarters {
    public class StartingRelic : VoteStarter {
        protected override bool StartVote() {
            if (!TwitchManager.Instance.EnsureConnected()) { return false; }
            Debug.Log("Vote for starting relic");


            Transform canvas = GameObject.Find("Camera").transform.Find("Canvas");
            Transform container = canvas.Find("HorizontalContainer");

            VoteManager voteManager = VoteManager.Instance;
            voteManager.CleanVote();

            int i = 0;
            foreach (Transform child in container) {
                if (child.gameObject.activeInHierarchy) {
                    StartingRelicVoteOption option = child.gameObject.AddComponent(typeof(StartingRelicVoteOption)) as StartingRelicVoteOption;
                    option.RelicIndex = i++;
                    voteManager.AddVoteOption(option);
                }
            }

            if (PTSSettingsManager.AllowSkipStarting) {
                SkipStartingVoteOption skipStarting = canvas.Find("SkipButton").gameObject.AddComponent(typeof(SkipStartingVoteOption)) as SkipStartingVoteOption;
                voteManager.AddVoteOption(skipStarting);
            }

            voteManager.VoteTimerText = Utils.CreateVoteTimerText(canvas, new Vector3(15, 120, 0));
            voteManager.StartVote();
            return true;
        }
    }
}
