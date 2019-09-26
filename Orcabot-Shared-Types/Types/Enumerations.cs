using System;
namespace Orcabot.Types.Enums
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
        Damaged,
        Agriculture
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
    namespace Ranks
    {
        public enum Combat
        {
            Unknown = -1,
            Harmless,
            MostlyHarmless,
            Novice,
            Competent,
            Expert,
            Master,
            Dangerous,
            Deadly,
            Elite
        }
        public enum Trade
        {
            Unknown = -1,
            Penniless,
            MostlyPenniless,
            Peddler,
            Dealer,
            Merchant,
            Broker,
            Entrepreneur,
            Tycoon,
            Elite
        }
        public enum Explore
        {
            Unknown = -1,
            Aimless,
            MostlyAimless,
            Scout,
            Surveyor,
            Trailblazer,
            Pathfinder,
            Ranger,
            Pioneer,
            Elite
        }
        public enum CQC
        {
            Unknown = -1,
            Helpless,
            MostlyHelpless,
            Amateur,
            SemiProfessional,
            Professional,
            Champion,
            Hero,
            Legend,
            Elite
        }
        public enum Federation
        {
            Unknown = -1,
            None,
            Recruit,
            Cadet,
            Midshipman,
            PettyOfficer,
            ChiefPettyOfficer,
            WarrantOfficer,
            Ensign,
            Lieutenant,
            LieutenantCommander,
            PostCommander,
            RearAdmiral,
            ViceAdmiral,
            Admiral
        }
        public enum Empire
        {
            Unknown = -1,
            None,
            Outsider,
            Serf,
            Master,
            Squire,
            Knight,
            Lord,
            Baron,
            Viscount,
            Count,
            Earl,
            Marquis,
            Duke,
            Prince,
            King
        }
    }
}
