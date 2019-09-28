using System;
using System.Collections.Generic;
using System.Numerics;
using Orcabot.Types.Enums;
using Newtonsoft.Json;
using Orcabot.Helpers;
using System.Runtime.Serialization;

namespace Orcabot.Types
{
    public class StarSystem : IDistanceProvider
    {
        #region Serialized Properties

        /// <summary>
        /// Name of the system
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Coordinates of the system, relative to sol in lightyears
        /// </summary>
        public Vector3 Coordinate { get; set; }
        /// <summary>
        /// All stations in the system
        /// </summary>
        public List<Station> Stations { get; set; } = new List<Station>();
        /// <summary>
        /// Security rating of this system
        /// </summary>
        public SystemSecurity Security { get; set; }
        /// <summary>
        /// Permit Name locking this system. Null if no permit lock
        /// </summary>
        public string PermitName { get; set; }

        #endregion
        #region Cached Properties

        /// <summary>
        /// True, if the system requires a permit to enter
        /// </summary>
        [JsonIgnore]
        public bool IsPermitLocked { get { return PermitName != null; } }

        /// <summary>
        /// The closest to entry orbital station featuring a large landing pad. Null if no large orbital stations present
        /// </summary>
        [JsonIgnore]
        public Station Station_BestOrbitalLarge { get; private set; }

        /// <summary>
        /// The closest to entry orbital station featuring a medium landing pad. Null if no medium orbital stations present
        /// </summary>
        [JsonIgnore]
        public Station Station_BestOrbitalMedium { get; private set; }

        /// <summary>
        /// The closest to entry planetary station. Null if no planetary stations present
        /// </summary>
        [JsonIgnore]
        public Station Station_BestPlanetary { get; private set; }

        /// <summary>
        /// The station in the system with a material trader. Null if no material trader present
        /// </summary>
        [JsonIgnore]
        public Station Station_MaterialTrader { get; private set; }

        /// <summary>
        /// True if system has a material trader
        /// </summary>
        [JsonIgnore]
        public bool HasMaterialTrader { get { return Station_MaterialTrader != null; } }

        /// <summary>
        /// The material trader type present in the system. Null if no material trader is present
        /// </summary>
        [JsonIgnore]
        public TraderType MaterialTraderType { get { if (HasMaterialTrader) { return Station_MaterialTrader.MaterialTrader; } else { return TraderType.NoTrader; } } }

        /// <summary>
        /// Method automatically called after deserializing
        /// </summary>
        [OnDeserialized]
        internal void SetupCachedProperties(StreamingContext context)
        {
            this.GetRelevantStations(out Station L, out Station M, out Station P, out Station T);
            Station_BestOrbitalLarge = L;
            Station_BestOrbitalMedium = M;
            Station_BestPlanetary = P;
            Station_MaterialTrader = T;
        }

        #endregion
        #region Methods

        public float GetDistanceSquared(Vector3 position)
        {
            return Vector3.DistanceSquared(Coordinate, position);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }

    [Obsolete]
    public struct Coordinate
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
