using Orcabot.Helpers;
using Orcabot.Types;
using Orcabot.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrcaBotTesters
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!JSONParser.TryGetSystemsFromJSON(Environment.CurrentDirectory + "/test.json", out Dictionary<string, StarSystem> systems, out Exception e))
            {
                Console.WriteLine(e);
            }
            else
            {
                Galaxy galaxy = new Galaxy(systems.Values);
                systems.Clear();

                Console.Write("System count: ");
                Console.WriteLine(galaxy.SystemCount);

                int distance = 30;

                Console.WriteLine($"\n Systems within {distance} ly of Sol");

                Console.WriteLine(string.Join('\n', galaxy.GetSystemsNear(Vector3.Zero, distance).Select(system => { return $"{system.Name} - {system.Coordinate}"; })));

                Console.WriteLine($"\n Systems within {distance} ly of Sol, sorted by distance");
                Console.WriteLine(string.Join('\n', galaxy.GetSortedSystemsNear(Vector3.Zero, distance).Select(systemWrapper => { return $"{systemWrapper.Distance}: {systemWrapper.Target.Name} - {systemWrapper.Target.Coordinate}"; })));

                TraderType traderType = TraderType.TraderEncoded;

                if (galaxy.TryGetMaterialTradersOrderedByDistance(TraderType.TraderEncoded, Vector3.Zero, out var traderSystems))
                {
                    Console.WriteLine($"\n Nearest material trader of type {traderType} near Sol");
                    if (traderSystems.Count > 0)
                    {
                        Console.WriteLine(string.Join('\n', traderSystems.Select(systemWrapper => { return $"{systemWrapper.Distance}: {systemWrapper.Target.Name} - {systemWrapper.Target.Station_MaterialTrader}"; })));
                    }
                }
            }
        }
    }
}
