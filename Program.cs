using Newtonsoft.Json;
using System;
using System.Configuration;

namespace solrtool
{
  class Program
  {
    static void Main(string[] args)
    {
      int start = 0;
      int rows = Convert.ToInt32(ConfigurationManager.AppSettings.Get("rows"));
      int numFound = 0;
      var fromUrl = ConfigurationManager.AppSettings.Get("fromUrl");
      var toUrl = ConfigurationManager.AppSettings.Get("toUrl");
      var sort = ConfigurationManager.AppSettings.Get("sort");
      var fq = ConfigurationManager.AppSettings.Get("fq");


      LogHelper.LogInfo("start");
      LogHelper.LogInfo("from url:" + fromUrl);
      LogHelper.LogInfo("to url:+" + toUrl);
      var jsonData = SolrHelper.getDataFromSolr(fromUrl, start, rows, fq, sort);
      var resopnseModel = JsonConvert.DeserializeObject<SolrResponse>(jsonData);
      if (resopnseModel != null && resopnseModel.response != null && resopnseModel.response.numFound > 0)
      {
        numFound = resopnseModel.response.numFound;
        LogHelper.LogInfo("numFound:" + numFound);


        //if (resopnseModel.response.docs != null && resopnseModel.response.docs.Count > 0)
        //{
        //  LogHelper.LogInfo("Import--------:1");
        //  var sDatas = JsonConvert.SerializeObject(resopnseModel.response.docs);
        //  var sResopnse = SolrHelper.SolrPost(sDatas, toUrl);
        //}

        for (int i = 0; i < (numFound / rows) + 1; i++)
        {
          LogHelper.LogInfo("start--------page:" + (i + 1));

          var jsonDataTemp = SolrHelper.getDataFromSolr(fromUrl, start, rows, fq, sort);
          var resopnseModelTemp = JsonConvert.DeserializeObject<SolrResponse>(jsonDataTemp);
          if (resopnseModelTemp != null && resopnseModelTemp.response != null && resopnseModelTemp.response.docs != null)
          {
            if (resopnseModelTemp.response.docs.Count > 0)
            {
              //
              LogHelper.LogInfo("Import--------total:" + ((i + 1) * rows > resopnseModel.response.numFound ? resopnseModel.response.numFound : (i + 1) * rows) + "/" + resopnseModel.response.numFound);
              var sDatas = JsonConvert.SerializeObject(resopnseModelTemp.response.docs);
              var sResopnse = SolrHelper.SolrPost(sDatas, toUrl);
              SolrHelper.FusionSolrCommit(toUrl);
            }
            if (resopnseModelTemp.response.docs.Count != rows)
            {
              break;//终止
            }
          }
          else
          {
            break;
          }



        }
      }


      LogHelper.LogInfo("complete...");

      Console.WriteLine("press any key/enter to exit");
      Console.Read();
    }
  }
}
