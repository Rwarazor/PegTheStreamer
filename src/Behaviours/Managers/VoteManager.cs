using PegTheStreamer.Behaviours.VoteOptions;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace PegTheStreamer.Behaviours {
	public class VoteManager : Singleton<VoteManager> {

		public bool IsVoteActive { get { return _isVoteActive; } }
		private bool _isVoteActive = false;

		public TextMeshProUGUI VoteTimerText { get; set; }

		private int _totalVotes;
		private List<VoteOption> _options;
		private float _startTime;
		private float _expectedEndTime;
		private bool _multipleVotes;
		private HashSet<string> chattersWithVote;

		private bool _shoudlChangeOddity;
		private bool _oddity, _lastOddity;

		public void CleanVote() {
			_options = new List<VoteOption>();
			_totalVotes = 0;
			_shoudlChangeOddity = false;
			chattersWithVote = new HashSet<string>();
		}

		public void AddVoteOption(VoteOption option) {
			_options.Add(option);
		}

		public void ChangeOddity() {
			_shoudlChangeOddity = true;
		}

		public void StartVote() {
			Debug.Log("StartVote");
			if (IsVoteActive) { InterruptVote(); }
			_oddity = _shoudlChangeOddity ? !_lastOddity : false;
			if (_options.Count == 0) { return; }
			if (_options.Count == 1) {
				if (PTSSettingsManager.DoForceOnlyOption) {
					_options[0].Choose();
				}
				return;
			}
			StartVoteSuccessfully();
		}

		public void StartVoteSuccessfully() {
			Debug.Log("StartVoteSuccessfully");
			_isVoteActive = true;
			_options.Sort();
			_startTime = Time.realtimeSinceStartup;
			_expectedEndTime = _startTime + PTSSettingsManager.VoteTime;
			_multipleVotes = PTSSettingsManager.AllowMultipleVotes;
			UpdateText();
			TwitchManager.Instance.onChatMessage += HandleMessage;
			StartVoteChatMessage();
		}

		public void InterruptVote() {
			Debug.Log("InterruptVote");
			if (IsVoteActive) {
				TwitchManager.Instance.onChatMessage -= HandleMessage;
				_isVoteActive = false;
				_lastOddity = _oddity;
				HideVoteTimer();
				foreach (var option in _options) {
					if (option) {
						option.HideCountText();
					}
				}
			}
		}

		public void EndVote() {
			Debug.Log("EndVote");
			if (IsVoteActive) {
				InterruptVote();
				SelectWinner();
			}
		}

		public void Update() {
			if (IsVoteActive) {
				UpdateText();
			}
		}

		public void LateUpdate() {
			if (IsVoteActive && Time.realtimeSinceStartup >= _expectedEndTime) {
				EndVote();
			}
		}

		const int ALPH_SIZE = 'Z' - 'A' + 1;

		private string GetOptionIndex(int index) {
			if (!_oddity) {
				return index.ToString();
			}
			StringBuilder ans = new StringBuilder();
			while (index > 0 || ans.Length == 0) {
				ans.Append((char)('A' + index % ALPH_SIZE));
				index /= ALPH_SIZE;
			}
			return ans.ToString();
		}

		private bool TryParseOptionIndex(string index, out int ans) {
			if (!_oddity) {
				return int.TryParse(index, out ans);
			} else {
				ans = 0;
				foreach (char c in index) {
					if (c < 'A' || c > 'Z') {
						return false;
					}
					ans = ans * ALPH_SIZE + (c - 'A');
				}
				return true;
			}
		}

		private void ShowVoteTimer() {
			if (VoteTimerText) {
				VoteTimerText.enabled = true;
			}
		}

		private void HideVoteTimer() {
			if (VoteTimerText) {
				VoteTimerText.enabled = false;
			}
		}

		private void UpdateText() {
			for (int i = 0; i < _options.Count; ++i) {
				var opt = _options[i];
				opt.ShowCountText();
				opt.SetCountText($"#{GetOptionIndex(i)}: {opt.Votes} ({(_totalVotes == 0 ? 0 : (opt.Votes * 100 / _totalVotes))}%)");
			}
			ShowVoteTimer();
			VoteTimerText.text = $"VOTE NOW: {Mathf.Max(0, Mathf.RoundToInt(_expectedEndTime - Time.realtimeSinceStartup))}s left";
		}

		private void StartVoteChatMessage() {
			string msg = "VOTE NOW:";
			for (int i = 0; i < _options.Count; ++i) {
				var opt = _options[i];
				msg += $" #{GetOptionIndex(i)}: {_options[i].GetOptionTextForChat()};";
			}
			TwitchManager.Instance.SendChatMessage(msg);
		}

		private void EndVoteChatMessage(VoteOption winner) {
			string msg = $"Vote ended... chose {winner.GetOptionTextForChat()}";
			TwitchManager.Instance.SendChatMessage(msg);
		}

		private void HandleMessage(string chatter, string message) {
			int voteNum;
			if (message[0] != '#') {
				return;
			}
			int firstWordLen = message.IndexOf(" ") == -1 ? message.Length - 1 : message.IndexOf(" ") - 1;
			if (TryParseOptionIndex(message.Substring(1, firstWordLen), out voteNum)) {
				AddChatVote(chatter, voteNum);
			}
		}

		private void AddChatVote(string chatter, int option) {
			if (!IsVoteActive) { return; }
			if (option >= 0 && option < _options.Count) {
				if (_multipleVotes || !chattersWithVote.Contains(chatter)) {
					_options[option].Votes++;
					_totalVotes += 1;
					chattersWithVote.Add(chatter);
				}
			}
		}

		private void SelectWinner() {
			List<VoteOption> maxVotes = new List<VoteOption>();
			foreach (var option in _options) {
				if (maxVotes.Count == 0 || option.Votes == maxVotes[0].Votes) {
					maxVotes.Add(option);
				} else if (option.Votes > maxVotes[0].Votes) {
					maxVotes = new List<VoteOption>();
					maxVotes.Add(option);
				}
			}
			VoteOption winner = maxVotes[Random.Range(0, maxVotes.Count)];
			EndVoteChatMessage(winner);
			winner.Choose();
		}
	}
}
