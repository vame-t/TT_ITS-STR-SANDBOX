using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Models
{
   public class Fach
    {
        //Eigenschaften: 
        public String FachName { get; set; }
        public String FachKuerzel { get; set; }
        public String FachID { get; set; }
        public int Fach_ID { get; set; }
        public DataView ItemSource { get; set; }


        



        //Konstruktor: 
        public Fach()
        {

        }




    }
}
