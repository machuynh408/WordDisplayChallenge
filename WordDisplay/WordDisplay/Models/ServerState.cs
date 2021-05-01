using System;

namespace WordDisplay.Models
{
    public class ServerState
    {
        public bool CommandRunning { get; set; }

        public string CurrentWordSelected { get; set; }

        public int CommandTimeRemaining { get; set; }
    }
}
