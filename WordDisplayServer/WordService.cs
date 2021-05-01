using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace WordDisplayServer {
    public class WordService {

        private List<string> words = new List<string>();
        private bool commandRunning = false;
        private string currentWordSelected = "";
        private int commandTimeRemainining = 0;
        private Random rng = new Random(DateTime.Now.Millisecond);
        private const int clockInterval = 1000;
        private Timer clock = null;
        private int cycleCounter = 0;
        private const int cycleMax = 60000; // 60 seconds in milliseconds
        private readonly IHubContext<ServerStateHub> hub;

        public WordService(IHubContext<ServerStateHub> hub)
        {
            this.hub = hub;
            this.clock = new Timer(clockInterval);
        }

        /// <summary>
        /// Randomizes the words, and starts the clock cycle and notifies the user of updates.
        /// </summary>
        /// <param name="data">The JSON array object containing the words.</param>
        public void Start(JArray data) {
            Console.WriteLine("[WordService]: Start!");
            this.words.Clear();

            // Randomize the words from the payload,
            this.words.AddRange(data.Values<string>().OrderBy(x => rng.Next()).ToList());

            if (commandRunning) {
                commandRunning = false;
                clock.Stop();
                clock.Elapsed -= OnElapsed;
            }
            commandRunning = true;
            clock.Elapsed += OnElapsed;
            currentWordSelected = words.ElementAt(0);
            clock.Start();
            Notify();
        }

        /// <summary>
        /// Calculates remaining time, and if the cycle is over, remove the word and start cycle for next word.
        /// It notifies the user of changes.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void OnElapsed(Object source, ElapsedEventArgs e) {
            cycleCounter += clockInterval;
            commandTimeRemainining = (cycleMax - cycleCounter);
            if (cycleCounter >= cycleMax) {
                this.words.RemoveAt(0);
                cycleCounter = 0;

                if (words.Count <= 0) {
                    clock.Stop();
                    clock.Elapsed -= OnElapsed;
                    commandRunning = false;
                    currentWordSelected = "";
                }
                else {
                    currentWordSelected = words.ElementAt(0);
                    Console.WriteLine($"[WordService]: Word [{currentWordSelected}] - Changed!");
                }
            }
            Notify();
        }

        /// <summary>
        /// Notifies the clients connected to our ServerStateHub via SignalR
        /// </summary>
        public void Notify() {
            hub.Clients.All.SendAsync("StateChanged", commandRunning, currentWordSelected, commandTimeRemainining / clockInterval);
        }
    }
}