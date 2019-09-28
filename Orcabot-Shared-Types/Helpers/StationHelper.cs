using Orcabot.Types.Enums;
using Orcabot.Types;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Orcabot.Helpers
{
    public static class StationHelper
    {
        [Obsolete]
        public static bool HasFacility(this Station station, StationFacility facility)
        {
            return station.Facilities.Contains(facility);
        }

        /// <summary>
        /// Filter the specified stationList by Facility. Only Stations with the given Facility remain.
        /// </summary>
        /// <returns>Subset of the given Station List that have the given Facility</returns>
        /// <param name="stationList">Station list.</param>
        /// <param name="facility">Facility to filter after</param>
        public static IList<Station>FilterFacility(this IList<Station> stationList, StationFacility facility) 
        {
            IList<Station> returnList = new List<Station>();
            foreach(var element in stationList)
            {
                if (element.Facilities.Contains(facility))
                {
                    returnList.Add(element);
                }
            }
            return returnList;
        }

        /// <summary>
        /// Returns wether or not a ship of certain padSize can land at this station
        /// </summary>
        /// <param name="station">The Station to check</param>
        /// <param name="padSize">Check if station has landing pad</param>
        /// <returns></returns>
        [Obsolete]
        public static bool HasLandingPad(this Station station,PadSize padSize)
        {
            var stationPad = station.LargestPadAvailable;
            if(padSize == PadSize.Unknown)
            {
                return false;
            }
            return (padSize <= stationPad);
        }


        public static IList<Station> FilterLandingPad(this IList<Station> stationList, PadSize padSize)
        {
            var returnList = new List<Station>();
            foreach(var element in stationList)
            {
                if (element.CanLand(padSize))
                {
                    returnList.Add(element);
                }
            }
            return returnList;
        }

        [Obsolete]
        public static bool IsPlanetary(this Station s) {
            return s.Type == StationType.SurfaceSettlement || s.Type == StationType.SurfaceStation;
        }

        [Obsolete]
        public static PadSize GetPadSize(this Station s) {
            if(
                s.Type == StationType.AsteroidBase ||
                s.Type == StationType.Coriolis ||
                s.Type == StationType.MegaShip ||
                s.Type == StationType.Ocellus ||
                s.Type == StationType.Orbis ||
                s.Type == StationType.SurfaceStation
                ) {
                return PadSize.Large;
            }else if(
                s.Type == StationType.Outpost
                ) {
                return PadSize.Medium;
            }
            else {
                return PadSize.None;
            }
        }

        [Obsolete]
        public static bool HasMatTrader(this Station s) {
            return s.HasFacility(StationFacility.TraderRaw) || s.HasFacility(StationFacility.TraderManufactured) || s.HasFacility(StationFacility.TraderEncoded);
        }

        [Obsolete]
        public static TraderType GetMatTrader(this Station s)
        {
            foreach (StationFacility fac in s.Facilities)
            {
                if (fac > StationFacility.TraderUnknown)
                {
                    return fac.ToTraderType();
                }
            }
            return TraderType.NoTrader;
        }

        [Obsolete]
        public static RelevantStationType GetRelevantStationType(this Station s)
        {
            if (s.Type < StationType.SurfaceStation)
            {
                return RelevantStationType.Unlandable;
            }
            else if (s.Type < StationType.Outpost)
            {
                return RelevantStationType.Planetary;
            }
            else if (s.Type < StationType.MegaShip)
            {
                return RelevantStationType.OrbitalMedium;
            }
            else
            {
                return RelevantStationType.OrbitalLarge;
            }
        }

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
            }
            if (filter.Facility != null)
            {
                remove.AddRange(result.Where(station => !station.HasFacility(filter.Facility.Value)));
            }
            if (filter.MinPadSize != null)
            {
                remove.AddRange(result.Where(station => !station.CanLand(filter.MinPadSize.Value)));
            }
            result.RemoveRange(remove);
            return result.Count > 0;
        }
    }
}
