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
        public HashSet<StationFacility> Facilities { get; set; } = new HashSet<StationFacility>();

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
        /// The system this station is found in
        /// </summary>
        [JsonIgnore]
        public StarSystem CurrentSystem { get; internal set; }
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

        /// <summary>
        /// True, if the station is located on a planets surface (therefor requiring horizons)
        /// </summary>
        [JsonIgnore]
        public bool IsPlanetary => Type == StationType.SurfaceSettlement || Type == StationType.SurfaceStation;

        /// <summary>
        /// Method automatically called after deserializing
        /// </summary>
        [OnDeserialized]
        internal void SetupCachedProperties(StreamingContext context)
        {
            RelevantType = Type.ToRelevantStationType();
            LargestPadAvailable = RelevantType.ToPadSize();
            MaterialTrader = getMatTrader();
        }

        private TraderType getMatTrader()
        {
            foreach (StationFacility fac in Facilities)
            {
                if (fac > StationFacility.TraderUnknown)
                {
                    return fac.ToTraderType();
                }
            }
            return TraderType.NoTrader;
        }

        #endregion
        #region Methods

        /// <summary>
        /// Check for a ships ability to land on this station
        /// </summary>
        /// <param name="shipSize">The minimum landing pad size the ship requires</param>
        /// <returns>True, if the specified ship can land on the station</returns>
        public bool CanLand(PadSize shipSize)
        {
            return LargestPadAvailable >= shipSize;
        }

        public override string ToString()
        {
            return $"{Name} ({Type}), Services: {string.Join(", ", Facilities)}";
        }

        public string ToString(IStationEmojiProvider emojiProvider)
        {
            return $"{emojiProvider.GetStationEmoji(Type)} {Name}, Services: {string.Join(", ", Facilities)}";
        }

        /// <summary>
        /// Check wether a station has a desired list of facilities
        /// </summary>
        /// <param name="facilities">Facilities to check for</param>
        /// <returns>True, if all facilities are present</returns>
        public bool HasFacilities(params StationFacility[] facilities)
        {
            foreach (StationFacility facility in facilities)
            {
                if (!Facilities.Contains(facility))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check wether a station has a facility
        /// </summary>
        /// <param name="facility">Facility to check for</param>
        /// <returns>True, if the facility is present</returns>
        public bool HasFacility(StationFacility facility)
        {
            return Facilities.Contains(facility);
        }

        #endregion
    }

    /// <summary>
    /// This class can provide station emoji strings for station types
    /// </summary>
    public interface IStationEmojiProvider
    {
        string GetStationEmoji(StationType type);
    }
}