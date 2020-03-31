using GalaSoft.MvvmLight.Command;
using MySql.Data.MySqlClient;
using Sandbox.Models;
using System;
using System.Collections.Generic;
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

        //MySqlConnection aufbauen
        static String connectionString = "SERVER=127.0.0.1;DATABASE=studentmanagement-db;UID=root;PASSWORD=;";
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

        //Fields: 
        private ICommand closeCommand;
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
        public Klasse KlasseProp
        {
            get { return klasse; }
            set { klasse = value;
                PropertyChanged(this, new PropertyChangedEventArgs("KlasseProp"));
            }
        }
        public Schueler SchuelerProp
        {
            get { return schueler; }
            set { schueler = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SchuelerProp"));
            }
        }

        //Konstruktor: 
        public MainViewModel_DeleteStudent()
        {
            putClassesIntoComboBox();
            listStudentsFromClass(); 
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

        public void listStudentsFromClass()
        {

            try
            {
            mySqlConnection.Open();
                if (KlasseProp.KlassenIDFK != 0)
            {
            string query = "SELECT Vorname FROM `studentmanagement-db`.tbl_student";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, mySqlConnection);
            using (sqlDataAdapter)
                {
                DataTable studentTable = new DataTable();
                sqlDataAdapter.Fill(studentTable);
                SchuelerProp.Vorname = "Vorname";
                SchuelerProp.ItemSource = studentTable.DefaultView;
                SchuelerProp.SchuelerID = "tbl_student_id"; 

                }
            }

            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}
