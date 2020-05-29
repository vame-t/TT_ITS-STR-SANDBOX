using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using MySql.Data.MySqlClient;
using Sandbox.ViewModels; 

namespace Sandbox.Models
{
    public class Schueler
    {
        //Fields:
        //static String connectionString = "SERVER=127.0.0.1;DATABASE=studentmanagement-db;UID=root;PASSWORD=;";
        //MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
        //Properties: 
        public String Anrede { get; set; }
        public String Vorname
        {
            get;
            set;

        }
        public String Nachname {get; set;}
        public String GeborenAm {get; set;}
        public String GeburtsOrt {get; set;}
        public String Anschrift1 { get; set;}
        public int Anschrift2 { get; set;}
        public String TelNr {get; set;}
        public String EMail {get; set;}
        public DataView ItemSource { get; set; }
        public String SchuelerID { get; set; }
        public int Schueler_ID { get; set; }

        //Konstruktor: 
        public Schueler()
        {

        }
        
        //Methoden: 
        public override string ToString()
        {
            return Vorname;
        }

       
      

        












    }
}
