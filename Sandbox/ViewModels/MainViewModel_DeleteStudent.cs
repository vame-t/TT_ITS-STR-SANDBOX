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
                    var result = studentTable;
                    SchuelerProp.Vorname = "Vorname";
                    SchuelerProp.SchuelerID = "tbl_student_id";
                    SchuelerProp.ItemSource = studentTable.DefaultView;
                    NotifyOfPropertyChange("SchuelerProp");
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

        public void deleteStudent()
        {
            try
            {
                mySqlConnection.Open();
                              //DELETE FROM `studentmanagement-db`.`tbl_student` WHERE(`tbl_student_id` = '31');
                string query = "DELETE FROM `studentmanagement-db`.`tbl_student` WHERE (`tbl_student_id` = @tbl_student_id);";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                SchuelerProp.SchuelerID = "tbl_student_id";
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@tbl_student_id", SchuelerProp.Schueler_ID);
                    command.ExecuteNonQuery();
                    NotifyOfPropertyChange("SchuelerProp");
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


    }


    //private String mySelectedItem;
    //private ObservableCollection<Klasse> values = new ObservableCollection<Klasse>()
    //{
    //  //Hier sollen alles Klassen eingetragen werden also die Klassen Namen 

    //};
    // public String MySelectedItem
    // {
    //     get { return mySelectedItem; }
    //     set
    //     {
    //         if (KlasseProp.KlassenIDFK >=1)
    //         {
    //             string query = "SELECT Vorname FROM `studentmanagement-db`.tbl_student;";
    //             MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection);
    //             using (sqlDataAdapter)
    //             {
    //                 DataTable studentTable = new DataTable();
    //                 sqlDataAdapter.Fill(studentTable);
    //                 SchuelerProp.Vorname = "Vorname";
    //                 SchuelerProp.ItemSource = studentTable.DefaultView;
    //                 SchuelerProp.SchuelerID = "tbl_student_id";

    //             }
    //         }
    //         mySelectedItem = value;
    //         PropertyChanged(this, new PropertyChangedEventArgs("MySelectedItem"));
    //     }
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
