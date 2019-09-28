using Orcabot.Types.Enums;
using Orcabot.Types;


using System.Collections.Generic;
using System;
using System.Linq;
using System.Numerics;

namespace Orcabot.Helpers
{
    public static class SystemHelper
    {
        /// <summary>
        /// Returns a List of all Systems that have a Material Trader of any kind.
        /// </summary>
        /// <returns>A new List of Systems with Material Traders</returns>
        /// <param name="systems">List of Systems</param>
        public static IList<StarSystem> FilterMaterialTrader(this IList<StarSystem> systems )
        {

            var returnList = new List<Types.StarSystem>();
            foreach(var system in systems)
            {
                if(system.HasMaterialTrader)
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }
        public static Dictionary<string,StarSystem> FilterMaterialTraders(this Dictionary<string,StarSystem> dict, bool keepTraderStationsOnly) {
            var returnDict = new Dictionary<string, StarSystem>();
            foreach (var entry in dict) {
                if (entry.Value.HasMaterialTrader) {
                    var system = entry.Value;
                    if (keepTraderStationsOnly) {
                        foreach (var station in system.Stations) {
                            if (!station.HasMaterialTrader) {
                                system.Stations.Remove(station);
                            }
                        }
                    }

                    returnDict.Add(entry.Key, system);
                }
            }
            return returnDict;
        }
        public static void RemoveIrrelevantStations (this StarSystem sys)
        {
            sys.Stations = sys.FilterRelevantStations();
        }

        public static List<Station> FilterRelevantStations(this StarSystem sys)
        {
            sys.GetRelevantStations(out Station L, out Station M, out Station P, out Station T);
            List<Station> filteredStations = new List<Station>();

            filteredStations.checkNullAdd(L);
            filteredStations.checkNullAdd(M);
            filteredStations.checkNullAdd(P);
            filteredStations.checkNullAdd(T);
            return filteredStations;
        }


        private static void checkNullAdd<T>(this List<T> list, T item) where T : class
        {
            if (item != null)
            {
                list.Add(item);
            }
        }

        public static void GetRelevantStations(this StarSystem sys, out Station bestOrbitalLarge, out Station bestOrbitalMedium, out Station bestPlanetary, out Station materialTrader)
        {
            IOrderedEnumerable<Station> stations = sys.Stations.OrderBy(s => s.Distance);
            bestOrbitalLarge = null;
            bestOrbitalMedium = null;
            bestPlanetary = null;
            materialTrader = null;
            foreach (var station in stations)
            {
                if (station.HasMaterialTrader)
                {
                    materialTrader = station;
                }
                if (station.RelevantType == RelevantStationType.Planetary)
                {
                    if (bestPlanetary == null)
                    {
                        bestPlanetary = station;
                    }
                }
                else if (station.RelevantType == RelevantStationType.OrbitalLarge)
                {
                    if (bestOrbitalLarge == null)
                    {
                        bestOrbitalLarge = station;
                    }
                }
                else if (station.RelevantType == RelevantStationType.OrbitalMedium)
                {
                    if (bestOrbitalMedium == null)
                    {
                        bestOrbitalMedium = station;
                    }
                }
                if (bestOrbitalLarge != null && bestOrbitalMedium != null && bestPlanetary != null && materialTrader != null)
                {
                    // sys.HasMatTrader() means traversing all stations 4! times! More efficient to just continue this traversal until materialtrader has been found
                    break;
                    //if (!sys.HasMatTrader())
                    //    break;
                    //else if (materialTrader != null)
                    //    break;
                }
            }
            // It's the same station, so one reference can be removed
            // We will keep materialTrader for this method, and filtering when calling. Don't want to delete the information that this system indeed has a material trader!

            //if (bestOrbitalLarge == materialTrader)
            //{
            //    materialTrader = null;
            //}

            //L M and Planet now have the closest Stations
            if ((bestOrbitalLarge != null) && (bestOrbitalMedium != null) && bestOrbitalLarge.Distance < bestOrbitalMedium.Distance)
            {
                bestOrbitalMedium = null; //Medium Station is irrelevant in this case due to the Large being closer.
            }
            if ((bestOrbitalLarge != null) && (bestPlanetary != null) && bestOrbitalLarge.Distance < bestPlanetary.Distance)
            {
                bestPlanetary = null; //Planetary is irrelevant in this case due to the large being closer.
            }
        }


        [Obsolete]
        public static bool HasMatTrader(this StarSystem system)
        {
            return (
                    system.Stations.FilterFacility(StationFacility.TraderEncoded).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderManufactured).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderRaw).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderUnknown).Count > 0
                    );
        }

        [Obsolete]
        public static StationFacility GetMatTraderType(this Types.StarSystem system)
        {
            if (system.Stations.FilterFacility(StationFacility.TraderEncoded).Count > 0)
            {
                return StationFacility.TraderEncoded;
            }
            if (system.Stations.FilterFacility(StationFacility.TraderManufactured).Count > 0)
            {
                return StationFacility.TraderManufactured;
            }
            if (system.Stations.FilterFacility(StationFacility.TraderRaw).Count > 0)
            {
                return StationFacility.TraderRaw;
            }
            if (system.Stations.FilterFacility(StationFacility.TraderUnknown).Count > 0)
            {
                return StationFacility.TraderUnknown;
            }
            throw new System.Exception("No Trader in System. Check if Trader exists before calling this Method.");
        }
        /// <summary>
        /// Returns a new List of Systems that have the given Facility. This can be used to get a specific Trader. Do get any trader, use FilterMaterialTrader instead
        /// </summary>
        /// <returns>The facility.</returns>
        /// <param name="systems">Systems.</param>
        /// <param name="stationFacility">Station facility.</param>
        public static IList<Types.StarSystem> FilterFacility(this IList<Types.StarSystem> systems, StationFacility stationFacility)
        {
            var returnList = new List<Types.StarSystem>();
            foreach(var system in systems)
            {
                if(system.Stations.FilterFacility(stationFacility).Count > 0)
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }
        public static float DistanceTo(this Types.StarSystem sys, Vector3 coordinate)
        {
            return Vector3.Distance(sys.Coordinate, coordinate);
        }

        [Obsolete]
        public static float DistanceTo(this Coordinate c1,Coordinate c2)
        {
            var X = c1.X - c2.X;
            var Y = c1.Y - c2.Y;
            var Z = c1.Z - c2.Z;
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

    }
}
