using System;
using System.Collections.Generic;
using Orcabot.Types.Enums;
namespace Orcabot.Types
{
    public class System
    {
        public string Name { get; set; }
        public Coordinate Coordinate { get; set; }
        public List<Station> Stations { get; set; }
        public Security SystemSecurity { get; set; }
        public System()
        {
            Stations = new List<Station>();
        }
    }
    public struct Coordinate
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
