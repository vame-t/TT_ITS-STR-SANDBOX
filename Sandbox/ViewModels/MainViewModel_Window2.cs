using GalaSoft.MvvmLight.Command;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
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
   public class MainViewModel_Window2 : INotifyPropertyChanged
    {
        //Implementierung des Interfaces:
        public event PropertyChangedEventHandler PropertyChanged;

        //MySqlConnection aufbauen
        static String connectionString = "SERVER=127.0.0.1;DATABASE=studentmanagement-db;UID=root;PASSWORD=;";
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

        //Ein Button generien soll noch implementiert werden. 

        //Fields: 
        int currentStudentID = 0;
        //public Schueler schueler = new Schueler();
        private DocGenerieren generieren = new DocGenerieren(); 
        //private Klasse klasse = new Klasse();
        private Note note = new Note();
        private Fach fach = new Fach();
        private ObservableCollection<Schueler> students = new ObservableCollection<Schueler>();
        private ObservableCollection<Note> noten = new ObservableCollection<Note>();
        private ObservableCollection<Fach> fächer = new ObservableCollection<Fach>(); 
        private ICommand searchCommand;
        private ICommand backCommand;
        private ICommand saveCommand;
        private ICommand generateCommand; 

        //Properties: 
        public Action CloseAction { get; set; }
        public Schueler SchuelerProp
        {
            get { return generieren.schueler; }
            set
            {
                generieren.schueler = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SchuelerProp"));
            }
        }
        public Klasse KlassenProp
        {
            get { return generieren.klasse; }
            set
            {
                generieren.klasse = value;
                PropertyChanged(this, new PropertyChangedEventArgs("KlassenProp"));
            }
        }
        public Note NotenProp
        {
            get { return note; }
            set
            {
                note = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NotenProp"));
            }
        }
        public Fach FachProp
        {
            get { return fach; }
            set
            {
                fach = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FachProp"));
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
            get { return noten; }
            set
            {
                noten = value;
                //PropertyChanged(this, new PropertyChangedEventArgs("Noten"));
            }
        }
        public ObservableCollection<Fach> Fächer
        {
            get { return fächer; }
            set
            {
                fächer = value;
            }
        }


        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand= new RelayCommand(() => searchStudents());
                }
                return searchCommand;
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
        public ICommand SaveCommand
        {
            get 
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(() => saveGradesIntoDB());
                }
                return saveCommand;
            }
            
        }
        public ICommand GenerateCommand
        {
            get
            {
                if (generateCommand == null)
                {
                    generateCommand = new RelayCommand(() => generieren.docGenerieren());
                }
                return generateCommand;
            }

        }

        //Constructor:
        public MainViewModel_Window2()
        {
            putClassesIntoComboBox();
            putFächerIntoCombobox();
            Noten = new ObservableCollection<Note>()
            {
                new Note(){Noten = 1, Bezeichnung = "Sehr gut"},
                new Note(){Noten = 2, Bezeichnung = "Gut"},
                new Note(){Noten = 3, Bezeichnung = "Befriedigend"},
                new Note(){Noten = 4, Bezeichnung = "Ausreichend"},
                new Note(){Noten = 5, Bezeichnung = "Mangelhaft"},
                new Note(){Noten = 6, Bezeichnung = "Ungenügend"},
            };
        


        }

        //Methods: 
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
                    KlassenProp.KlassenNamen = "ClassName";
                    KlassenProp.ItemSource = classTable.DefaultView;
                    KlassenProp.KlassenID = "tbl_class_id";

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        public void putFächerIntoCombobox()
        {
            {
                try
                {

                    string query = "SELECT * FROM `studentmanagement-db`.tbl_fach;";
                    MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection);

                    using (sqlDataAdapter)
                    {
                        DataTable fachTable = new DataTable();
                        sqlDataAdapter.Fill(fachTable);
                        FachProp.FachName= "Name";
                        FachProp.ItemSource= fachTable.DefaultView;
                        FachProp.FachID = "tbl_Fach_id";

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace);
                }
            }
        }

        public void searchStudents()
        {
            try
            {
                mySqlConnection.Open();
                string query = "SELECT * FROM `studentmanagement-db`.tbl_student WHERE FK_Klasse= @FK_Klasse;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@FK_Klasse", KlassenProp.KlassenIDFK);
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
                        //generieren.schueler.Schueler_ID = (int)(dataRow["tbl_student_id"]);
                        nschueler.Vorname = Convert.ToString(dataRow["Vorname"]);
                        //generieren.schueler.Nachname = Convert.ToString(dataRow["Nachname"]);
                       
                        
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

        public void backToMainWindow()
        {
            FirstWindow mainWindow = new FirstWindow();
            mainWindow.Show();
            CloseAction();
            //System.Windows.Application.Current.Shutdown();

        }

        public void saveGradesIntoDB()
        {
            
            try
            {
                mySqlConnection.Open();
                currentStudentID  = pickStudentID();
                string query = "INSERT INTO tbl_note (`Note`, `FK_Fach_id`, `FK_Student_id`) VALUES(@Note,@FK_Fach_id,@FK_Student_id);";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                    command.Parameters.AddWithValue("@Note", NotenProp.Noten);
                    command.Parameters.AddWithValue("@FK_Fach_id",FachProp.Fach_ID);
                    command.Parameters.AddWithValue("@FK_Student_id",currentStudentID);
                    command.ExecuteNonQuery();
                MessageBox.Show("Note wurde für das ausgewählte Fach eingetragen, bitte beachten Sie, dass momentan mehrere Einträge zum selben Fach möglich sind. Dies sollte unterlassen werden. Tragen Sie noch die restlichen Noten ein, um die Generierung zu vervollständigen.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public int pickStudentID()
        {
            
            int currentStudentID = 0;
            string query = "SELECT * FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
            using (sqlDataAdapter)
            {
                command.Parameters.AddWithValue("@Vorname", SchuelerProp.Vorname);
                currentStudentID = (Int32)command.ExecuteScalar(); 

            }
            return currentStudentID;
            
        }

    }
}

