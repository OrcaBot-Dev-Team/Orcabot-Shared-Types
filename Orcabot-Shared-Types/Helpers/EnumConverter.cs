using Orcabot.Types.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orcabot.Helpers
{
    public static class EnumConverter
    {
        public static TraderType ToTraderType(this StationFacility fac)
        {
            if (fac < StationFacility.TraderUnknown)
            {
                return TraderType.NoTrader;
            }
            else
            {
                return (TraderType)fac;
            }
        }

        public static PadSize ToPadSize(this RelevantStationType type)
        {
            switch (type)
            {
                case RelevantStationType.Planetary:
                case RelevantStationType.OrbitalLarge:
                    return PadSize.Large;
                case RelevantStationType.OrbitalMedium:
                    return PadSize.Medium;
                case RelevantStationType.Unlandable:
                    return PadSize.None;
                default:
                    return PadSize.Unknown;
            }
        }

        public static PadSize ToPadSize(this StationType type)
        {
            if (type < StationType.SurfaceStation)
            {
                return PadSize.None;
            }
            else if (type < StationType.Outpost)
            {
                return PadSize.Large;
            }
            else if (type < StationType.MegaShip)
            {
                return PadSize.Medium;
            }
            else
            {
                return PadSize.Large;
            }
        }

        public static RelevantStationType ToRelevantStationType(this StationType type)
        {
            if (type < StationType.SurfaceStation)
            {
                return RelevantStationType.Unlandable;
            }
            else if (type < StationType.Outpost)
            {
                return RelevantStationType.Planetary;
            }
            else if (type < StationType.MegaShip)
            {
                return RelevantStationType.OrbitalMedium;
            }
            else
            {
                return RelevantStationType.OrbitalLarge;
            }
        }
    }
}
