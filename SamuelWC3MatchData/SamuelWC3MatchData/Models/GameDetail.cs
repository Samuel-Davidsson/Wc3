using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamuelWC3MatchData.Models
{
    public class GameDetail
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string GameType { get; set; }
        public string Map { get; set; }
        public string Allies { get; set; }
        public string Opponents { get; set; }
        public string GameLength { get; set; }
        public string Result { get; set; }

    }
}
