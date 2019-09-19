using System;
namespace OrcaBot.SharedTypes.Enums
{
    /// <summary>
    /// Gives the maximum Pad size at a facility. Although Values like "Small" are not used ingame, they might become relevant in the future.
    /// </summary>
    public enum PadSize
    {
        Unknown = 0,
        None = 1,
        Small = 2,
        Medium = 3,
        Large = 4
    }
    /// <summary>
    /// Represents a Station Facility. Unknown is for when a facility cannot be parsed because of a wrong format
    /// </summary>
    public enum StationFacility
    {
        Unknown = 0,
        Market,
        InterstellarFactors,
        Refuel,
        Repair,
        Restock,
        Shipyard,
        TraderUnknown,
        TraderEncoded,
        TraderRaw,
        TraderManufactured,
        BlackMarket,
        Outfitting,
        Contacts,
        UniversalCartographics,
        Missions,
        CrewLounge,
        RemoteEngineering,
        SearchAndResque,
        TechnologyBroker
    }
    /// <summary>
    /// Defines a system's/station's economy.
    /// </summary>
    public enum Economy
    {
        Unknown = 0,
        Colony,
        Extraction,
        HighTech,
        Industrial,
        Military,
        Refinery,
        Service,
        Terraforming,
        Tourism,
        PrisonColony,
        Repair,
        Rescue,
        Damaged
    }
    public enum Security
    {
        Unknown = 0,
        Low = 16,
        Medium = 32,
        High = 48,
        Anarchy = 64 
    }
    /// <summary>
    /// Defines the Station type. Some of these might not be used (not written to the JSON)
    /// </summary>
    public enum StationType
    {
        Unknown = 0,
        Coriolis,
        Ocellus,
        Orbis,
        Outpost,
        AsteroidBase,
        Installation,
        MegaShip,
        SurfaceStation,
        SurfaceSettlement
    }
}
