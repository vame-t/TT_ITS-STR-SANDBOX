using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Sandbox.Models
{
    public class Schueler
    {
        //Fields: 
        //Properties: 
        public String Anrede { get; set; }
        public String Vorname {get; set;}
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
        public DataTable SchuelerTable { get; set; }

        //Konstruktor: 
        public Schueler()
        {

        }
        //Methoden: 












    }
}
