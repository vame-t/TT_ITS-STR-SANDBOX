using System;
using System.ComponentModel;
using Sandbox.Models;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sandbox.Views;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data;

namespace Sandbox.ViewModels
{
   public class MainViewModel_Formular : INotifyPropertyChanged
    {
        //MySqlConnection aufbauen
        static String connectionString = "SERVER=127.0.0.1;DATABASE=studentmanagement-db;UID=root;PASSWORD=;";
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString); 
            
        //Implementierung des Interfaces: 
        public event PropertyChangedEventHandler PropertyChanged;

        //Fields:
        private bool anredeHerr;
        private bool anredeFrau; 
        public Window2 notenFormular = new Window2(); 
        private Schueler schueler = new Schueler();
        private Klasse klasse = new Klasse();
        private Schuljahr schulJahr = new Schuljahr();
        private Betrieb betrieb = new Betrieb(); 
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
        public Schuljahr SchuljahrProp
        {
            get { return schulJahr; }
            set { schulJahr = value;}
        }
        public Betrieb BetriebProp
        {
            get { return betrieb; }
            set {
                betrieb = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("BetriebProp")); 
            } 
        }
        public bool AnredeHerr
        {
            get { return anredeHerr; }
            set { anredeHerr = value; }

        }
        public bool AnredeFrau
        {
            get { return anredeFrau; }
            set { anredeFrau = value; }
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
            putClassesIntoComboBox();
            putSchoolYearIntoComboBox();
            putCompanyIntoComboBox(); 
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
            decideAnrede();
            mySqlConnection.Open();
            string query = "INSERT INTO `studentmanagement-db`.`tbl_student` (`Anrede`,`Nachname`, `Vorname`, `Geburtsdatum`,`Geburtsort`,`Anschrift_1`,`Anschrift_2`,`Telefonnummer`,`EMail`,`FK_Klasse`,`FK_Schuljahr`) " +
                           "VALUES(@Anrede,@Nachname,@Vorname,@Geburtsdatum,@Geburtsort,@Anschrift_1,@Anschrift_2,@Telefonnummer,@EMail,@FK_Klasse,@FK_Schuljahr);";
            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            command.Parameters.AddWithValue("@Anrede", SchuelerProp.Anrede); 
            command.Parameters.AddWithValue("@Nachname",SchuelerProp.Nachname);
            command.Parameters.AddWithValue("@Vorname",SchuelerProp.Vorname);
            command.Parameters.AddWithValue("@Geburtsdatum",SchuelerProp.GeborenAm);
            command.Parameters.AddWithValue("@Geburtsort",SchuelerProp.GeburtsOrt);
            command.Parameters.AddWithValue("@Anschrift_1", SchuelerProp.Anschrift1);
            command.Parameters.AddWithValue("@Anschrift_2", SchuelerProp.Anschrift2);
            command.Parameters.AddWithValue("@Telefonnummer",SchuelerProp.TelNr);
            command.Parameters.AddWithValue("@EMail",SchuelerProp.EMail);
            command.Parameters.AddWithValue("@FK_Klasse", KlasseProp.KlassenIDFK);
            command.Parameters.AddWithValue("@FK_Schuljahr", SchuljahrProp.SchulJahrIDFK);
            //command.Parameters.AddWithValue("@FK_Betrieb", BetriebProp.BetriebsIDFK);
            command.ExecuteNonQuery();
            //if (BetriebProp.BetriebsIDFK == 0) //Hier muss eine andere Bedingung rein, denn diese Property wird mit dem click verändert auch wenn sie anfangs 0 sein sollte. 2 Felder bedeuten 2 Properties also eventuell noch eine Property erstellen
            //{
            //    string query2 = "INSERT INTO `studentmanagement-db`.`tbl_Betrieb` (`BetriebsName`) VALUES(@BetriebsName)"; 
            //    MySqlCommand command2 = new MySqlCommand(query2, mySqlConnection);
            //    command2.Parameters.AddWithValue("@BetriebsName",BetriebProp.NewBetrieb);
            //    command2.ExecuteNonQuery();

            //    string query3 = "INSERT INTO `studentmanagement-db`.`tbl_student` (`FK_Betrieb`) VALUES(@FK_Betrieb)";
            //    MySqlCommand command3 = new MySqlCommand(query3, mySqlConnection);
            //    command3.Parameters.AddWithValue("@FK_Betrieb", BetriebProp.BetriebsIDFK);
            //    command3.ExecuteNonQuery(); 
            //}

            MessageBox.Show("Alle Daten wurden vollständig hinzugefügt", "Info",MessageBoxButton.OK, MessageBoxImage.Information);
            mySqlConnection.Close(); 
        }

        public void decideAnrede()
        {
            if (AnredeHerr == true)
            {
                SchuelerProp.Anrede = "Herr";
            }
            else if (AnredeFrau == true)
            {
                SchuelerProp.Anrede = "Frau";
            }
            else
            {
                //MessageBox.Show("Bitte wählen Sie unter 'Anrede' eines der Optionen aus! ", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); 
            }

        }

        public void putClassesIntoComboBox()
        {
            try
            {
                
                string query = "SELECT * FROM `studentmanagement-db`.tbl_class;";
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable classTable = new DataTable();
                    sqlDataAdapter.Fill(classTable);
                    KlasseProp.KlassenNamen = "ClassName";
                    KlasseProp.ItemSource = classTable.DefaultView;
                    KlasseProp.KlassenID = "tbl_class_id";
                   
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        public void putSchoolYearIntoComboBox()
        {
            try
            {
                string query = "SELECT * FROM `studentmanagement-db`.tbl_schuljahr;";
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable schoolYearTable = new DataTable();
                    sqlDataAdapter.Fill(schoolYearTable);
                    SchuljahrProp.SchulJahrProperty = "Schuljahr";
                    SchuljahrProp.ItemSource = schoolYearTable.DefaultView;
                    SchuljahrProp.SchulJahrID = "tbl_Schuljahr_id"; 


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        public void putCompanyIntoComboBox()
        {
            try
            {
                string query = "SELECT * FROM `studentmanagement-db`.tbl_betrieb;";
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable companyTable = new DataTable();
                    sqlDataAdapter.Fill(companyTable);
                    BetriebProp.BetriebsName = "BetriebsName";
                    BetriebProp.ItemSource = companyTable.DefaultView;
                    BetriebProp.BetriebsID = "tbl_Betrieb_id";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }


    }

   
}
