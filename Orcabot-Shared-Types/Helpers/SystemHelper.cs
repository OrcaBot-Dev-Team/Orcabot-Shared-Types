using Orcabot.Types.Enums;
using Orcabot.Types;


using System.Collections.Generic;
using System;
using Sys = Orcabot.Types.System;
using System.Linq;

namespace Orcabot.Helpers
{
    public static class SystemHelper
    {
        /// <summary>
        /// Returns a List of all Systems that have a Material Trader of any kind.
        /// </summary>
        /// <returns>A new List of Systems with Material Traders</returns>
        /// <param name="systems">List of Systems</param>
        public static IList<Sys> FilterMaterialTrader(this IList<Sys> systems )
        {

            var returnList = new List<Types.System>();
            foreach(var system in systems)
            {
                if(system.HasMatTrader())
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }
        public static Dictionary<string,Sys> FilterMaterialTraders(this Dictionary<string,Sys> dict, bool keepTraderStationsOnly) {
            var returnDict = new Dictionary<string, Sys>();
            foreach (var entry in dict) {
                if (entry.Value.HasMatTrader()) {
                    var system = entry.Value;
                    if (keepTraderStationsOnly) {
                        foreach (var station in system.Stations) {
                            if (!station.HasFacility(StationFacility.TraderEncoded) && !station.HasFacility(StationFacility.TraderManufactured) && !station.HasFacility(StationFacility.TraderRaw)) {
                                system.Stations.Remove(station);
                            }
                        }
                    }

                    returnDict.Add(entry.Key, system);
                }
            }
            return returnDict;
        }
        public static Sys RemoveIrrelevantStations (this Sys sys) {
            List<Station> stations = sys.Stations.OrderBy(s => s.Distance).ToList();
            Station L = null, M = null, Planet = null, MatTrader = null;
            foreach(var station in stations) {
                if (station.HasMatTrader()) {
                    MatTrader = station;
                }
                if (station.IsPlanetary()) {
                    if(Planet == null) {
                        Planet = station;
                    }
                }
                else if(station.GetPadSize() == PadSize.Large) {
                    if(L == null) {
                        L = station;
                    }
                }
                else if(station.GetPadSize() == PadSize.Medium){
                    if(M == null) {
                        M = station;
                    }
                }
                if(L != null && M != null && Planet != null) {
                    if (!sys.HasMatTrader())
                        break;
                    else if (MatTrader != null)
                        break; 
                }
            }
            //It's the same station, so one reference can be removed
            if(L == MatTrader) {
                MatTrader = null;
            }
            //L M and Planet now have the closest Stations
            if(L.Distance < M.Distance) {
                M = null; //Medium Station is irrelevant in this case due to the Large being closer.
            }
            if(L.Distance < Planet.Distance) {
                Planet = null; //Planetary is irrelevant in this case due to the large being closer.
            }
            List<Station> filteredStations = new List<Station>();
            var arr = new Station[]{
                L,M,Planet,MatTrader
            };
            foreach(var s in arr) {
                if(s != null) {
                    filteredStations.Add(s);
                }
            }
            sys.Stations = filteredStations;
            return sys;
        }

 

        public static bool HasMatTrader(this Sys system)
        {
            return (
                    system.Stations.FilterFacility(StationFacility.TraderEncoded).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderManufactured).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderRaw).Count > 0 ||
                    system.Stations.FilterFacility(StationFacility.TraderUnknown).Count > 0
                    );
        }
        public static StationFacility GetMatTraderType(this Types.System system)
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
        public static IList<Types.System> FilterFacility(this IList<Types.System> systems, StationFacility stationFacility)
        {
            var returnList = new List<Types.System>();
            foreach(var system in systems)
            {
                if(system.Stations.FilterFacility(stationFacility).Count > 0)
                {
                    returnList.Add(system);
                }
            }
            return returnList;
        }
        public static float DistanceTo(this Types.System sys,Coordinate coordinate)
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
