using Orcabot.Types.Enums;
using Orcabot.Types;

using System;
using System.Collections.Generic;

namespace Orcabot.Helpers
{
    public static class StationHelper
    {
        public static bool HasFacility(this Station station, StationFacility facility)
        {
            return station.StationFacilities.Contains(facility);
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
                if (element.HasFacility(facility))
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
        public static bool HasLandingPad(this Station station,PadSize padSize)
        {
            var stationPad = station.GetPadSize();
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
                if (element.HasLandingPad(padSize))
                {
                    returnList.Add(element);
                }
            }
            return returnList;
        }
        public static bool IsPlanetary(this Station s) {
            return s.Type == StationType.SurfaceSettlement || s.Type == StationType.SurfaceStation;
        }
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
        
    }
}
