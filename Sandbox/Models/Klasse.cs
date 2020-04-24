using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Sandbox.Models
{
    public class Klasse
    {
        //Eigenschaften: 
        public String KlassenNamen { get; set;}
        public String KlassenID { get; set; }
        public DataView ItemSource { get; set; } 
        public int KlassenIDFK { get;set;}

        //Konstruktor: 
        public Klasse()
        {

        }

        //Methoden: 

        



    }
}
