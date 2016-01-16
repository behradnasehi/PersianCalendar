using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text.RegularExpressions;

/*
 http://www.codeproject.com/Articles/28129/Creating-a-CLR-Persian-Date-Convertor-Function-for
 */

public enum Date_part
{
    YEAR,
    Month,
    DAY
}

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString ToPersianDateTime(DateTime dt)
    {
        try
        {
            string result = "";
            if (dt != null)
            {
                PersianCalendar objPersianCalendar = new PersianCalendar();
                int year = objPersianCalendar.GetYear(dt);
                int month = objPersianCalendar.GetMonth(dt);
                int day = objPersianCalendar.GetDayOfMonth(dt);
                int hour = objPersianCalendar.GetHour(dt);
                int min = objPersianCalendar.GetMinute(dt);
                int sec = objPersianCalendar.GetSecond(dt);
                result = year.ToString().PadLeft(4, '0') + "/" +
                         month.ToString().PadLeft(2, '0') + "/" +
                         day.ToString().PadLeft(2, '0') + " " +
                         hour.ToString().PadLeft(2, '0') + ":" +
                min.ToString().PadLeft(2, '0') + ":" +
                                       sec.ToString().PadLeft(2, '0');
            }
            return new SqlString(result);
        }
        catch (Exception e)
        {
            return SqlString.Null;
        }
    }
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString ToPersianDate(DateTime dt)
    {
        try
        {
            string result = "";
            if (dt != null)
            {
                PersianCalendar objPersianCalendar = new PersianCalendar();
                int year = objPersianCalendar.GetYear(dt);
                int month = objPersianCalendar.GetMonth(dt);
                int day = objPersianCalendar.GetDayOfMonth(dt);
                result = year.ToString().PadLeft(4, '0') + "/" +
                         month.ToString().PadLeft(2, '0') + "/" +
                         day.ToString().PadLeft(2, '0');
            }
            return new SqlString(result);
        }
        catch (Exception e)
        {
            return SqlString.Null;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean IsLeapYear(int sal)
    {
        try
        {
            PersianCalendar jc = new PersianCalendar();

            return jc.IsLeapYear(sal);
        }
        catch (Exception e)
        {
            return SqlBoolean.Null;
        }

    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString DATEADDPersian(string datepart, int number, string date)
    {
        try
        {
            PersianCalendar objPersianCalendar = new PersianCalendar();

            DateTime PersianDate = new DateTime();

            string result = "";

            string[] date_spilt = date.Split('/');

            if (datepart == Date_part.YEAR.ToString())
            {
                PersianDate = objPersianCalendar.AddYears(objPersianCalendar.ToDateTime(int.Parse(date_spilt[0]), int.Parse(date_spilt[1]), int.Parse(date_spilt[2]), 0, 0, 0, 0), number);
            }
            else if (datepart == Date_part.DAY.ToString())
            {
                PersianDate = objPersianCalendar.AddDays(objPersianCalendar.ToDateTime(int.Parse(date_spilt[0]), int.Parse(date_spilt[1]), int.Parse(date_spilt[2]), 0, 0, 0, 0), number);
            }


            result = string.Format("{0}/{1}/{2}", objPersianCalendar.GetYear(PersianDate).ToString().PadLeft(4, '0'), objPersianCalendar.GetMonth(PersianDate).ToString().PadLeft(2, '0'), objPersianCalendar.GetDayOfMonth(PersianDate).ToString().PadLeft(2, '0'));

            return new SqlString(result);
        }
        catch (Exception e)
        {
            return SqlString.Null;
        }

    }


    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt32 DATEDIFFPersian(string datepart, string date_from, string date_to)
    {
        try
        {
            PersianCalendar objPersianCalendar = new PersianCalendar();

            DateTime PersianDate_from = new DateTime();
            DateTime PersianDate_to = new DateTime();

            string[] date_from_spilt = date_from.Split('/');
            string[] date_to_spilt = date_to.Split('/');

            if (datepart == Date_part.DAY.ToString())
            {
                PersianDate_from = objPersianCalendar.ToDateTime(int.Parse(date_from_spilt[0]), int.Parse(date_from_spilt[1]), int.Parse(date_from_spilt[2]), 0, 0, 0, 0);
                PersianDate_to = objPersianCalendar.ToDateTime(int.Parse(date_to_spilt[0]), int.Parse(date_to_spilt[1]), int.Parse(date_to_spilt[2]), 0, 0, 0, 0);
            }

            //int days = DateDiff(datepart, PersianDate_from, PersianDate_to)

            return (SqlInt32)(PersianDate_to - PersianDate_from).TotalDays;
        }
        catch (Exception e)
        {
            return SqlInt32.Null;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean ISDATEPersian(string date)
    {
        try
        {
            if (Regex.IsMatch(date, @"^(\d{4}\/(0[1-9]|1[012])\/(0[1-9]|[12][0-9]|3[01]))"))
            {
                PersianCalendar objPersianCalendar = new PersianCalendar();

                DateTime Date = new DateTime();

                string[] date_spilt = date.Split('/');

                Date = objPersianCalendar.ToDateTime(int.Parse(date_spilt[0]), int.Parse(date_spilt[1]), int.Parse(date_spilt[2]), 0, 0, 0, 0);

                DateTime temp;

                if (DateTime.TryParse(Date.ToShortDateString(), out temp))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }

}
