using System;
using System.ComponentModel;
using Sandbox.Models;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sandbox.Views;
using System.Windows;
using System.Collections.ObjectModel;

namespace Sandbox.ViewModels
{
   public class MainViewModel_Formular : INotifyPropertyChanged
    {
        //MySqlConnection aufbauen
        static String connectionString = "SERVER=127.0.0.1;DATABASE=zeugnisdb;UID=root;PASSWORD=;";
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString); 
            
        //Implementierung des Interfaces: 
        public event PropertyChangedEventHandler PropertyChanged;

        //Fields:
        public Window2 notenFormular = new Window2(); 
        private Schueler schueler = new Schueler();
        private Klasse klasse = new Klasse(); 
        private ICommand saveCommand;
        private ICommand nextCommand;
        
        //Properties:
        public Schueler SchuelerProp
        {
            get { return schueler; }
            set { schueler = value;
                //Welche Eigenschaft soll aktualisiert werden:      
                PropertyChanged(this, new PropertyChangedEventArgs("SchuelerProp"));
            }

        }
        public Klasse KlasseProp
        {
            get { return klasse; }
            set
            {
                klasse = value;
                //Welche Eigenschaft soll aktualisiert werden:      
                PropertyChanged(this, new PropertyChangedEventArgs("KlasseProp"));
            }

        }
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand(
                        () => clickNext());
                }
                return nextCommand;
            }
        }
        public ICommand SaveCommand
        {
            get {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(() => SaveStudentToDB());
                }
                return saveCommand; }
        }

        //Konstruktor:
        public MainViewModel_Formular()
        {
            //SchuelerAnrede = new ObservableCollection<Schueler>()
            //{
            //    new Schueler() { Anrede = "Herr" },
            //    new Schueler() { Anrede = "Frau" }
            //};
            
        }

        //Methoden:

        //Diese Methode ist noch unvollständig und muss noch bearbeitet werden !!
        public void clickNext()
        {
            notenFormular.Show(); 
        }

        //Diese Methode ist noch unvollständig und muss noch bearbeitet werden !!
        public void SaveStudentToDB()
        {
            mySqlConnection.Open();
            string query = "INSERT INTO `zeugnisdb`.`tbl_schüler` (`Anrede`,`Nachname`, `Vorname`, `Geburtsdatum`,`Geburtsort`,`Telefonnummer`,EMail) VALUES(@Anrede,@Nachname,@Vorname,@Geburtsdatum,@Geburtsort,@Telefonnummer,@EMail);";
            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            command.Parameters.AddWithValue("@Anrede", SchuelerProp.Anrede); 
            command.Parameters.AddWithValue("@Nachname",SchuelerProp.Nachname);
            command.Parameters.AddWithValue("@Vorname",SchuelerProp.Vorname);
            command.Parameters.AddWithValue("@Geburtsdatum",SchuelerProp.GeborenAm);
            command.Parameters.AddWithValue("@Geburtsort",SchuelerProp.GeburtsOrt);
            command.Parameters.AddWithValue("@Telefonnummer",SchuelerProp.TelNr);
            command.Parameters.AddWithValue("@EMail",SchuelerProp.EMail);
            //Query2 unnötig, Klassen sollen schon im vorraus selektierbar sein´, es sollen keine neuen Klassen erstellt werden, wird eine neue Klasse erstellt dann ein neuer Eintrag in die Datenbank. 
            string query2 = "INSERT INTO `zeugnisdb`.`tbl_klasse` (`NameKlasse`) VALUES (@NameKlasse);";
            MySqlCommand command2 = new MySqlCommand(query2, mySqlConnection);
            //command2.Parameters.AddWithValue("@NameKlasse", KlasseProp.KlassenName);
            command.ExecuteNonQuery();
            command2.ExecuteNonQuery(); 
            MessageBox.Show("Alle Daten wurden vollständig hinzugefügt", "Info",MessageBoxButton.OK, MessageBoxImage.Information);
            mySqlConnection.Close(); 

        }

    }

    //Versuche: Combobox Auswahl: 
    //private ObservableCollection<Schueler> schuelerAnrede;
    //private Schueler selectedAnrede; 
    //public ObservableCollection<Schueler> SchuelerAnrede
    //{
    //    get { return schuelerAnrede; }
    //    set { schuelerAnrede = value; }
    //}
    //public Schueler SelectedAnrede
    //{
    //    get { return selectedAnrede;}
    //    set { selectedAnrede = value; }
    //}

        //protected void NotifyOfPropertyChange(string name)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(name));
        //    }
        //}
}
