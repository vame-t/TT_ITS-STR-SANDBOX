using GalaSoft.MvvmLight.Command;
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
    class MainViewModel_Window2 : INotifyPropertyChanged
    {
        //Implementierung des Interfaces:
        public event PropertyChangedEventHandler PropertyChanged;

        //MySqlConnection aufbauen
        static String connectionString = "SERVER=127.0.0.1;DATABASE=studentmanagement-db;UID=root;PASSWORD=;";
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

        //Ein Button generien soll noch implementiert werden. 

        //Fields: 
        private Schueler schueler = new Schueler();
        private Klasse klasse = new Klasse();
        private ObservableCollection<Schueler> students = new ObservableCollection<Schueler>();
        private ICommand searchCommand;  

        //Properties: 
        public Schueler SchuelerProp
        {
            get { return schueler; }
            set
            {
                schueler = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SchuelerProp"));
            }
        }
        public Klasse KlassenProp
        {
            get { return klasse; }
            set
            {
                klasse = value;
                PropertyChanged(this, new PropertyChangedEventArgs("KlassenProp"));
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

        public MainViewModel_Window2()
        {
            putClassesIntoComboBox();
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
                    KlassenProp.KlassenNamen = "ClassName";
                    KlassenProp.ItemSource = classTable.DefaultView;
                    KlassenProp.KlassenID = "tbl_class_id";

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



    }


}

