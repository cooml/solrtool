using System;
using System.Collections.Generic;
using System.Text;

namespace solrtool
{
  [Serializable]
  public class SolrResponseSignal
  {
    public ResponseSignal response { get; set; }
  }

  [Serializable]
  public class ResponseSignal
  {
    public int numFound { get; set; }
    public int start { get; set; }

    public List<DocSignal> docs { get; set; }


  }

  [Serializable]
  public class DocSignal
  {
    public string type { get; set; }
    public string id { get; set; }
    public string params_language_s { get; set; }
    public string query { get; set; }
    public string params_realm_s { get; set; }
    public string doc_id { get; set; }
    public string date { get; set; }
    public int count_i { get; set; }

  }


  [Serializable]
  public class DocSignalOpensearch
  {
    public string type { get; set; }   
    public string language { get; set; }
    public string keyword { get; set; }
    public string realm { get; set; }
    public string indexId { get; set; }
    public string time { get; set; }
    public int count { get; set; }

  }
}
