using eService.Microservices.Types.Search;
using Grpc.Core;
using Newtonsoft.Json;
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
        var queryUrl=baseUrl + "/select?q=" + q + fqString + sortString + rowsString+ startString;
        LogHelper.LogInfo(queryUrl);
        return getFromPrd(queryUrl);
        oRequest = (HttpWebRequest)WebRequest.Create(queryUrl);
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

    public static string getFromPrd(string url)
    {

      httpRequestClient h = new httpRequestClient();
      h.baseUrl = url;
      h.getEncoding = "utf-8";
      h.contentType = "application/json;charset=utf-8";
      h.method = "GET";

      Channel channel = new Channel("", ChannelCredentials.Insecure);
      channel.ConnectAsync(DateTime.UtcNow.AddSeconds(2)).GetAwaiter().GetResult();
      var client = new SearchGrpc.SearchGrpcClient(channel);
      var req = new ContentMesReq();
      req.ReqMes = "";
      req.ValueMes = JsonConvert.SerializeObject(h, Newtonsoft.Json.Formatting.Indented);
      string ret = client.SearchExtension(req).ValueMes;
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

  public class openSearchIndexModel
  {
    public string jsonData { get; set; }
    public string baseUrl { get; set; }
    public string appId { get; set; }
    public string token { get; set; }
    public string indexType { get; set; }
    public Dictionary<string, string> openSearchHeaders { get; set; }


  }
  public class httpRequestClient
  {
    public string baseUrl { get; set; }
    public string method { get; set; } = "POST";
    public byte[] b { get; set; } = new byte[0];
    public Dictionary<string, string> headers { get; set; }
    public string contentType { get; set; } = "application/json";
    public string getEncoding { get; set; } = "utf-8";

  }


}
