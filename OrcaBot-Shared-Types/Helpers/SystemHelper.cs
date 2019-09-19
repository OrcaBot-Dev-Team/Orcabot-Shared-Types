using OrcaBot.SharedTypes.Enums;
using OrcaBot.SharedTypes;


using System.Collections.Generic;
using System;

namespace OrcaBot.Helpers
{
    public static class SystemHelper
    {
        /// <summary>
        /// Returns a List of all Systems that have a Material Trader of any kind.
        /// </summary>
        /// <returns>A new List of Systems with Material Traders</returns>
        /// <param name="systems">List of Systems</param>
        public static IList<SharedTypes.System> FilterMaterialTrader(this IList<SharedTypes.System> systems )
        {

            var returnList = new List<SharedTypes.System>();
            foreach(var system in systems)
            {
                if(system.HasMatTrader())
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }


        public static bool HasMatTrader(this SharedTypes.System system)
        {
            return (
                    system.Stations.FilterFacility(StationFacility.TraderEncoded).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderManufactured).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderRaw).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderUnknown).Count > 0
                    );
        }
        public static StationFacility GetMatTraderType(this SharedTypes.System system)
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
        public static IList<SharedTypes.System> FilterFacility(this IList<SharedTypes.System> systems, StationFacility stationFacility)
        {
            var returnList = new List<SharedTypes.System>();
            foreach(var system in systems)
            {
                if(system.Stations.FilterFacility(stationFacility).Count > 0)
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }
        public static float DistanceTo(this SharedTypes.System sys,Coordinate coordinate)
        {
            var syscoordinate = sys.Coordinate;
            return syscoordinate.DistanceTo(coordinate);
        }
        public static float DistanceTo(this Coordinate c1,Coordinate c2)
        {
            var X = c1.X - c2.X;
            var Y = c1.Y - c2.Y;
            var Z = c1.Z - c2.Z;
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

    }
}
