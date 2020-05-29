using GalaSoft.MvvmLight.CommandWpf;
using MySql.Data.MySqlClient;
using Sandbox.Models;
using Sandbox.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sandbox.ViewModels
{
    class MainViewModel_DeleteStudent : INotifyPropertyChanged
    {
        //Implementierung des Interfaces: 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        //MySqlConnection aufbauen
        static String connectionString = "SERVER=127.0.0.1;DATABASE=studentmanagement-db;UID=root;PASSWORD=;";
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

        //Fields: 
        private String tempVorname = ""; 
        private ICommand deleteCommand;
        private ICommand searchCommand;
        private ICommand backCommand;
        private Klasse klasse = new Klasse();
        private Schueler schueler = new Schueler();
        private Note note = new Note(); 
        private ObservableCollection<Schueler> students = new ObservableCollection<Schueler>();
        private ObservableCollection<Note> noten = new ObservableCollection<Note>();
        //Properties: 
        public Action CloseAction { get; set; }
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
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new RelayCommand(() => searchStudents());
                }
                return searchCommand;
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(() => deleteStudent());
                }
                return deleteCommand;
            }
        }
        public Klasse KlasseProp
        {
            get { return klasse; }
            set
            {
                klasse = value;
                NotifyOfPropertyChange("KlasseProp");
            }
        }
        public Schueler SchuelerProp
        {
            get { return schueler; }
            set
            {
                schueler = value;
                NotifyOfPropertyChange("SchuelerProp");
            }
        }
        public ObservableCollection<Schueler> Students
        {
            get { return students; }
            set
            {
                students = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Students"));
            }
        }

        public ObservableCollection<Note> Noten
        {
            get { return noten;  }
            set
            {
                noten = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Noten"));
            }
        }

        //Konstruktor: 
        public MainViewModel_DeleteStudent()
        {
            putClassesIntoComboBox();

        }

        //Methods: 

        /// <summary>
        /// Mit dieser Methode ist es möglich, wieder ins Hauptfenster(MainWindow: FirstWindow.xaml) zu gelangen. 
        /// </summary>
        public void backToMainWindow()
        {
            //TODO
            FirstWindow mainWindow = new FirstWindow();
            mainWindow.Show();
            CloseAction(); 
            //System.Windows.Application.Current.Shutdown();

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
        /// Mit dieser Methode werden alle Schüler gefunden, die in einer bestimmten Schulklasse sind.
        /// Diese Methode wird nach der Auswahl der Schulklasse und nach dem klickt des "Suchen" Buttons ausgeführt. 
        /// Es zeigt alle Schüler mit Vornamen in einer Listbox an. (siehe View: DeleteStudent.xaml)
        /// </summary>
        public void searchStudents()
        {
            try
            {

                mySqlConnection.Open();
                string query = "SELECT `tbl_student`.`Vorname` FROM `studentmanagement-db`.tbl_student WHERE FK_Klasse= @FK_Klasse;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@FK_Klasse", KlasseProp.KlassenIDFK);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (students != null)
                    {
                        students.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        Schueler nschueler = new Schueler();
                        nschueler.Vorname = Convert.ToString(dataRow["Vorname"]);

                        students.Add(nschueler);
                    }
                   
                }

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
        /// Diese Methode selektiert alle Schüler aus der Datenbank
        /// </summary>
        public void selectStudents()
        {
            try
            {
                
                string query = "SELECT * FROM `studentmanagement-db`.tbl_student;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (students != null)
                    {
                        students.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        Schueler nschueler = new Schueler();
                        nschueler.Vorname = Convert.ToString(dataRow["Vorname"]);

                        students.Add(nschueler);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Diese Methode gibt die ID des ausgewählten Schülers an. 
        /// </summary>
        /// <returns>Gibt ein Integer zurück mit der ID des ausgewählten Schülers</returns>
        public int getStudentID()
        {
            try
            {
                int IDFromPickedStudent = 0;
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                string query = "SELECT tbl_student_id FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@Vorname", SchuelerProp.Vorname);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    tempVorname = SchuelerProp.Vorname; 
                    if (students != null)
                    {
                        students.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        schueler.Schueler_ID = (int)(dataRow["tbl_student_id"]);
                        IDFromPickedStudent = schueler.Schueler_ID;
                       
                    }
                }
                return IDFromPickedStudent;
            }
            catch (Exception e)
            {
                return -1;
            }
           

        }

        //public int getIDFromClass()
        //{
                //TODO 

        //}

        /// <summary>
        /// Diese Methode Befüllt die Noten-Property, wenn es Noteneinträge zu dem ausgewählen Schüler gibt.  
        /// </summary>
        public void FillNotePropertyIfPossilbe()
        {
          
            try
            {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                string query = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Student_id = @StudentID;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@StudentID", SchuelerProp.Schueler_ID);
                    command.ExecuteNonQuery();
                    DataTable notenTable = new DataTable();
                    sqlDataAdapter.Fill(notenTable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in notenTable.Rows)
                    {
                        note.Noten= (int)(dataRow["Note"]);
                    }
                }
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
        /// Diese Methode löscht den ausgewählten Schüler, dabei wird noch beachtet, ob der Schüler Noteneinträge hat, wenn ja, dann werden ebenso diese Einträge gelöscht. 
        /// </summary>
        public void deleteStudent()
        {
            getStudentID();
            FillNotePropertyIfPossilbe();
            try
            {
                mySqlConnection.Open();
                string query = "DELETE FROM `studentmanagement-db`.`tbl_student` WHERE (`Vorname` = @Vorname);";
                string query2 = "DELETE FROM `studentmanagement-db`.`tbl_note` WHERE (`FK_Student_id` = @StudentID);";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlCommand command2 = new MySqlCommand(query2, mySqlConnection);
                command.Parameters.AddWithValue("@Vorname", tempVorname);
                command2.Parameters.AddWithValue("@StudentID", SchuelerProp.Schueler_ID);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    if (tempVorname == null)
                    {
                        MessageBox.Show("Sie müssen eine Person auswählen","Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (note.Noten != 0)
                        {
                            command2.ExecuteNonQuery(); 
                        }
                        MessageBoxResult result = MessageBox.Show("Möchten Sie wirklich die ausgewählte Person aus dem System entfernen", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            command.ExecuteNonQuery();
                            new PropertyChangedEventArgs("Students");
                        }
                        else if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                selectStudents(); 
                mySqlConnection.Close();
            }
        }
    }
    
}
