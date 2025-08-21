using System.Collections.Generic;
using System;

namespace GIKCore.Utilities
{
    public class ITimeSpanFormat
    {
        /// <summary>Example: 1 second => 1</summary>
        public const string s = @"%s";
        /// <summary>Example: 1 seconds => 01</summary>
        public const string ss = @"ss";
        /// <summary>Example: 1 minute => 1</summary>
        public const string m = @"%m";
        /// <summary>Example: 1 minute => 01</summary>
        public const string mm = @"mm";
        /// <summary>Example: 1 hour => 1</summary>
        public const string h = @"%h";
        /// <summary>Example: 1 hour => 01</summary>
        public const string hh = @"hh";
        /// <summary>Example: 1 minute : 9 seconds => 01:09</summary>
        public const string mm_ss = @"mm\:ss";
        /// <summary>Example: 1 hour : 9 minutes => 01:09</summary>
        public const string hh_mm = @"hh\:mm";
        /// <summary>Example: 1 hour : 9 minutes : 9 seconds => 01:09:09</summary>
        public const string hh_mm_ss = @"hh\:mm\:ss";
    }
    public class IDateTimeFormat
    {
        /// <summary>Example: 14:05 20/07/2022</summary>
        public const string HH_mm_dd_MM_yyyy = "HH:mm dd/MM/yyyy";

        /// <summary>Example: 02:05 PM</summary>
        public const string hh_mm_ampm = "hh:mm tt";
        /// <summary>Example: 02:05 PM 20/07/22</summary>
        public const string hh_mm_ampm_dd_MM_yy = "hh:mm tt dd/MM/yy";
        /// <summary>Example:<br>02:05 PM<br>20/07/22</br></br></summary>
        public const string hh_mm_ampm_br_dd_MM_yy = "hh:mm tt\ndd/MM/yy";
        /// <summary>Example: 02:05 PM 20/07/2022</summary>
        public const string hh_mm_ampm_dd_MM_yyyy = "hh:mm tt dd/MM/yyyy";

        /// <summary>Example: 20/07</summary>
        public const string dd_MM = "dd/MM";
        /// <summary>Example: 20/07/22</summary>
        public const string dd_MM_yy = "dd/MM/yy";
        /// <summary>Example: 20/07/2022 </summary>
        public const string dd_MM_yyyy = "dd/MM/yyyy";

        /// <summary>Example: 20/07 14:05</summary>
        public const string dd_MM_HH_mm = "dd/MM HH:mm";
        /// <summary>Example: 20/07/22 14:05</summary>
        public const string dd_MM_yy_HH_mm = "dd/MM/yy HH:mm";
        /// <summary>Example: 20/07/2022 14:05</summary>
        public const string dd_MM_yyyy_HH_mm = "dd/MM/yyyy HH:mm";

