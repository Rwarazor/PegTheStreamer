using PegTheStreamer.Behaviours.Settings;
using UnityEngine;

namespace PegTheStreamer.Behaviours {
	public class PTSSettingsManager : Singleton<PTSSettingsManager> {

		static readonly Color backgroundColor = new Color(0.8f, 0.45f, 0.2f, 0.2f);
		static readonly Color textColor = new Color(0.2f, 0.1961f, 0.2392f, 1.0f);


		public static SettingBoolean EnableMod = new SettingBoolean("EnableMod", "Peg The Streamer?", true, backgroundColor);
		public static SettingBoolean DoPullNicknames = new SettingBoolean("DoPullNicknames", "Peg The Chat?", true, backgroundColor);
		public static SettingString TwitchLogin = new SettingString("TwitchLogin", "Twitch login: ", "", "Enter login here", backgroundColor, textColor);
		public static SettingString TwitchOAuth = new SettingString("TwitchOAuth", "Twitch OAuth: ", "", "Enter OAuth here", backgroundColor, textColor);
		public static SettingInt VoteTime = new SettingInt("VoteTime", "Vote timer (seconds)", 35, new int[] { 10, 15, 20, 25, 30, 35, 40, 50, 60, 90, 120 }, backgroundColor);
		public static SettingBoolean DoForceOnlyOption = new SettingBoolean("DoForceOnlyOption", "Auto-choose if single option", true, backgroundColor);
		public static SettingBoolean EnableVoteStarting = new SettingBoolean("EnableVoteStarting", "Vote for starting relic", true, backgroundColor);
		public static SettingBoolean AllowSkipStarting = new SettingBoolean("AllowSkipStarting", "Allow skip starting relic", false, backgroundColor);
		public static SettingBoolean EnableVotePostbattle = new SettingBoolean("EnableVotePostbattle", "Vote for post-battle reward", true, backgroundColor);
		public static SettingInt MaxPostbattleVotes = new SettingInt("MaxPostbattleVotes", "Max postbattle votes", 1, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, backgroundColor);
		public static SettingBoolean AllowPostbattleUpgrades = new SettingBoolean("AllowPostbattleUpgrades", "Allow post-battle orb upgrades", false, backgroundColor);
		public static SettingBoolean AllowPostbattleHeal = new SettingBoolean("AllowPostbattleHeal", "Allow post-battle heal", false, backgroundColor);
		public static SettingBoolean AllowPostbattleToad = new SettingBoolean("AllowPostbattleToad", "Allow post-battle +max HP", false, backgroundColor);
		public static SettingBoolean EnableVoteChest = new SettingBoolean("EnableVoteChest", "Vote for chest relics", true, backgroundColor);
		public static SettingBoolean AllowSkipChest = new SettingBoolean("AllowSkipChest", "Allow skip chest relic", false, backgroundColor);
		public static SettingBoolean EnableVoteElite = new SettingBoolean("EnableVoteElite", "Vote for elite relics", true, backgroundColor);
		public static SettingBoolean AllowSkipElite = new SettingBoolean("AllowSkipElite", "Allow skip elite relic", false, backgroundColor);
		public static SettingBoolean EnableVoteBoss = new SettingBoolean("EnableVoteBoss", "Vote for boss relics", true, backgroundColor);
		public static SettingBoolean AllowSkipBoss = new SettingBoolean("AllowSkipBoss", "Allow skip boss relic", false, backgroundColor);
		public static SettingBoolean AllowMultipleVotes = new SettingBoolean("AllowMultipleVotes", "Multiple votes/chatter (DEBUG)", false, backgroundColor);

		private SettingBase[] settings = {
			EnableMod,
			DoPullNicknames,
			TwitchLogin,
			TwitchOAuth,
			VoteTime,
			DoForceOnlyOption,
			EnableVoteStarting,
			AllowSkipStarting,
			EnableVotePostbattle,
			MaxPostbattleVotes,
			AllowPostbattleUpgrades,
			AllowPostbattleHeal,
			AllowPostbattleToad,
			EnableVoteChest,
			AllowSkipChest,
			EnableVoteElite,
			AllowSkipElite,
			EnableVoteBoss,
			AllowSkipBoss,
			AllowMultipleVotes
		};
		private void Awake() {
			LoadSettings();
		}

		private void Start() {
			this.InitializePreviousSettings();
			this.SaveSettings();
		}

		public void ApplySettings() {
			this.SaveSettings();
			this.InitializePreviousSettings();
		}

		public void LoadSettings() {
			foreach (SettingBase setting in settings) { setting.LoadValue(); }
		}

		public void SaveSettings() {
			foreach (SettingBase setting in settings) { setting.SaveValue(); }
			PlayerPrefs.Save();
		}

		public void InitializePreviousSettings() {
			foreach (SettingBase setting in settings) { setting.InitializePreviousValue(); }
		}

		public void RevertSettings() {
			foreach (SettingBase setting in settings) { setting.RevertValue(); }
			this.ApplySettings();
		}

		public void ShowSettings() {
			foreach (SettingBase setting in settings) { setting.ShowValue(); }
		}

		public void AddApplyActions() {
			foreach (SettingBase setting in settings) { setting.AddApplyAction(); }
		}

		public void RemoveApplyActions() {
			foreach (SettingBase setting in settings) { setting.RemoveApplyAction(); }
		}

		public void CreateUI() {
			foreach (SettingBase setting in settings) { setting.CreateUI(); }
		}
	}
}
