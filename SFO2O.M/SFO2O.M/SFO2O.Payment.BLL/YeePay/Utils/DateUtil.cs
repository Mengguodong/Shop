using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 有关日期使用的小工具
/// </summary>
public static class DateUtil
{

    /// <summary>
    /// 获取当前戳
    /// </summary>
    /// <returns></returns>
    public static int GetTimeStamp()
    {
        int transtime = 1;
        DateTime t1 = DateTime.Now;
        DateTime t2 = new DateTime(1970, 1, 1);
        double t = t1.Subtract(t2).TotalSeconds;
        transtime = (int)t;
        return transtime;
    }

    /// <summary>
    /// 获取到指定时间的时间戳
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="date">日</param>
    /// <returns></returns>
    public static int GetTimeStamp(int year,int month,int date)
    {

        int transtime = 1;
        DateTime t1 = DateTime.Now;
        DateTime t2;
        try
        {
             t2 = new DateTime(year, month, date);
        }
        catch (Exception e)
        {
            Exception time = new Exception("无法将year:month:date="+year+":"+month+":"+date+ "转化成DateTime");
            throw time;
        }
      
        double t = t1.Subtract(t2).TotalSeconds;
        transtime = (int)t;
        return transtime;
    }
    /// <summary>
    /// 获取IP
    /// </summary>
    /// <returns></returns>
    public static string getIp()
    {
        if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
        else
            return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
    }



}
