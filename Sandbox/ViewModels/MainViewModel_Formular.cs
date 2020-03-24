using System;
using System.ComponentModel;
using Sandbox.Models;

namespace Sandbox.ViewModels
{
   public class MainViewModel_Formular : INotifyPropertyChanged
    {
        //Implementierung des Interfaces: 
        public event PropertyChangedEventHandler PropertyChanged;

        //Fields: 
        private Schueler schueler;

        //Properties:
        public Schueler Schueler
        {
            get { return schueler; }
            set { schueler = value;
                //Welche Eigenschaft soll aktualisiert werden:      
                PropertyChanged(this, new PropertyChangedEventArgs("Schueler"));
            }

        }


        //Methoden: 


        /*
         * Bei dem Button "Speichern" sollen alle eingefügten Daten in die Datenbank eingespeichert werden 
         * CommandBinding soll hier angewendet werden 
         * 
         * 
         */



         



    }
}