        /// <summary>Example: 20/07 02:05 PM</summary>
        public const string dd_MM_hh_mm_ampm = "dd/MM hh:mm tt";
        /// <summary>Example: 20/07/22 02:05 PM</summary>
        public const string dd_MM_yy_hh_mm_ampm = "dd/MM/yy hh:mm tt";
        /// <summary>Example: 20/07/2022 02:05 PM</summary>
        public const string dd_MM_yyyy_hh_mm_ampm = "dd/MM/yyyy hh:mm tt";
    }
    public class ITimer
    {
        private static DateTime Jan1st1970Utc { get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); } }
        public static long UtcNowTicks { get { return DateTime.UtcNow.Ticks; } }
        public static long GetTimestampInMilliseconds(DateTime? utc = null)
        {
            if (utc == null) utc = DateTime.UtcNow;
            return (long)(utc.Value - Jan1st1970Utc).TotalMilliseconds;
        }
        public static long GetTimestampInSeconds(DateTime? utc = null)
        {
            if (utc == null) utc = DateTime.UtcNow;
            return (long)(utc.Value - Jan1st1970Utc).TotalSeconds;
        }
        public static long GetTimeDeltaFromNowInSeconds(long endSeconds)
        {
            DateTime dateStart = DateTime.UtcNow;
            DateTime dateEnd = GetUtcTimeFromTimestamp(endSeconds);

            long delta = (long)(dateEnd - dateStart).TotalSeconds;
            return Math.Abs(delta);
        }
        public static long GetTimeDeltaFromNowInMilliseconds(long endMilliseconds)
        {
            DateTime dateStart = DateTime.UtcNow;
            DateTime dateEnd = GetUtcTimeFromTimestamp2(endMilliseconds);

            long delta = (long)(dateEnd - dateStart).TotalMilliseconds;
            return Math.Abs(delta);
        }
        public static long GetTimePassInSeconds(long lastTicks)
        {
            long delta = UtcNowTicks - lastTicks;
            long seconds = (long)TimeSpan.FromTicks(delta).TotalSeconds;
            return seconds;
        }
        public static long GetTimePassInSeconds2(DateTime utc)
        {
            return GetTimePassInSeconds(utc.Ticks);
        }
        public static long GetTimePassInMilliseconds(long lastTicks)
        {
            long delta = UtcNowTicks - lastTicks;
            long milliseconds = (long)TimeSpan.FromTicks(delta).TotalMilliseconds;
            return milliseconds;
        }
        public static long GetTimePassInMilliseconds2(DateTime utc)
        {
            return GetTimePassInMilliseconds(utc.Ticks);
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-timespan-format-strings?redirectedfrom=MSDN
        /// </summary>
        /// <param name="remainSeconds"></param>
        /// <param name="timeSpanFormat">Should begin with @; example: @"hh\:mm"</param>
        /// <returns></returns>
        public static string FormatTimeSpan(long remainSeconds, string timeSpanFormat = ITimeSpanFormat.s)
        {
            TimeSpan ts = TimeSpan.FromSeconds(remainSeconds);
            return ts.ToString(timeSpanFormat);
        }
        public static string FormatTimeSpanMilliseconds(long remainMilliseconds, string timeSpanFormat = ITimeSpanFormat.s)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(remainMilliseconds);
            return ts.ToString(timeSpanFormat);
        }

        /// <summary>
        /// https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="dateTimeFormat"></param>
        /// <returns></returns>
        public static string FormatTimestampToLocalTime(long seconds, string dateTimeFormat = IDateTimeFormat.HH_mm_dd_MM_yyyy)
        {
            DateTime local = GetLocalTimeFromTimestamp(seconds);
            return local.ToString(dateTimeFormat);
        }

        public static DateTime GetLocalTimeFromTimestamp(long seconds)
        {
            DateTime utc = Jan1st1970Utc.AddSeconds(seconds);
            DateTime local = utc.ToLocalTime();
            return local;
        }
        public static DateTime GetUtcTimeFromTimestamp(long seconds)
        {
            DateTime utc = Jan1st1970Utc.AddSeconds(seconds);
            return utc;
        }
        public static DateTime GetUtcTimeFromTimestamp2(long milliseconds)
        {
            DateTime utc = Jan1st1970Utc.AddMilliseconds(milliseconds);
            return utc;
        }

        public static bool CheckSameDate(DateTime date1, DateTime date2)
        {
            return (date1 != null && date2 != null) ? date1.Date == date2.Date : false;
        }
        public static bool CheckSameYear(DateTime date1, DateTime date2)
        {
            return (date1 != null && date2 != null) ? date1.Year == date2.Year : false;
        }
        public static bool CheckLocalThisYear(DateTime local)
        {
            return CheckSameYear(local, DateTime.Today);
        }
        public static bool CheckLocalToday(DateTime local)
        {
            return CheckSameDate(local, DateTime.Today);
        }
        public static List<long> GetTimestampRangeInSeconds(long secondsStart, long secondsEnd, long secondsOffset = 24 * 60 * 60, bool stopInUtcNow = true)
        {
            if (stopInUtcNow)
            {
                long secondsNow = GetTimestampInSeconds();
                if (secondsEnd > secondsNow) secondsEnd = secondsNow;
            }

            List<long> lstTimestamp = new List<long>();
            for (long seconds = secondsStart; seconds <= secondsEnd; seconds += secondsOffset)
            {
                lstTimestamp.Add(seconds);
            }
            return lstTimestamp;
        }

        public static void UpdateTimeDeltaSeconds(ref ITimeDelta timeDelta, ICallback.CallFunc2<long> onValueChanged, ICallback.CallFunc onFailed = null)
        {
            if (timeDelta > 0)
            {
                timeDelta.MakeTimePassInSeconds();
                onValueChanged?.Invoke(timeDelta.time);
            }
            else
            {
                onFailed?.Invoke();
            }
        }
        public static void UpdateTimeDeltaMilliseconds(ref ITimeDelta timeDelta, ICallback.CallFunc2<long> onValueChanged, ICallback.CallFunc onFailed = null)
        {
            if (timeDelta > 0)
            {
                timeDelta.MakeTimePassInMilliseconds();
                onValueChanged?.Invoke(timeDelta.time);
            }
            else
            {
                onFailed?.Invoke();
            }
        }
    }
    public class ITimeCache
    {
        private long time = 60;//in seconds or milliseconds
        private long lastTicks = 0;//in milliseconds
        private bool isMilliseconds = false;

        public ITimeCache(long delta = 60, bool renew = false, bool isMilliseconds = false) { SetCache(delta, renew, isMilliseconds); }

        public void SetCache(long delta, bool renew = false, bool isMilliseconds = false)
        {
            time = delta;
            this.isMilliseconds = isMilliseconds;
            if (renew) Renew();
        }
        public long GetCache() { return time; }
        public long GetCacheRemainInSeconds()
        {
            long remain = isMilliseconds ? (time / 1000) : time;//convert to seconds
            long timePass = ITimer.GetTimePassInSeconds(lastTicks);
            if (timePass > 0) remain -= timePass;
            if (remain <= 0) remain = 0;

            return remain;
        }
        public long GetCacheRemainInMilliseconds()
        {
            long remain = isMilliseconds ? time : (time * 1000);//convert to milliseconds
            long timePass = ITimer.GetTimePassInMilliseconds(lastTicks);
            if (timePass > 0) remain -= timePass;
            if (remain <= 0) remain = 0;

            return remain;
        }
        public void Renew() { lastTicks = DateTime.UtcNow.Ticks; }
        public void Stop() { lastTicks = -1; }

        public bool CheckExpired(bool autoRenew = true)
        {
            bool expired = false;
            if (lastTicks <= 0) expired = true;
            else
            {
                long remain = GetCacheRemainInMilliseconds();
                if (remain <= 0) expired = true;
            }

            if (expired && autoRenew) Renew();
            return expired;
        }

        public void ForceExpired() { lastTicks = 0; }
    }
    public struct ITimeDelta
    {
        public long time { get; private set; }//in seconds or milliseconds
        private long lastTicks;//in milliseconds

        public ITimeDelta(long value)
        {
            time = value;
            lastTicks = DateTime.UtcNow.Ticks;
        }

        public static implicit operator ITimeDelta(long value)
        {
            ITimeDelta td = new ITimeDelta(value);
            return td;
        }

        public static ITimeDelta operator -(ITimeDelta source, long timePass)
        {
            source.time -= timePass;
            if (source.time <= 0) source.time = 0;
            source.lastTicks = DateTime.UtcNow.Ticks;
            return source;
        }

        public static ITimeDelta operator +(ITimeDelta source, long timePass)
        {
            source.time += timePass;
            source.lastTicks = DateTime.UtcNow.Ticks;
            return source;
        }

        public static bool operator >(ITimeDelta source, long value)
        {
            return source.time > value;
        }

        public static bool operator <(ITimeDelta source, long value)
        {
            return source.time < value;
        }

        public static bool operator ==(ITimeDelta source, long value)
        {
            return source.time == value;
        }

        public static bool operator !=(ITimeDelta source, long value)
        {
            return source.time != value;
        }

        public static bool operator >=(ITimeDelta source, long value)
        {
            return source.time >= value;
        }

        public static bool operator <=(ITimeDelta source, long value)
        {
            return source.time <= value;
        }

        public override bool Equals(object obj)
        {
            if (obj is ITimeDelta)
                return this == ((ITimeDelta)obj).time;
            else if (obj is long)
                return this == (long)obj;
            else if (obj is int)
                return this == (int)obj;
            return false;
        }

        public override int GetHashCode()
        {
            return time.GetHashCode() ^ lastTicks.GetHashCode();
        }

        public long timePassInSeconds
        {
            get
            {
                long delta = DateTime.UtcNow.Ticks - lastTicks;
                long seconds = (long)TimeSpan.FromTicks(delta).TotalSeconds;
                return seconds;
            }
        }

        public long timePassInMilliseconds
        {
            get
            {
                long delta = DateTime.UtcNow.Ticks - lastTicks;
                long milliseconds = (long)TimeSpan.FromTicks(delta).TotalMilliseconds;
                return milliseconds;
            }
        }

        public void MakeTimePassInSeconds()
        {
            long timePass = timePassInSeconds;
            if (timePass > 0) this -= timePass;
        }
        public void MakeTimePassInMilliseconds()
        {
            long timePass = timePassInMilliseconds;
            if (timePass > 0) this -= timePass;
        }

        public void MakeTimeNextInSeconds()
        {
            long timePass = timePassInSeconds;
            if (timePass > 0) this += timePass;
        }
        public void MakeTimeNextInMilliseconds()
        {
            long timePass = timePassInMilliseconds;
            if (timePass > 0) this += timePass;
        }

        public void UpdateInSeconds(ICallback.CallFunc2<long> onValueChanged, ICallback.CallFunc onFailed = null)
        {
            if (time > 0)
            {
                MakeTimePassInSeconds();
                onValueChanged?.Invoke(time);
            }
            else
            {
                onFailed?.Invoke();
            }
        }
        public void UpdateNextInSeconds(ICallback.CallFunc2<long> onValueChanged)
        {
            if (time >= 0)
            {
                MakeTimeNextInSeconds();
                onValueChanged?.Invoke(time);
            }
        }

        public void UpdateInMilliseconds(ICallback.CallFunc2<long> onValueChanged, ICallback.CallFunc onFailed = null)
        {
            if (time > 0)
            {
                MakeTimePassInMilliseconds();
                onValueChanged?.Invoke(time);
            }
            else
            {
                onFailed?.Invoke();
            }
        }
        public void UpdateNextInMilliseconds(ICallback.CallFunc2<long> onValueChanged)
        {
            if (time >= 0)
            {
                MakeTimeNextInMilliseconds();
                onValueChanged?.Invoke(time);
            }
        }
    }
}