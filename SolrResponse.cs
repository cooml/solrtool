using System;
using System.Collections.Generic;
using System.Text;

namespace solrtool
{
    [Serializable]
    public class SolrResponse
    {
        public Response response { get; set; }
    }

    [Serializable]
    public class Response
    {
        public int numFound { get; set; }
        public int start { get; set; }

        public List<Doc> docs { get; set; }


    }

    [Serializable]
    public class Doc
    {
        public string query_s { get; set; }
        public string id { get; set; }
        public string query_t { get; set; }
        public string aggr_type_s { get; set; }
        public string filters_s { get; set; }
        public List<string> filters_ss { get; set; }
        public double weight_d { get; set; }
        public int aggr_count_i { get; set; }
        public string aggr_id_s { get; set; }
        public string aggr_job_id_s { get; set; }
        public string flag_s { get; set; }
        public DateTime timestamp_tdt { get; set; }

    }
}
