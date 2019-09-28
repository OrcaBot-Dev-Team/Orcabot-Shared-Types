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
    }
}
