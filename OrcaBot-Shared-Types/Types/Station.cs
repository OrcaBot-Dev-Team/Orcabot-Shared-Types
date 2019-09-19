using System.Collections.Generic;
using OrcaBot.SharedTypes.Enums;
namespace OrcaBot.SharedTypes
{
    public class Station
    {
        public string Name { get; set; }
        public float Distance { get; set; }
        public PadSize PadSize { get; set; }
        public List<StationFacility> StationFacilities { get; set; }
        public Economy Economy { get; set; }
        public Station()
        {
            StationFacilities = new List<StationFacility>();
        }
    }
}