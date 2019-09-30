using Orcabot.Types.Enums;
using Orcabot.Types;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Orcabot.Helpers
{
    public static class StationHelper
    {
        /// <summary>
        /// Filter a list of stations with parameters
        /// </summary>
        /// <param name="stations">List of stations to filter through</param>
        /// <param name="filter">Filter configuration</param>
        /// <returns>True, if atleast one station matching the filters was found</returns>
        public static bool Filter(this IEnumerable<Station> stations, StationSearchFilter filter, out List<Station> result)
        {
            result = new List<Station>(stations);
            List<Station> remove = new List<Station>();
            if (filter.Type != null)
            {
                remove.AddRange(result.Where(station => station.Type != filter.Type.Value));
                result.RemoveClear(result);
            }
            if (filter.Facility != null)
            {
                remove.AddRange(result.Where(station => !station.HasFacility(filter.Facility.Value)));
                result.RemoveClear(result);
            }
            if (filter.MinPadSize != null)
            {
                remove.AddRange(result.Where(station => !station.CanLand(filter.MinPadSize.Value)));
                result.RemoveClear(result);
            }
            return result.Count > 0;
        }
    }
}
