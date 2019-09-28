using System.Collections.Generic;
using Orcabot.Types.Enums;
using Newtonsoft.Json;
using Orcabot.Helpers;
using System.Runtime.Serialization;
using Orcabot.Types.Enums.Ranks;
using System.Numerics;

namespace Orcabot.Types
{
    /// <summary>
    /// Represents a station as found in <see cref="StarSystem"/>s
    /// </summary>
    public class Station
    {
        #region Serialized Properties

        /// <summary>
        /// Name of the station
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Distance of the station from system entry point. In lightseconds.
        /// </summary>
        public float Distance { get; set; }
        
        /// <summary>
        /// List of all Facilities the station offers
        /// </summary>
        public HashSet<StationFacility> StationFacilities { get; set; } = new HashSet<StationFacility>();

        /// <summary>
        /// Economy dictating the stations mission board and market
        /// </summary>
        public Economy Economy { get; set; }

        /// <summary>
        /// Type of the station
        /// </summary>
        public StationType Type { get; set; }

        #endregion
        #region Cached Properties

        /// <summary>
        /// The largest pad available at a station.
        /// </summary>
        [JsonIgnore]
        public PadSize LargestPadAvailable { get; private set; }

        /// <summary>
        /// True, if the station has a material trader
        /// </summary>
        [JsonIgnore]
        public bool HasMaterialTrader { get { return MaterialTrader > 0; } }

        /// <summary>
        /// The type of the material trader. If no material trader is present, null
        /// </summary>
        [JsonIgnore]
        public TraderType MaterialTrader { get; private set; }

        /// <summary>
        /// Relevant Type of this station
        /// </summary>
        [JsonIgnore]
        public RelevantStationType RelevantType { get; private set; }

        [OnDeserialized]
        internal void SetupCachedProperties(StreamingContext context)
        {
            RelevantType = this.GetRelevantStationType();
            LargestPadAvailable = RelevantType.ToPadSize();
            MaterialTrader = this.GetMatTrader();
        }

        public override string ToString()
        {
            return $"{Name} ({Type}), Services: {string.Join(", ", StationFacilities)}";
        }

        public string ToString(IStationEmojiProvider emojiProvider)
        {
            return $"{emojiProvider} {Name}, Services: {string.Join(", ", StationFacilities)}";
        }

        #endregion
    }

    public interface IStationEmojiProvider
    {
        string GetStationEmoji(StationType type);
    }
}