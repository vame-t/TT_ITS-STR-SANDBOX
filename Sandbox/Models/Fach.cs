using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Models
{
   public class Fach
    {
        //Eigenschaften: 
        private String FachName { get; set; }
        private String FachKuerzel { get; set; }

        //Hat-Beziehungen -- Composition
        // Eventuell Klasse Note erstellen, da jedes Fach eine/mehrere Note/n HAT. 
        // Gedanke: Mehrere Fächer Klassen erstellen wie z.b: SAE,ITS,BWL,WI,GK,D,E alle würden von dieser Klasse erben. 
        // Eventuell Fragen ob dies eine sinnvolle Idee wäre. 
        //




        //Konstruktor: 
        public Fach()
        {

        }




    }
}
