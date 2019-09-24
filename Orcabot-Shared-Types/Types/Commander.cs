using System;
using System.Collections.Generic;
using System.Text;
using Orcabot.Types.Enums;
using Orcabot.Types.Enums.Ranks;

namespace Orcabot.Types
{
    class Commander
    {
        public string CommanderName { get; set; }
        public ulong? Balance { get; set; }
        public ulong? TotalAssets { get; set; }

        #region Ranks
        public Combat CombatRank { get; set; }
        public Trade TradeRank { get; set; }
        public Explore ExplorationRank { get; set; }
        public Federation FederationRank { get; set; }
        #endregion
        public Types.System LastKnownLocation { get; set; }
    }
}
