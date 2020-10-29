using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
      LogHelper.LogInfo("to local file:result_json.txt");
      LogHelper.LogInfo("to local file:result_binary");
      LogHelper.LogInfo("to local file:result_binary_source");
      var jsonData = SolrHelper.getDataFromSolr(fromUrl, start, rows, fq, sort);
      var resopnseModel = JsonConvert.DeserializeObject<SolrResponseSignal>(jsonData);
      List<DocSignal> docFiles = new List<DocSignal>();
      List<string> solrResponseStrBinary = new List<string>();

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

          var jsonDataTemp = SolrHelper.getDataFromSolr(fromUrl, rows * i, rows, fq, sort);
          solrResponseStrBinary.Add(jsonDataTemp);
          var resopnseModelTemp = JsonConvert.DeserializeObject<SolrResponseSignal>(jsonDataTemp);
          if (resopnseModelTemp != null && resopnseModelTemp.response != null && resopnseModelTemp.response.docs != null)
          {
            if (resopnseModelTemp.response.docs.Count > 0)
            {
              //
              LogHelper.LogInfo("Import--------total:" + ((i + 1) * rows > resopnseModel.response.numFound ? resopnseModel.response.numFound : (i + 1) * rows) + "/" + resopnseModel.response.numFound);

              //var sDatas = JsonConvert.SerializeObject(resopnseModelTemp.response.docs);
              docFiles.AddRange(resopnseModelTemp.response.docs);



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


        System.IO.File.WriteAllText(("result_json_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt"), JsonConvert.SerializeObject(docFiles, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


        List<DocSignalOpensearch> opensearch = docFiles.Select(a => new DocSignalOpensearch()
        {

          count = a.count_i,
          time = a.date,
          indexId = a.doc_id.ToUpper(),
          language = a.params_language_s,
          keyword = a.query,
          type = a.type,
          realm = a.params_realm_s

        }).ToList();


        System.IO.File.WriteAllText(("result_json_open" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt"), JsonConvert.SerializeObject(opensearch, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

        BinarySerialize.Serialize<List<string>>(solrResponseStrBinary, "result_binary_source" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        BinarySerialize.Serialize<List<DocSignal>>(docFiles, "result_binary" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        BinarySerialize.Serialize<List<DocSignalOpensearch>>(opensearch, "result_binary_open" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

        LogHelper.LogInfo("complete...");

        Console.WriteLine("press any key/enter to exit");
        Console.Read();
      }





    }



















    static void aintest(string[] args)
    {
      #region remove
      ////var solrStrBinary = BinarySerialize.DeSerialize<List<string>>("solrResponseStrBinary_2020-08-15-15-38-45");
      //var solrBinary = BinarySerialize.DeSerialize<List<SolrResponse>>("solrResponseBinary_2020-08-15-15-38-15");
      //var openSearchSuggest = BinarySerialize.DeSerialize<List<suggestModel>>("opensearchSuggest_2020-09-02-16-10-15");
      //List<suggestModel> suggestList = new List<suggestModel>();
      //foreach (var item in solrBinary)
      //{
      //  if (item != null && item.response != null && item.response.docs != null)
      //  {
      //    foreach (var itemResponse in item.response.docs)
      //    {
      //      if (itemResponse.query_s != null && itemResponse.query_s.Length < 60 && !itemResponse.query_s.Contains("�"))
      //      {
      //        var r = "";
      //        var l = "";
      //        if (itemResponse.filters_ss != null)
      //        {
      //          foreach (var itemFilter in itemResponse.filters_ss)
      //          {
      //            if (itemFilter.ToLower().Contains("realm:"))
      //            {
      //              r = itemFilter.ToLower().Split(':')[1].Trim();
      //            }
      //            if (itemFilter.ToLower().Contains("language:"))
      //            {
      //              l = itemFilter.ToLower().Split(':')[1].Trim();
      //            }

      //          }
      //        }
      //        suggestList.Add(new suggestModel
      //        {
      //          appId = "164",
      //          scenario = "2ac2d0fe31384dc8b2ec5aa96ab1a967",
      //          language = l.ToLower(),
      //          realm = r.ToLower(),
      //          type = "keyword",
      //          word = itemResponse.query_s.ToLower()

      //        });

      //      }

      //    }
      //  }

      //}

      //System.IO.File.WriteAllText(@"json.txt", JsonConvert.SerializeObject(suggestList, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

      //// BinarySerialize.Serialize<List<suggestModel>>(suggestList, "opensearchSuggest_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));


      //int start = 0;
      //int rows = Convert.ToInt32(ConfigurationManager.AppSettings.Get("rows"));
      //int numFound = 0;
      //var fromUrl = ConfigurationManager.AppSettings.Get("fromUrl");
      //var toUrl = ConfigurationManager.AppSettings.Get("toUrl");
      //var sort = ConfigurationManager.AppSettings.Get("sort");
      //var fq = ConfigurationManager.AppSettings.Get("fq");
      //LogHelper.LogInfo("start");
      //LogHelper.LogInfo("from url:" + fromUrl);
      //LogHelper.LogInfo("to url:+" + toUrl);
      //var jsonData = SolrHelper.getDataFromSolr(fromUrl, start, rows, fq, sort);
      //var resopnseModel = JsonConvert.DeserializeObject<SolrResponse>(jsonData);
      //if (resopnseModel != null && resopnseModel.response != null && resopnseModel.response.numFound > 0)
      //{
      //  numFound = resopnseModel.response.numFound;
      //  LogHelper.LogInfo("numFound:" + numFound);


      //  //if (resopnseModel.response.docs != null && resopnseModel.response.docs.Count > 0)
      //  //{
      //  //  LogHelper.LogInfo("Import--------:1");
      //  //  var sDatas = JsonConvert.SerializeObject(resopnseModel.response.docs);
      //  //  var sResopnse = SolrHelper.SolrPost(sDatas, toUrl);
      //  //}
      //  List<SolrResponse> solrResponseBinary = new List<SolrResponse>();
      //  List<string> solrResponseStrBinary = new List<string>();
      //  for (int i = 0; i < (numFound / rows) + 1; i++)
      //  {
      //    LogHelper.LogInfo("start--------page:" + (i + 1));

      //    var jsonDataTemp = SolrHelper.getDataFromSolr(fromUrl, rows * i, rows, fq, sort);
      //    solrResponseStrBinary.Add(jsonDataTemp);
      //    var resopnseModelTemp = JsonConvert.DeserializeObject<SolrResponse>(jsonDataTemp);
      //    solrResponseBinary.Add(resopnseModelTemp);
      //    if (resopnseModelTemp != null && resopnseModelTemp.response != null && resopnseModelTemp.response.docs != null)
      //    {
      //      if (resopnseModelTemp.response.docs.Count > 0)
      //      {
      //        //
      //        LogHelper.LogInfo("Import--------total:" + ((i + 1) * rows > resopnseModel.response.numFound ? resopnseModel.response.numFound : (i + 1) * rows) + "/" + resopnseModel.response.numFound);

      //        var sDatas = JsonConvert.SerializeObject(resopnseModelTemp.response.docs);
      //        //LogHelper.LogInfo(sDatas);

      //        var sResopnse = SolrHelper.SolrPost(sDatas, toUrl);

      //        SolrHelper.FusionSolrCommit(toUrl);
      //      }
      //      if (resopnseModelTemp.response.docs.Count != rows)
      //      {
      //        break;//终止
      //      }
      //    }
      //    else
      //    {
      //      break;
      //    }



      //  }
      //  BinarySerialize.Serialize<List<string>>(solrResponseStrBinary, "solrResponseStrBinary_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
      //  BinarySerialize.Serialize<List<SolrResponse>>(solrResponseBinary, "solrResponseBinary_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));


      //}
      //SolrHelper.FusionSolrCommit(toUrl);


      //LogHelper.LogInfo("complete...");

      //Console.WriteLine("press any key/enter to exit");
      //Console.Read();

      #endregion
    }

  }
}
