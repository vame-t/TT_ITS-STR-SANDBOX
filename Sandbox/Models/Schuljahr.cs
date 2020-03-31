using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Models
{
  public class Schuljahr
    {
        //Properties: 
        public String SchulJahrProperty
        {
            get;
            set; 
        }
        public DataView ItemSource { get; set; }
        public String SchulJahrID
        {
            get;
            set;
        }
        public int SchulJahrIDFK
        {
            get;
            set;
        }

        //Konstruktor: 
        public Schuljahr()
        {

        }
    }
}
