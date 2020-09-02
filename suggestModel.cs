using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solrtool
{
  [Serializable]
  public class suggestModel
  {
    public string appId { get; set; }

    public string scenario { get; set; }
    public string language { get; set; }
    public string realm { get; set; }
    public string type { get; set; }
    public string word { get; set; }


  }
}
