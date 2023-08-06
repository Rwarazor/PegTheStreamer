using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace PegTheStreamer.Behaviours {
    public class TwitchManager : Singleton<TwitchManager> {

        public delegate void OnChatMessage(string chatter, string msg);
        public event OnChatMessage onChatMessage;

        public bool EnsureConnected() {
            if (Twitch != null && Twitch.Connected) { return true; }
            if (!ExponentialBackoff()) { return false; }

            Debug.Log("Connecting to Twitch");

            Twitch = new TcpClient(URL, PORT);
            Reader = new StreamReader(Twitch.GetStream());
            Writer = new StreamWriter(Twitch.GetStream());
            Connect(User);

            if (Twitch.Connected) {
                Debug.Log("Connection successful?");
                SendChatMessage("Welcome to Peg The Streamer!");
                currentDelay = 1;
                return true;
            } else {
                Debug.Log($"Connection failed :(, trying again in {currentDelay} seconds");
                return false;
            }
        }
        public void SendChatMessage(string msg) {
            Writer.WriteLine($"PRIVMSG #{User.ToLower()} :" + msg);
            Writer.Flush();
        }

        void Awake() {
            lastTimeTried = -1;
        }

        void Update() {
            if (VoteManager.Instance.IsVoteActive && !Twitch.Connected) { EnsureConnected(); }
            while (Twitch?.Available > 0) { HandleRawLine(Reader.ReadLine()); }
        }

        string User { get { return PTSSettingsManager.TwitchLogin; } }
        string OAuth { get { return PTSSettingsManager.TwitchOAuth; } }

        TcpClient Twitch;
        StreamReader Reader;
        StreamWriter Writer;

        const string URL = "irc.chat.twitch.tv";
        const int PORT = 6667;

        private void Connect(string channel) {
            Writer.Write(
                $"PASS {OAuth}\n" +
                $"NICK {User.ToLower()}\n" +
                $"JOIN #{channel.ToLower()}\n");
            Writer.Flush();
        }

        private float lastTimeTried;
        private float currentDelay = 1;
        private bool ExponentialBackoff() {
            if (Time.realtimeSinceStartup >= lastTimeTried + currentDelay) {
                lastTimeTried = Time.realtimeSinceStartup;
                currentDelay = Mathf.Min(64, currentDelay * 2);
                return true;
            } else {
                return false;
            }
        }

        private void HandleRawLine(string rawLine) {
            if (rawLine.StartsWith("PING ")) {
                Writer.WriteLine("PONG " + rawLine.Substring(5));
                Writer.Flush();
                return;
            } else if (rawLine.Contains("PRIVMSG")) {
                int indNameStart = rawLine.IndexOf("#") + 1;
                int indMessageStart = rawLine.IndexOf(":", indNameStart + 1) + 1;
                int nameLength = indMessageStart - indNameStart - 2;
                string name = rawLine.Substring(indNameStart, nameLength);
                string message = rawLine.Substring(indMessageStart);
                onChatMessage?.Invoke(name, message);
            }
        }
    }
}
