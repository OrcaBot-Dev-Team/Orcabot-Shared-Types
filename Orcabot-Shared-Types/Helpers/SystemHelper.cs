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
        public static IList<StarSystem> FilterMaterialTrader(this IList<StarSystem> systems)
        {

            var returnList = new List<Types.StarSystem>();
            foreach (var system in systems)
            {
                if (system.HasMaterialTrader)
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }


        public static Dictionary<string, StarSystem> FilterMaterialTraders(this Dictionary<string, StarSystem> dict)
        {
            return new Dictionary<string, StarSystem>(dict.Where(pair => { return pair.Value.HasMaterialTrader; }));
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
        public static IList<StarSystem> FilterFacility(this IList<StarSystem> systems, StationFacility stationFacility)
        {
            var returnList = new List<Types.StarSystem>();
            foreach (var system in systems)
            {
                if (system.Stations.FilterFacility(stationFacility).Count > 0)
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }

        /// <summary>
        /// Filter a list of systems with parameters
        /// </summary>
        /// <param name="systems">List of systems to filter through</param>
        /// <param name="result">New list with only the filtered systems</param>
        /// <param name="filter">Filter settings</param>
        /// <returns>True, if atleast one system matching the filters was found</returns>
        public static bool Filter(this IEnumerable<StarSystem> systems, SystemSearchFilter filter, out List<StarSystem> result)
        {
            if (filter.PermitName != null)
            {
                filter.PermitLocked = true;
            }
            result = new List<StarSystem>(systems);
            List<StarSystem> remove = new List<StarSystem>();
            if (filter.TraderType != null)
            {
                remove.AddRange(result.Where(system => system.MaterialTraderType != filter.TraderType));
            }
            if (filter.PermitLocked != null)
            {
                remove.AddRange(result.Where(system => system.IsPermitLocked != filter.PermitLocked.Value));
            }
            if (filter.PermitName != null)
            {
                remove.AddRange(result.Where(system => system.PermitName != filter.PermitName));
            }
            if (filter.Security != null)
            {
                remove.AddRange(result.Where(system => system.Security != filter.Security.Value));
            }
            result.RemoveRange(remove);
            return result.Count > 0;
        }

        /// <summary>
        /// Filters stations in a list of systems
        /// </summary>
        /// <param name="systems">Enumerable to filter through</param>
        /// <param name="systemFilter">system filter to apply</param>
        /// <param name="stationFilter">station filter to apply</param>
        /// <param name="result">a resulting new List with all stations</param>
        /// <returns></returns>
        public static bool FilterStations(this IEnumerable<StarSystem> systems, SystemSearchFilter systemFilter, StationSearchFilter stationFilter, out List<Station> result)
        {
            result = new List<Station>();
            if (systems.Filter(systemFilter, out var validSystems))
            {
                foreach (StarSystem system in validSystems)
                {
                    if (system.FilterStations(stationFilter, out List<Station> stations))
                    {
                        result.AddRange(stations);
                    }
                }
                return result.Count > 0;
            }
            else
            {
                return false;
            }
        }

        [Obsolete]
        public static float DistanceTo(this Coordinate c1, Coordinate c2)
        {
            var X = c1.X - c2.X;
            var Y = c1.Y - c2.Y;
            var Z = c1.Z - c2.Z;
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

    }
}
