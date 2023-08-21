using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PegTheStreamer.Behaviours {
    class TwitchNamesPoolManager : Singleton<TwitchNamesPoolManager> {
        public bool TryFetchName(out string res) {
            if (_unused.Count == 0) {
                res = null;
                return false;
            } else {
                res = _unused.ElementAt(Random.Range(0, _unused.Count));
                _unused.Remove(res);
                _currentlyUsed.Add(res);
                return true;
            }
        }

        public void FreeName(string res) {
            _currentlyUsed.Remove(res);
            if (FilterNickname(res)) {
                _unused.Add(res);
            }
        }

        private HashSet<string> _currentlyUsed = new HashSet<string>();
        private HashSet<string> _unused = new HashSet<string>();

        private string StripNickname(string name) {
            return name.Substring(0, name.IndexOf("!"));
        }

        private bool FilterNickname(string name) {
            return name.Length <= 12;
        }

        private void HandleChatMessage(string chatter, string msg) {
            chatter = StripNickname(chatter);
            if (!_currentlyUsed.Contains(chatter) && FilterNickname(chatter)) {
                _unused.Add(chatter);
            }
        }

        private void Awake() {
            TwitchManager.Instance.onChatMessage += HandleChatMessage;
        }

        private void OnDestroy() {
            TwitchManager.Instance.onChatMessage -= HandleChatMessage;
        }
    }
}
