using GalaSoft.MvvmLight.CommandWpf;
using MySql.Data.MySqlClient;
using Sandbox.Models;
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
        private ICommand closeCommand;
        private ICommand searchCommand;
        private ICommand deleteCommand;
        private Klasse klasse = new Klasse();
        private Schueler schueler = new Schueler();
        private ObservableCollection<Schueler> students = new ObservableCollection<Schueler>();
        //Properties: 
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                {
                    closeCommand = new RelayCommand(() => closeWindow());
                }
                return closeCommand;
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

        //Konstruktor: 
        public MainViewModel_DeleteStudent()
        {
            putClassesIntoComboBox();

        }

        //Methods: 
        public void closeWindow()
        {
            System.Windows.Application.Current.Shutdown();

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

        public void deleteStudent()
        {
            try
            {
                mySqlConnection.Open();
                string query = "DELETE FROM `studentmanagement-db`.`tbl_student` WHERE (`Vorname` = @Vorname);";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                command.Parameters.AddWithValue("@Vorname", SchuelerProp.Vorname);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {

                    if (SchuelerProp.Vorname == null)
                    {
                        MessageBox.Show("Sie müssen eine Person auswählen","Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
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
