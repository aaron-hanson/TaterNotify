using System.Collections.Generic;
using System.Windows.Forms;

namespace TaterNotify
{
    class TaterStandingsRow
    {
        public string Owner { get; set; }
        public int Taters { get; set; }
        public List<long> Batters; 

        public TaterStandingsRow(string owner, int taters)
        {
            Owner = owner;
            Taters = taters;
            Batters = new List<long>();
        }
    }

}