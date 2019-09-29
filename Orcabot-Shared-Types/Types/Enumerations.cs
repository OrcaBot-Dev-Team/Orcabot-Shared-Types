using System;
namespace Orcabot.Types.Enums
{
    /// <summary>
    /// Gives the maximum Pad size at a facility. Although Values like "Small" are not used ingame, they might become relevant in the future.
    /// </summary>
    public enum PadSize : byte
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
    public enum StationFacility : byte
    {
        Unknown = 0,
        Market = 1,
        InterstellarFactors = 2,
        Refuel = 3,
        Repair = 4,
        Restock = 5,
        Shipyard = 6,
        BlackMarket = 7,
        Outfitting = 8,
        Contacts = 9,
        UniversalCartographics = 10,
        Missions = 11,
        CrewLounge = 12,
        RemoteEngineering = 13,
        SearchAndResque = 14,
        TechnologyBroker = 15,
        TraderUnknown = 16,
        TraderEncoded = 18,
        TraderRaw = 19,
        TraderManufactured = 20,
    }
    public enum TraderType : byte
    {
        NoTrader = 0,
        TraderUnknown = 16,
        TraderEncoded = 18,
        TraderRaw = 19,
        TraderManufactured = 20,
    }
    /// <summary>
    /// Defines a system's/station's economy.
    /// </summary>
    public enum Economy : byte
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
    public enum SystemSecurity : byte
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
    public enum StationType : byte
    {
        Unknown = 0,
        Installation = 1,
        SurfaceSettlement = 2,
        SurfaceStation = 3,
        Outpost = 4,
        MegaShip = 5,
        Coriolis = 6,
        Ocellus = 7,
        Orbis = 8,
        AsteroidBase = 9
    }
    public enum RelevantStationType : byte
    {
        Unknown = 0,
        Unlandable = 1,
        Planetary = 2,
        OrbitalMedium = 3,
        OrbitalLarge = 4
    }
    namespace Ranks
    {
        public enum Combat : sbyte
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
        public enum Trade : sbyte
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
        public enum Explore : sbyte
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
        public enum CQC : sbyte
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
        public enum Federation : sbyte
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
        public enum Empire : sbyte
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
