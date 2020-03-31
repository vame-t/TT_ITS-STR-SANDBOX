using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Models
{
   public class Betrieb
    {
        //Eigenschaften: 
        public String BetriebsName { get; set; }
        public String NewBetrieb { get; set; }
        public String BetriebsID { get; set; }
        public int BetriebsIDFK { get; set; }
        public DataView ItemSource { get; set; }
        private String Standort { get; set;  }

    }
}
