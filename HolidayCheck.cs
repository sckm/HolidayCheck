using System;

namespace Manyousen
{
    public class HolidayCheck
    {
        // 祝日かどうか判定
        public static Boolean isHoliday(DateTime dateTime)
        {
            if (isHolidayNotSubstitute(dateTime))
            {
                return true;
            }
            else if (isSubstituteHoliday(dateTime))
            {
                return true;
            }
            else if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 振替休日以外の祝日か判定
        public static Boolean isHolidayNotSubstitute(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;

            // 日付が決まっている祝日
            if (month == 1 && day == 1)
            {
                return true;
            }
            else if (month == 2 && day == 11)
            {
                return true;
            }
            else if (month == 4 && day == 29)
            {
                return true;
            }
            else if (month == 5)
            {
                if (day == 3 || day == 4 || day == 5)
                    return true;
            }
            else if (month == 11)
            {
                if (day == 3 || day == 23)
                    return true;
            }
            else if (month == 12 && day == 23)
            {
                return true;
            }

            // 第○週の○曜日の祝日
            // 7月第3月曜　海の日
            if (month == 1 && dayOfWeek == DayOfWeek.Monday)
            {
                if (getWeekOfMonth(dateTime) == 2)
                    return true;

            }
            else if (month == 7 && dayOfWeek == DayOfWeek.Monday)
            {
                if (getWeekOfMonth(dateTime) == 3)
                    return true;
            }
            // 9月第3月曜 敬老の日
            else if (month == 9 && dayOfWeek == DayOfWeek.Monday)
            {
                if (getWeekOfMonth(dateTime) == 3)
                    return true;
            }
            // 10月第2月曜 体育の日
            else if (month == 10 && dayOfWeek == DayOfWeek.Monday)
            {
                if (getWeekOfMonth(dateTime) == 2)
                    return true;
            }

            // 春分の日
            if (month == 3)
            {
                int syunbunDay = getSyunbunDay(year);
                if (day == syunbunDay)
                    return true;
            }
            // 秋分の日
            else if (month == 9)
            {
                int syuubunDay = getSyuubunDay(year);
                if (day == syuubunDay)
                    return true;

                // 敬老の日(9月第3月曜)と秋分の日の間の日の場合も祝日
                // →前日が敬老の日
                // →翌日が秋分の日
                if (day == syuubunDay - 1 && getWeekOfMonth(dateTime.AddDays(-1)) == 3 && dateTime.DayOfWeek == DayOfWeek.Tuesday)
                    return true;
            }

            return false;
        }

        // 振替休日か判定
        public static Boolean isSubstituteHoliday(DateTime dateTime)
        {
            // 1.前日が祝日か
            // 2.祝日の時、その日が日曜日か
            // 3.日曜日じゃなければその前の日が祝日か
            DateTime previousDayDateTime = dateTime.AddDays(-1);
            while (isHolidayNotSubstitute(previousDayDateTime))
            {
                if (previousDayDateTime.DayOfWeek == DayOfWeek.Sunday)
                    return true;
                else
                    previousDayDateTime = previousDayDateTime.AddDays(-1);
            }


            return false;
        }

        // 冬季か判定
        public static Boolean isWinter(DateTime dateTime)
        {
            return isWinter(dateTime, 12, 31, 3, 31);
        }
        public static Boolean isWinter(DateTime dateTime, int startWinterMonth, int startWinterDay, int endWinterMonth, int endWinterDay)
        {
            if (startWinterMonth <= endWinterMonth)
            {
                if (startWinterMonth == dateTime.Month)
                {
                    if (startWinterDay <= dateTime.Day)
                        return true;
                }
                else if (endWinterMonth == dateTime.Month)
                {
                    if (dateTime.Day <= endWinterDay)
                        return true;
                }
                else if (startWinterMonth + 1 <= dateTime.Month && dateTime.Month <= 12)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (startWinterMonth == dateTime.Month)
                {
                    if (startWinterDay <= dateTime.Day)
                        return true;
                }
                else if (dateTime.Month == endWinterMonth)
                {
                    if (dateTime.Day <= endWinterMonth)
                        return true;
                }
                else if (startWinterMonth + 1 <= dateTime.Month && dateTime.Month <= 12)
                {
                    return true;
                }
                else if (1 <= dateTime.Month && dateTime.Month <= endWinterMonth - 1)
                {
                    return true;
                }


                return false;

            }
        }

        // 月の何週目かを調べて返す
        private static int getWeekOfMonth(DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;

            int day = dateTime.Day;
            int weekOfMonth = 0;


            weekOfMonth = (day - 1) / 7 + 1;

            return weekOfMonth;
        }

        // 春分の日(3月)
        // 1990年～2099年まで対応 (ずれる可能性あり)
        private static int getSyunbunDay(int year)
        {
            int r = year % 4;
            if (1990 <= year && year <= 2099)
            {
                if (r == 0)
                {
                    if (1900 <= year && year <= 1956)
                        return 21;
                    else if (1960 <= year && year <= 2088)
                        return 20;
                    else if (2092 <= year && year <= 2096)
                        return 19;
                }
                else if (r == 1)
                {
                    if (1901 <= year && year <= 1989)
                        return 21;
                    else if (1993 <= year && year <= 2097)
                        return 20;
                }
                else if (r == 2)
                {
                    if (1902 <= year && year <= 2022)
                        return 21;
                    else if (2026 <= year && year <= 2098)
                        return 20;
                }
                else if (r == 3)
                {
                    if (1903 <= year && year <= 1923)
                        return 22;
                    else if (1927 <= year && year <= 2055)
                        return 21;
                    else if (2059 <= year && year <= 2099)
                        return 20;
                }
            }

            return 21;
        }

        // 秋分の日(9月)
        // 1990年～2099年まで対応 (ずれる可能性あり)
        private static int getSyuubunDay(int year)
        {
            int r = year % 4;
            if (1990 <= year && year <= 2099)
            {
                if (r == 0)
                {
                    if (1900 <= year && year <= 2008)
                        return 23;
                    else if (2012 <= year && year <= 2096)
                        return 22;
                }
                else if (r == 1)
                {
                    if (1901 <= year && year <= 1917)
                        return 24;
                    else if (1921 <= year && year <= 2041)
                        return 23;
                    else if (2045 <= year && year <= 2097)
                        return 22;

                }
                else if (r == 2)
                {
                    if (1902 <= year && year <= 1946)
                        return 24;
                    else if (1950 <= year && year <= 2074)
                        return 23;
                    else if (2078 <= year && year <= 2098)
                        return 22;
                }
                else if (r == 3)
                {
                    if (1903 <= year && year <= 1979)
                        return 24;
                    else if (1983 <= year && year <= 2099)
                        return 23;
                }
            }

            return 23;
        }

        //　日曜日か判定
        private static Boolean isSunday(DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                return true;
            else
                return false;
        }

    }
}
