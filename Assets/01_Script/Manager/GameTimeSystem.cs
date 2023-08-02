using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameTimeSystem
{
    public static DateTime m_UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private static DateTimeOffset m_ParseTime;

    public static float m_StopTime = 0;

    public static void SetTime()
    {
        m_ParseTime = System.DateTime.UtcNow;
    }

    public static TimeSpan GetTime()
    {
        return System.DateTime.UtcNow - m_ParseTime;
    }
}
