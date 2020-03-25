using System;
using System.ComponentModel;
using Sandbox.Models;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sandbox.Views;
using System.Windows;

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
        private ICommand saveCommand;
        private ICommand nextCommand;
        //Properties:
        public Schueler SchuelerProp
        {
            get { return schueler; }
            set { schueler = value;
                //Welche Eigenschaft soll aktualisiert werden:      
                PropertyChanged(this, new PropertyChangedEventArgs("Schueler"));
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




        //Konstruktor:
        public MainViewModel_Formular()
        {
            
            
        }
        //Command-Methoden: 

        public ICommand SaveCommand
        {
            get {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(() => SaveStudentToDB());
                }
                return saveCommand; }
        }

        public void clickNext()
        {
            notenFormular.Show(); 
        }



        //Methoden:
        public void SaveStudentToDB()
        {
            Formular formular = new Formular(); 
            mySqlConnection.Open();
            string query = "INSERT INTO `zeugnisdb`.`tbl_schüler` (`Nachname`) VALUES(@Nachname);";
            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            formular.txt_Nachname.Text = SchuelerProp.Nachname; 
            command.Parameters.AddWithValue("@Nachname",SchuelerProp.Nachname);
            //command.Parameters.AddWithValue("@Vorname",SchuelerProp.Vorname);
            //command.Parameters.AddWithValue("@Geburtsdatum",formular.txt_GeborenAm.Text);
            //command.Parameters.AddWithValue("@Geburtsort",formular.txt_In.Text);
            //command.Parameters.AddWithValue("@Telefonnummer",formular.txt_Telefon.Text);
            //command.Parameters.AddWithValue("@EMail",formular.txt_EMail.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("Button funktioniert"); 
            mySqlConnection.Close(); 

        }



         



    }
}
