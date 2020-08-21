using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace solrtool
{
  public class SolrHelper
  {
    //http://10.62.109.126:8983/solr/Framework_20
    public static void FusionSolrCommit(string baseUrl)
    {
      try
      {
        var result = new WebClient().DownloadString(baseUrl + "/update?commit=true");
      }
      catch (Exception e)
      {
        LogHelper.LogError("FusionSolrCommit.exp:" + e.Message + e.StackTrace);

      }
    }
    public static string getDataFromSolr(string baseUrl, int start=0, int rows=10, string fq = "", string sort = "", string q = "*:*")
    { ///select?fq=aggr_type_s:(suggestion)&q=*:*&rows=2&sort=timestamp_tdt%20desc&start=1
      var fqString = "";
      if (!string.IsNullOrEmpty(fq))
      {
        fqString = "&fq=" + fq;
      }
      var sortString = "";
      if (!string.IsNullOrEmpty(sort))
      {
        sortString = "&sort=" + sort;
      }
      if (rows > 1000)
      {
        rows = 1000;
      }
      var rowsString = "&rows=" + rows;
      var startString = "&start=" + start;

      string ret = "";
      Stream oResponseStream = null;
      Stream oRequestStream = null;
      HttpWebRequest oRequest = null;
      try
      {

        oRequest = (HttpWebRequest)WebRequest.Create(baseUrl + "/select?q=" + q + fqString + sortString + rowsString+ startString);
        oRequest.Method = "GET";
        oRequest.ContentType = "application/json;charset=utf-8";
        HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();
        oResponseStream = oResponse.GetResponseStream();
        ret = new StreamReader(oResponseStream, Encoding.GetEncoding("utf-8")).ReadToEnd();
      }
      catch (Exception e)
      {
        LogHelper.LogError("SolrPost.exp:" + e.Message + e.StackTrace);
      }
      finally
      {
        try
        {
          if (oRequest != null)
          {
            oRequest.Abort();
            if (oRequest != null)
            {
              oRequest = null;
            }

          }
          if (oResponseStream != null)
          {
            oResponseStream.Close();
          }
          if (oRequestStream != null)
          {
            oRequestStream.Close();
            oRequestStream.Dispose();
          }
        }
        catch (Exception ex)
        { }

      }
      return ret;


    }
    public static string SolrPost(string jsonData, string baseUrl)
    {
      string ret = "";
      Stream oResponseStream = null;
      Stream oRequestStream = null;
      HttpWebRequest oRequest = null;
      try
      {
        byte[] b = Encoding.UTF8.GetBytes(jsonData);
        oRequest = (HttpWebRequest)WebRequest.Create(baseUrl + "/update");
        oRequest.Method = "POST";
        oRequest.ContentType = "application/json;charset=utf-8";
        oRequest.ContentLength = b.Length;
        oRequestStream = oRequest.GetRequestStream();
        oRequestStream.Write(b, 0, b.Length);
        oRequestStream.Close();
        HttpWebResponse oResponse = (HttpWebResponse)oRequest.GetResponse();
        oResponseStream = oResponse.GetResponseStream();
        ret = new StreamReader(oResponseStream, Encoding.GetEncoding("utf-8")).ReadToEnd();
      }
      catch (Exception e)
      {
        LogHelper.LogError("SolrPost.exp:" + e.Message + e.StackTrace);
      }
      finally
      {
        try
        {
          if (oRequest != null)
          {
            oRequest.Abort();
            if (oRequest != null)
            {
              oRequest = null;
            }

          }
          if (oResponseStream != null)
          {
            oResponseStream.Close();
          }
          if (oRequestStream != null)
          {
            oRequestStream.Close();
            oRequestStream.Dispose();
          }
        }
        catch (Exception ex)
        { }

      }
      return ret;
    }
  }
}
