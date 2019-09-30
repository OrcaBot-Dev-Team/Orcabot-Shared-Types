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
                result.RemoveClear(remove);
            }
            if (filter.PermitLocked != null)
            {
                remove.AddRange(result.Where(system => system.IsPermitLocked != filter.PermitLocked.Value));
                result.RemoveClear(remove);
            }
            if (filter.PermitName != null)
            {
                remove.AddRange(result.Where(system => system.PermitName != filter.PermitName));
                result.RemoveClear(remove);
            }
            if (filter.Security != null)
            {
                remove.AddRange(result.Where(system => system.Security != filter.Security.Value));
                result.RemoveClear(remove);
            }
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

    }
}
