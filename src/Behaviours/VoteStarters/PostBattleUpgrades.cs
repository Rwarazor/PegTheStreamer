using Currency;
using PegTheStreamer.Behaviours.VoteOptions;
using TMPro;
using UnityEngine;

namespace PegTheStreamer.Behaviours.VoteStarters {
    public class PostBattleUpgrades : VoteStarter {

        int votesStarted = 0;

        public float TimeOut { set { _start_time = Time.realtimeSinceStartup + value; } }

        private float _start_time = 0;

        public bool didHeal = false;
        public bool didToad = false;
        public DeckManager deckManager;

        public static bool CanAffordNewOrb(GameObject orb) {
            TextMeshProUGUI comp = orb.transform.Find("Gold Container")?.Find("Gold Text")?.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
            if (comp == null) { return false; }
            int value = int.Parse(comp.text);
            return CurrencyManager.Instance.CanAfford(value);
        }

        public static bool CanAffordHeal(GameObject healButton) {
            TextMeshProUGUI comp = healButton.transform.Find("Gold Container (1)")?.Find("Gold Text")?.GetComponent(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
            if (comp == null) { return false; }
            int value = int.Parse(comp.text);
            return CurrencyManager.Instance.CanAfford(value);
        }

        public static bool CanAffordUpgrade(GameObject upgradeButton) {
            return CanAffordNewOrb(upgradeButton);
        }


        protected override bool StartVote() {
            if (votesStarted >= PTSSettingsManager.MaxPostbattleVotes) { return true; }
            if (Time.realtimeSinceStartup < _start_time) { return false; }
            if (!TwitchManager.Instance.EnsureConnected()) { return false; }

            Debug.Log("Vote for postbattle upgrades");

            Transform containerOptions = transform.Find("Container/UpgradePanel/Image/OptionsContainer");
            Transform orbRow = containerOptions.Find("SuggestionOrbRow");

            VoteManager voteManager = VoteManager.Instance;
            voteManager.CleanVote();

            int i = 0;
            foreach (Transform child in orbRow) {
                if (child.gameObject.activeSelf && CanAffordNewOrb(child.gameObject)) {
                    PostBattleNewOrbVoteOption option = child.gameObject.AddComponent(typeof(PostBattleNewOrbVoteOption)) as PostBattleNewOrbVoteOption;
                    option.OrbIndex = i++;
                    option.voteStarter = this;
                    voteManager.AddVoteOption(option);
                }
            }

            if (deckManager.GetUpgradeableOrbs().Count > 0 && PTSSettingsManager.AllowPostbattleUpgrades) {
                GameObject upgradeButton = containerOptions.Find("ButtonLayout/HealUpgradeContainer/UpgradeButton").gameObject;
                if (upgradeButton != null && upgradeButton.activeInHierarchy && CanAffordUpgrade(upgradeButton)) {
                    PostBattleUpgradeVoteOption option = upgradeButton.gameObject.AddComponent(typeof(PostBattleUpgradeVoteOption)) as PostBattleUpgradeVoteOption;
                    voteManager.AddVoteOption(option);
                    option.voteStarter = this;
                    option.deckManager = deckManager;
                }
            }

            if (!didHeal && PTSSettingsManager.AllowPostbattleHeal) {
                GameObject healButton = containerOptions.Find("ButtonLayout/HealUpgradeContainer/HealButton").gameObject;
                if (healButton != null && healButton.activeInHierarchy && CanAffordHeal(healButton)) {
                    PostBattleHealVoteOption option = healButton.gameObject.AddComponent(typeof(PostBattleHealVoteOption)) as PostBattleHealVoteOption;
                    voteManager.AddVoteOption(option);
                    option.voteStarter = this;
                }
            }

            if (!didToad && PTSSettingsManager.AllowPostbattleToad) {
                GameObject toadMaxHPButton = containerOptions.Find("ButtonLayout/ToademContainer/ToademMaxHPButton").gameObject;
                if (toadMaxHPButton != null && toadMaxHPButton.activeInHierarchy && CanAffordHeal(toadMaxHPButton)) {
                    PostBattleToadMaxHPVoteOption option = toadMaxHPButton.gameObject.AddComponent(typeof(PostBattleToadMaxHPVoteOption)) as PostBattleToadMaxHPVoteOption;
                    voteManager.AddVoteOption(option);
                    option.voteStarter = this;
                }
            }

            GameObject skipButton = containerOptions.Find("ContinueButton").gameObject;
            PostBattleSkipUpgradeVoteOption skipOption = skipButton.gameObject.AddComponent(typeof(PostBattleSkipUpgradeVoteOption)) as PostBattleSkipUpgradeVoteOption;
            voteManager.AddVoteOption(skipOption);


            voteManager.VoteTimerText = Utils.CreateVoteTimerText(gameObject.transform.Find("Container"), new Vector3(15, 228, 0));
            if (votesStarted > 0) {
                voteManager.ChangeOddity();
            }
            voteManager.StartVote();
            votesStarted++;
            return true;
        }
    }
}
