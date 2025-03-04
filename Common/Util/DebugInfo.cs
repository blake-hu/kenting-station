using System;

namespace KentingStation.Common.Util;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DebugInfo : Attribute
{
    public DebugInfo(string displayName = null)
    {
        DisplayName = displayName;
    }

    public string DisplayName { get; }
}