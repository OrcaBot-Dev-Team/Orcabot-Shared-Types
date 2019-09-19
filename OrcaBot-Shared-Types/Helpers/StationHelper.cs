using OrcaBot.SharedTypes.Enums;
using OrcaBot.SharedTypes;

using System;
using System.Collections.Generic;

namespace OrcaBot.Helpers
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
        /// <returns>The filter.</returns>
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


        public static bool HasLandingPad(this Station station,PadSize padSize)
        {   
            if(padSize == PadSize.Unknown)
            {
                return false;
            }
            return (padSize <= station.PadSize);
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
    }
}
