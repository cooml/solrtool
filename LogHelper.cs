using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace solrtool
{
  public static class LogHelper
  {
    public static ILog log = null;

    static LogHelper()
    {

      InitLog4Net();

    }
    private static void InitLog4Net()
    {
      try
      {
        if (log == null)
        {
          ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
          XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
          log = LogManager.GetLogger(repository.Name, "NETCorelog4net");
        }

      }
      catch (Exception e)
      {
        Console.WriteLine("InitLog4Net.exo" + e.Message + e.StackTrace);
      }



    }

    public static void LogInfo(this string info)
    {

      try
      {
        log.Info(info);
      }
      catch (Exception e)
      {

        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + info);
      }
    }
    public static void LogError(this string info)
    {

      try
      {
        log.Error(info);
      }
      catch (Exception e)
      {

        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + info);
      }
    }
  }
}
