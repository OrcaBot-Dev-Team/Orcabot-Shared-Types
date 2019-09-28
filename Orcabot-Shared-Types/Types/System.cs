using System;
using System.Collections.Generic;
using System.Numerics;
using Orcabot.Types.Enums;
using Newtonsoft.Json;
using Orcabot.Helpers;
using System.Runtime.Serialization;
using System.Linq;

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
            setupStations();
        }

        private void setupStations()
        {
            IOrderedEnumerable<Station> stations = Stations.OrderBy(s => s.Distance);

            Station_BestOrbitalLarge = null;
            Station_BestOrbitalMedium = null;
            Station_BestPlanetary = null;
            Station_MaterialTrader = null;
            foreach (var station in stations)
            {
                station.CurrentSystem = this;
                if (station.HasMaterialTrader)
                {
                    Station_MaterialTrader = station;
                }
                if (Station_BestPlanetary == null && station.RelevantType == RelevantStationType.Planetary)
                {
                    Station_BestPlanetary = station;
                }
                else if (Station_BestOrbitalLarge == null && station.RelevantType == RelevantStationType.OrbitalLarge)
                {
                    Station_BestOrbitalLarge = station;
                }
                else if (Station_BestOrbitalMedium == null && station.RelevantType == RelevantStationType.OrbitalMedium)
                {
                    Station_BestOrbitalMedium = station;
                }
            }
        }

        #endregion
        #region Methods

        /// <summary>
        /// Calculates the distance of this system to <paramref name="position"/>
        /// </summary>
        public float GetDistanceTo(Vector3 position)
        {
            return Vector3.Distance(Coordinate, position);
        }

        /// <summary>
        /// Calculates the squared distance of this system to <paramref name="position"/>
        /// </summary>
        public float GetDistanceSquaredTo(Vector3 position)
        {
            return Vector3.DistanceSquared(Coordinate, position);
        }

        public override string ToString()
        {
            return $"{Name}, Stations: {string.Join(", ", GetBestStationsDistChecked(false))}";
        }

        /// <summary>
        /// Returns a list of the best stations, with filtering on the distance.
        /// </summary>
        /// <param name="hideMaterialTraderIfBestOrbitalLarge">If true and the best orbital large is the material trader station, the material trader station is not provided again</param>
        public IReadOnlyList<Station> GetBestStationsDistChecked(bool hideMaterialTraderIfBestOrbitalLarge)
        {
            List<Station> result = new List<Station>();
            if (Station_BestOrbitalLarge != null)
            {
                result.Add(Station_BestOrbitalLarge);
            }

            compareAddStation(result, Station_BestOrbitalLarge, Station_BestOrbitalMedium);
            compareAddStation(result, Station_BestOrbitalLarge, Station_BestPlanetary);

            if (Station_MaterialTrader != null)
            {
                if (!hideMaterialTraderIfBestOrbitalLarge || Station_MaterialTrader == Station_BestOrbitalLarge)
                {
                    result.Add(Station_MaterialTrader);
                }
            }

            return result.AsReadOnly();
        }

        /// <summary>
        /// Removes all irrelevant or doubled up stations from the stations list.
        /// </summary>
        public void RemoveIrrelevantStations()
        {
            Stations.Clear();
            Stations.AddRange(GetBestStationsDistChecked(true));
        }

        private void compareAddStation(List<Station> list, Station first, Station optional)
        {
            if (optional != null)
            {
                if (first != null)
                {
                    if (optional.Distance < first.Distance)
                    {
                        list.Add(optional);
                    }
                }
                else
                {
                    list.Add(optional);
                }
            }
        }

        /// <summary>
        /// Filter the stations in this system
        /// </summary>
        /// <param name="type">If not null, will only provide stations of the desired type</param>
        /// <param name="facility">If not null, will only provide stations with the desired facility available</param>
        /// <param name="minPadSize">If not null, will only provide stations with the minimum landing pad size present</param>
        /// <param name="result">New list with only the filtered stations</param>
        /// <returns>True, if atleast one station matching the filters was found</returns>
        public bool FilterStations(StationSearchFilter filter, out List<Station> result)
        {
            return Stations.Filter(filter, out result);
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
