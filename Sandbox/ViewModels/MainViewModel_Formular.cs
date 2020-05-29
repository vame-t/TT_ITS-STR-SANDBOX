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
        //MySqlConnection aufbauen:
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
        private ICommand saveCommand;
        private ICommand nextCommand;
        private ICommand backCommand;

        //Properties:
        public Action CloseAction { get; set; }
        public Schueler SchuelerProp
        {
            get { return schueler; }
            set
            {
                schueler = value;
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
            set { schulJahr = value; }
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
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(() => SaveStudentToDB());
                }
                return saveCommand;
            }
        }
        public ICommand BackCommand
        {
            get
            {
                if (backCommand == null)
                {
                    backCommand = new RelayCommand(() => backToMainWindow());
                }
                return backCommand;
            }
        }

        //Konstruktor:
        public MainViewModel_Formular()
        {
            putClassesIntoComboBox();
            putSchoolYearIntoComboBox();
        }

        //Methoden:

        //TODO: Diese Methode ist noch unvollständig und muss noch bearbeitet werden !!
        /// <summary>
        /// Diese Methode öffnet die View: Window2.xaml was zur Noteneintragung und zum generieren des Zeugnisses dient. 
        /// Da diese Methode unvollständig ist wird die aktuelle View:Formular.xaml nicht geschlossen, dieses muss manuell durchgeführt werden. 
        /// </summary>
        public void clickNext()
        {
            notenFormular.Show();
        }

        //TODO: Diese Methode ist noch unvollständig und muss noch bearbeitet werden !!
        /// <summary>
        /// Diese Methode speichert alle Daten aus der Oberfläche in die Datenbank zu den jeweiligen Spalten der Tabelle tbl_student ab. 
        /// Wichtig ist, dass diese Methode nur dann funktioniert, wenn alle Felder auf der Oberfläche (Formular.xaml) befüllt sind.
        /// Auch gibt es momentan keine Validierung zu den Werten, man kann also egal was für einen Wert man will in die Textboxen reinschreiben. 
        /// Auch ist es möglich einen Schüler mehrmals in die Datenbank abzuspeichern, hier wurde kein Datenbankabgleich implementiert. 
        /// Daher ist es wichtig Echtdaten einzugeben, damit man die Anwendung anwendbar ist. 
        /// Fehler können entstehen, wenn Felder leer gelassen werden. 
        /// Ebenso können Folgefehler entstehen, wenn man einen Namen mehrmals abspeichert. 
        /// 
        /// </summary>
        public void SaveStudentToDB()
        {
            try
            {
                decideAnrede();
                mySqlConnection.Open();
                string query = "INSERT INTO `studentmanagement-db`.`tbl_student` (`Anrede`,`Nachname`, `Vorname`, `Geburtsdatum`,`Geburtsort`,`Anschrift_1`,`Anschrift_2`,`Telefonnummer`,`EMail`,`FK_Klasse`,`FK_Schuljahr`) " +
                               "VALUES(@Anrede,@Nachname,@Vorname,@Geburtsdatum,@Geburtsort,@Anschrift_1,@Anschrift_2,@Telefonnummer,@EMail,@FK_Klasse,@FK_Schuljahr);";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                command.Parameters.AddWithValue("@Anrede", SchuelerProp.Anrede);
                command.Parameters.AddWithValue("@Nachname", SchuelerProp.Nachname);
                command.Parameters.AddWithValue("@Vorname", SchuelerProp.Vorname);
                command.Parameters.AddWithValue("@Geburtsdatum", SchuelerProp.GeborenAm);
                command.Parameters.AddWithValue("@Geburtsort", SchuelerProp.GeburtsOrt);
                command.Parameters.AddWithValue("@Anschrift_1", SchuelerProp.Anschrift1);
                command.Parameters.AddWithValue("@Anschrift_2", SchuelerProp.Anschrift2);
                command.Parameters.AddWithValue("@Telefonnummer", SchuelerProp.TelNr);
                command.Parameters.AddWithValue("@EMail", SchuelerProp.EMail);
                command.Parameters.AddWithValue("@FK_Klasse", KlasseProp.KlassenIDFK);
                command.Parameters.AddWithValue("@FK_Schuljahr", SchuljahrProp.SchulJahrIDFK);
                command.ExecuteNonQuery();
                MessageBox.Show("Alle Daten wurden vollständig hinzugefügt", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                mySqlConnection.Close();
            }


        }

        /// <summary>
        /// Mit dieser Methode wird die Anrede Property durch Auswahl eines der beiden Radiobuttons(siehe View: Formular.xaml) befüllt.
        /// </summary>
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

        /// <summary>
        /// Diese Methode fügt alle verfügbaren Schulklassen aus der Datenbank in eine Combobox. 
        /// </summary>
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

        /// <summary>
        /// Diese Methode fügt alle verfügbaren Schuljahre aus der Datenbank in eine Combobox. 
        /// </summary>
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
        /// <summary>
        /// Mit dieser Methode ist es möglich, wieder ins Hauptfenster(MainWindow: FirstWindow.xaml) zu gelangen. 
        /// </summary>
        public void backToMainWindow()
        {
            FirstWindow mainWindow = new FirstWindow();
            mainWindow.Show();
            CloseAction();
            //System.Windows.Application.Current.Shutdown();

        }


    }






}
