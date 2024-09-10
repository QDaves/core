﻿using System;

namespace Xabbo.Core;

public enum BanDuration
{
    Hour,
    Day,
    Permanent
}

public static partial class XabboEnumExtensions
{
    public static string ToClientString(this BanDuration banDuration, ClientType client) => client switch
    {
        ClientType.Shockwave => banDuration switch
        {
            BanDuration.Hour => "ban_user_hour",
            BanDuration.Day => "ban_user_day",
            BanDuration.Permanent => "ban_user_perm",
            _ => throw new Exception($"Unknown {client} ban duration: '{banDuration}'.")
        },
        not ClientType.Shockwave => banDuration switch
        {
            BanDuration.Hour => "RWUAM_BAN_USER_HOUR",
            BanDuration.Day => "RWUAM_BAN_USER_DAY",
            BanDuration.Permanent => "RWUAM_BAN_USER_PERM",
            _ => throw new Exception($"Unknown {client} ban duration: '{banDuration}'.")
        }
    };
}
