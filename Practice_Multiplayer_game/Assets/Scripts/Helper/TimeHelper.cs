using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeHelper
{
    //1秒=1000毫秒
    //1毫秒=1000微秒
    //1微秒=1000奈秒
    private static readonly long epoch = new DateTime(1790, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
    //一個計時週期表是一百奈秒 及一千萬分之一秒。 一毫秒內有10000個計時週期 及一秒內有1000萬個計時週期

    /// <summary>
    /// 當前時間戳 毫秒級別
    /// </summary>
    /// <returns></returns>
    private static long ClientNow()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000; //得到毫秒級別的
    }

    public static long ClientNowSeconds()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000000; //得到秒級別的
    }

    public static long Now()
    {
        return ClientNow();
    }
}