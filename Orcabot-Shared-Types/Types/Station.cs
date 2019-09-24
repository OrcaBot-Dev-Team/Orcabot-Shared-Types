using System.Collections.Generic;
using Orcabot.Types.Enums;
namespace Orcabot.Types
{
    public class Station
    {
        public string Name { get; set; }
        public float Distance { get; set; }
        
        public List<StationFacility> StationFacilities { get; set; }
        public Economy Economy { get; set; }
        public Station()
        {
            StationFacilities = new List<StationFacility>();
        }
        public StationType Type { get; set; }
    }
}